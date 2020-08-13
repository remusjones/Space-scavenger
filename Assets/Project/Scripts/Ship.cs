using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour
{

    [Tooltip("Used for testing (will be deprecated) if the user is controlling the ship")]
    public bool isPlayerDriving = false;

    Vector2 lastMouse = new Vector2();
    Rigidbody rb = null;
    private Vector2 residualVelocity = Vector2.zero;

    private float rotX;

    [Tooltip("How long the mouse movement has to be, before the residual velocity is modified | (currMouse - lastMouse).magnitude >= residualMagnitudeLimit")]
    [SerializeField]
    float residualMagnitudeLimit = 0.1f;

    [Tooltip("How much the mouse movement magnitude influences the ships residual velocity, when the user moves the mouse | residualVelocity = (currMouse + lastMouse) / residualDivider;")]
    [SerializeField]
    float residualDivider = 50f;

    [Tooltip("Used for quickness of mouse movement deaccel | residualVelocity -= ((residualVelocity * residualDeductionMultiplier) * Time.deltaTime)")]
    [SerializeField]
    float residualDeductionMultiplier = 5f;

    [Tooltip("Overall Speed for WASD movement keys.")]
    [SerializeField]
    float directionAccel = 100f;

    [Tooltip("Rigidbody Pitch is multiplied by this value | (new Vector3(0, 0, -pitch) * pitchAccel)")]
    [SerializeField]
    float pitchAccel = 100f;

    [Tooltip("When the user is pressing the Slow Key the speed is reduced, and multiplied by this value | rb.velocity - ((rb.velocity * deaccelSpeed) * Time.deltaTime)")]
    [SerializeField]
    float deaccelSpeed = 1f;

    [Tooltip("Turn Speed Multiplier for mouse axis | (mX + residualVelocity.x) * turnSpeed)")]
    public float turnSpeed = 1.0f;
    [Tooltip("Min clamp angle for X mouse axis.")]
    public float minTurnAngle = -90.0f;
    [Tooltip("Max clamp angle for X mouse axis.")]
    public float maxTurnAngle = 90.0f;
    [Tooltip("The dividend per frame to slow down angular velocity | (rb.angularVelocity -= rb.angularVelocity / angularVelocityReduction)")]
    public float angularVelocityReduction = 100f;


    [Tooltip("Used for accelerting specific vectors (except backwards) | new Vector3(movement.x * movementSpeedPerAxis.x, movement.y * movementSpeedPerAxis.y, movement.z * movementSpeedPerAxis.z)")]
    public Vector3 movementSpeedPerAxis = new Vector3(0.1f, 0.1f, 1f);
    [Tooltip("Used as a speed multiplier when moving backwards | movement.z = Mathf.Clamp(movement.z, -maxBackwardsThrust, 0f)")]
    public float maxBackwardsThrust = 0.1f;

    void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    // up down - space/shift # updown
    // pitch - mouse up/down # mouse
    // yaw - a/d # horizontal 
    // foward/back - w/s # vertical
    // turn - mouse horizontal # mouse
    // Update is called once per frame
    private void FixedUpdate()
    {
        if (!isPlayerDriving)
        {
            if (rb.velocity.magnitude == 0.0f && rb.angularVelocity.magnitude == 0.0f)
                rb.isKinematic = true;
            else
                rb.isKinematic = false;
            return;
        }
        else
            rb.isKinematic = false;


        Cursor.lockState = CursorLockMode.Locked;
        rb.angularVelocity -= rb.angularVelocity / angularVelocityReduction;
        float mX = Input.GetAxis("Mouse X");
        float mY = Input.GetAxis("Mouse Y");
        float pitch = Input.GetAxis("Pitch");
        Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("UpDown"), Input.GetAxis("Vertical"));
        Vector2 currMouse = new Vector2(mX, mY);
        if ((currMouse - lastMouse).magnitude >= residualMagnitudeLimit)
        {
            residualVelocity = (currMouse + lastMouse) / residualDivider;
        }
        else
        {
            residualVelocity -= ((residualVelocity * residualDeductionMultiplier) * Time.deltaTime);
        }


        float y = ((mX + residualVelocity.x) * turnSpeed);
        rotX = ((mY + residualVelocity.y) * turnSpeed);
        rotX = Mathf.Clamp(rotX, minTurnAngle, maxTurnAngle);

        lastMouse = currMouse;
        // input

        if (rb)
        {
            

            rb.AddRelativeTorque((new Vector3(0, 0, -pitch) * pitchAccel) * Time.fixedDeltaTime, ForceMode.Acceleration);
            if (movement.z < 0f)
                movement.z = Mathf.Clamp(movement.z, -maxBackwardsThrust, 0f);

            rb.AddRelativeForce((new Vector3(movement.x * movementSpeedPerAxis.x, movement.y * movementSpeedPerAxis.y, movement.z * movementSpeedPerAxis.z) * directionAccel) * Time.fixedDeltaTime, ForceMode.Acceleration);
            rb.AddRelativeTorque(-(residualVelocity.y), (residualVelocity.x), 0f,ForceMode.Acceleration);

            if (Input.GetKey(KeyCode.LeftAlt))
            {
                rb.velocity = rb.velocity - ((rb.velocity * deaccelSpeed) * Time.deltaTime);
            }
        }
    }
    public void EnableShipControls(bool isEnabled)
    {
        isPlayerDriving = isEnabled;
    }
}
