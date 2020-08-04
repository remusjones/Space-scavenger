﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public bool _Seated = false;
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
    private float _StartDirectionAccelleration;
    float directionAccel = 100f;
    [SerializeField]
    float pitchAccel = 80f;

    private void Start()
    {
        _StartDirectionAccelleration = directionAccel;
        rb = this.GetComponent<Rigidbody>();
    }
    public Image _InteractMarker;
    public Image _DefaultMarker;

    private void Update()
    {
        if (_Seated)
        {
            maxTurnAngle = 45;
            minTurnAngle = -45;
            rb.isKinematic = true;
            directionAccel = 0;
            if (InteractionButtonDown())
            {
                _Seated = false;
            }
        }
        else
        {
            maxTurnAngle = 90;
            minTurnAngle = -90;
            rb.isKinematic = false;
            directionAccel = _StartDirectionAccelleration;
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        Cursor.lockState = CursorLockMode.Locked;

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
         rotX = ((mY + residualVelocity.y) * turnSpeed);
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);
        this.transform.Rotate(Vector3.up, y, Space.Self);
        //rotate around local x;
        this.transform.Rotate(Vector3.right, -rotX, Space.Self);


        lastMouse = currMouse;
        // input

        if (rb)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("UpDown"), Input.GetAxis("Vertical"));

            float pitch = Input.GetAxis("Pitch");
            rb.AddRelativeTorque((new Vector3(0, 0, -pitch) * pitchAccel) * Time.fixedDeltaTime, ForceMode.Force);
            rb.AddRelativeForce((new Vector3(movement.x, movement.y, movement.z) * directionAccel) * Time.fixedDeltaTime, ForceMode.Force);

            rb.angularVelocity = residualVelocity;

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                rb.velocity = rb.velocity - ((rb.velocity * 4) * Time.deltaTime);  
            }
        }


        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _InteractRange, _InteractLayer))
        {
            if (hit.collider.GetComponent<IObjectInteract>() != null)
            {
                _InteractMarker.enabled = true;
                _DefaultMarker.enabled = false;
                if (InteractionButton())
                {
                    hit.collider.GetComponent<IObjectInteract>().Interact(this);
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
}
