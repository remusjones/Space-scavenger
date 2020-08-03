using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float _InteractRange;
    public LayerMask _InteractLayer;

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
    float directionAccel = 100f;
    [SerializeField]
    float pitchAccel = 80f;

    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {

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

         float y = ((mX + residualVelocity.x) * turnSpeed);
         rotX += ((mY + residualVelocity.y) * turnSpeed);
        // float y = ((mX) * turnSpeed);
         //rotX += ((mY) * turnSpeed);
        // clamp the vertical rotation
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);
        // rotate the camera
        transform.eulerAngles = new Vector3(-rotX, transform.eulerAngles.y + y, 0);
        lastMouse = currMouse;
        // input

        if (rb)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("UpDown"), Input.GetAxis("Vertical"));

            float pitch = Input.GetAxis("Pitch");
            rb.AddRelativeTorque((new Vector3(0, pitch, 0) * pitchAccel) * Time.fixedDeltaTime, ForceMode.Force);
            rb.AddRelativeForce((new Vector3(movement.x, movement.y, movement.z) * directionAccel) * Time.fixedDeltaTime, ForceMode.Force);

            rb.angularVelocity = residualVelocity;

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                rb.velocity = rb.velocity / 4;
            }
        }


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _InteractRange, _InteractLayer))
        {
            if (hit.collider.GetComponent<IObjectInteract>() != null)
            {
                if(MouseDown())
                    hit.collider.GetComponent<IObjectInteract>().Interact();
            }
        }
        Debug.DrawRay(transform.position, transform.forward);
    }

    bool MouseDown()
    {
        return Input.GetMouseButtonDown(0);
    }
}
