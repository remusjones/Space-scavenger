using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
[RequireComponent(typeof(Rigidbody))]
public class PlayerController : MonoBehaviour, IDamageable
{

    [Header("Player Controls and Settings")]
    public bool _Seated = false;

    public float camSens = 0.25f; //How sensitive it with mouse
    public float turnSpeed = 1.0f;
    
    public float moveSpeed = 2.0f;

    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 90.0f;
    private float rotX;

    private Vector2 residualVelocity = Vector2.zero;
    private Vector2 lastMouse = Vector2.zero;

    [SerializeField]
    float residualMagnitudeLimit = 0.1f;
    [SerializeField]
    float residualDivider = 15f;
    [SerializeField]
    float residualDeductionMultiplier = 5f;
    private Rigidbody rb;
    [SerializeField]
    private float _StartDirectionAccelleration;
    float directionAccel = 100f;
    [SerializeField]
    float pitchAccel = 80f;


    [Header("Player Components")]
    [SerializeField]
    private AudioSource movementSound = null;
    private VignetteController vignetteController = null;
    public Image _InteractMarker;
    public Image _DefaultMarker;

    [Header("Player Health")]
    [SerializeField]
    private float health = 100f;
    private float maxHealth = 100f;
    [SerializeField]
    private float magnitudeDamageVelocity = 30f;
    [SerializeField]
    private float velocityDamageFloor = 2f;
    [SerializeField]
    private float velocityDamageMultiplier = 0.1f;

    [Header("UI Settings")]
    public float _InteractRange;
    public LayerMask _InteractLayer;
    public float maxDisplayAngle = 20f;
    private GameObject lastDisplayedObject = null;
    private IEnumerator updateObjectDisplay = null;
    public Vector2 displayOffset = Vector2.zero;
    
    [SerializeField]
    private CanvasRenderer uiDescription = null;
    [SerializeField]
    private TMPro.TMP_Text textDescription = null;
    [SerializeField]
    private Camera playerCamera = null;
    private Canvas descriptionCanvas = null;
    private RectTransform canvasTransform = null;
    private RectTransform uiDescriptionTransform = null;
    

#if UNITY_EDITOR
    [Header("Debug")]
    [SerializeField]
    private float lastHitMagnitude = 0f;
#endif

    private void Start()
    {
        _StartDirectionAccelleration = directionAccel;
        rb = this.GetComponent<Rigidbody>();
        vignetteController = GameObject.FindObjectOfType<VignetteController>();

        if (textDescription != null)
        {
            descriptionCanvas = textDescription.canvas;
            canvasTransform = textDescription.canvas.GetComponent<RectTransform>();
        }
        if (uiDescription !=null)
            uiDescriptionTransform = uiDescription.GetComponent<RectTransform>();
    }


    private void Update()
    {
        if (_Seated)
        {
            rb.isKinematic = true;
            directionAccel = 0;
            if (InteractionButtonDown())
            {
                _Seated = false;
            }
        }
        else
        {
            rb.isKinematic = false;
            directionAccel = _StartDirectionAccelleration;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb.angularVelocity -= rb.angularVelocity / 10;
        float mX = Input.GetAxis("Mouse X");
        float mY = Input.GetAxis("Mouse Y");
        Vector2 currMouse = new Vector2(mX, mY);
        if ((currMouse - lastMouse).magnitude >= residualMagnitudeLimit)
        {
            residualVelocity = (currMouse + lastMouse) / residualDivider;
        }else
        {
            residualVelocity -= ((residualVelocity * residualDeductionMultiplier) * Time.deltaTime);
        }

        if (vignetteController)
        {

            vignetteController.HandleTurnAcceleration(residualVelocity);
        }

         float y = ((mX + residualVelocity.x) * turnSpeed);
         rotX = ((mY + residualVelocity.y) * turnSpeed);
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);
        this.transform.Rotate(Vector3.up, y, Space.Self);
       // //rotate around local x;
        this.transform.Rotate(Vector3.right, -rotX, Space.Self);


        lastMouse = currMouse;
        // input

        if (rb)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("UpDown"), Input.GetAxis("Vertical"));
            float pitch = Input.GetAxis("Pitch");
            if (movement.magnitude > 0f)
            {
                if (!movementSound.isPlaying)
                    movementSound.Play();

                Vector3 soundOffset = movement;
                soundOffset.z += pitch;
                movementSound.transform.localPosition = soundOffset * -1f;
                movementSound.volume = movement.magnitude / 1000f;
            }else
                movementSound.Stop();

            rb.AddRelativeTorque((new Vector3(0, 0, -pitch) * pitchAccel) * Time.fixedDeltaTime, ForceMode.Acceleration);
            rb.AddRelativeForce((new Vector3(movement.x, movement.y, movement.z) * directionAccel) * Time.fixedDeltaTime, ForceMode.Force);

            rb.angularVelocity = new Vector3(rb.angularVelocity.x +  (residualVelocity.y / 10), rb.angularVelocity.y + (residualVelocity.x / 10), rb.angularVelocity.z);

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                if (!movementSound.isPlaying)
                {
                    movementSound.transform.localPosition = Vector3.zero;
                    movementSound.Play();

                }
                rb.velocity = rb.velocity - ((rb.velocity * 4) * Time.deltaTime);  
            }
        }


      
    }

    IEnumerator DisplayObject(IDescription desc, float maxAngle)
    {

        EnableDisplay();
        bool angleExceeded = false;
        while (angleExceeded == false)
        {
            if (!textDescription || !uiDescription || !playerCamera || !lastDisplayedObject)
                yield return null;

            Vector3 angleAxis = lastDisplayedObject.transform.position - this.transform.position;
            float angle = Vector3.Angle(this.transform.forward, angleAxis);

            MoveToWorldPoint(lastDisplayedObject.transform.position, uiDescriptionTransform, (displayOffset ) + (new Vector2(Screen.width, Screen.height)/2));
            textDescription.text = desc.GetPrintable();

            if (angle > maxAngle)
            {
                angleExceeded = true;

                ResetDisplay();
                lastDisplayedObject = null;
            }

            yield return new WaitForEndOfFrame();
        }
    }

    public void MoveToWorldPoint(Vector3 objectTransformPosition, RectTransform rectTransform, Vector2 offset)
    {
        Vector2 sizeDelta = canvasTransform.sizeDelta;
        Vector2 ViewportPosition = playerCamera.WorldToViewportPoint(objectTransformPosition);
        Vector2 proportionalPosition = new Vector2(ViewportPosition.x * sizeDelta.x, ViewportPosition.y * sizeDelta.y);

        rectTransform.localPosition = proportionalPosition - offset;
    }
    private void ResetDisplay()
    {
        // animations and such for ui here
        uiDescription.gameObject.SetActive(false);
    }
    private void EnableDisplay()
    {
        // animations and such for ui here
        uiDescription.gameObject.SetActive(true);
    }
    private void LateUpdate()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _InteractRange, _InteractLayer))
        {
            IObjectInteract objectInteract = hit.collider.GetComponent<IObjectInteract>();
            IDescription objectDescription = hit.collider.GetComponent<IDescription>();
            if (objectInteract != null)
            {
                _InteractMarker.enabled = true;
                _DefaultMarker.enabled = false;
                if (InteractionButtonDown())
                {
                    objectInteract.Interact(this);
                }
                if (objectDescription != null)
                {
                    // update current display
                    if (lastDisplayedObject != hit.collider.gameObject)
                    {
                    
                        if (updateObjectDisplay != null && lastDisplayedObject != hit.collider.gameObject)
                        {
                            StopCoroutine(updateObjectDisplay);
                            updateObjectDisplay = null;
                            ResetDisplay();
                        }

                        lastDisplayedObject = hit.collider.gameObject;
                        StartCoroutine(DisplayObject(objectDescription, maxDisplayAngle));
                    }
                }
            }
            else
            {
                _InteractMarker.enabled = false;
                _DefaultMarker.enabled = true;
            }



        }
        Debug.DrawRay(transform.position, transform.forward);
    }
    bool InteractionButton()
    {
        return Input.GetKey(KeyCode.E);
    }
    bool InteractionButtonDown()
    {
        return Input.GetKeyDown(KeyCode.E);
    }

    public void Damage(float damage)
    {
        float final = Mathf.Clamp(health - damage, 0, maxHealth);
        // if (final == 0f) // handle death .. 
        health = final;
    }

    public void OnCollisionEnter(Collision collision)
    {
        float magnitude = (rb.velocity + rb.angularVelocity).magnitude;
        Rigidbody otherRigidBody = collision.gameObject.GetComponent<Rigidbody>();
        if (otherRigidBody)
        {
            magnitude += (otherRigidBody.velocity + otherRigidBody.angularVelocity).magnitude;
        }

        if (magnitude > magnitudeDamageVelocity)
        {
            // handle helmet break, etc.. 
            this.Damage(velocityDamageFloor * (magnitude * velocityDamageMultiplier));
            Debug.Log("Player took: " + (velocityDamageFloor * (magnitude * velocityDamageMultiplier)) + " Damage");
            
        }
#if UNITY_EDITOR
        lastHitMagnitude = magnitude;
#endif
    }
}
