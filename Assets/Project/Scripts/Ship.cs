using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour
{

    public bool isPlayerDriving = false;

    // Start is called before the first frame update
    Vector2 lastMouse = new Vector2();
    Rigidbody rb = null;

    [SerializeField]
    Vector3 input;
    [SerializeField]
    Vector2 currMouse;
    [SerializeField]
    Vector2 mouseInput;
    private Vector2 residualVelocity = Vector2.zero;

    private float rotX;

    [SerializeField]
    float residualMagnitudeLimit = 0.1f;
    [SerializeField]
    float residualDivider = 50f;
    [SerializeField]
    float residualDeductionMultiplier = 5f;
    [SerializeField]
    float directionAccel = 100f;
    [SerializeField]
    float pitchAccel = 100f;

    [SerializeField]
    float deaccelSpeed = 1f;


    public float turnSpeed = 1.0f;
    public float moveSpeed = 2.0f;
    public float minTurnAngle = -90.0f;
    public float maxTurnAngle = 90.0f;
    public float angularVelocityReduction = 100f;

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
            return;


        Cursor.lockState = CursorLockMode.Locked;
        rb.angularVelocity -= rb.angularVelocity / angularVelocityReduction;
        float mX = Input.GetAxis("Mouse X");
        float mY = Input.GetAxis("Mouse Y");
        float pitch = Input.GetAxis("Pitch");

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
       // this.transform.Rotate(Vector3.up, y, Space.Self);
       // // //rotate around local x;
       // this.transform.Rotate(Vector3.right, -rotX, Space.Self);


        lastMouse = currMouse;
        // input

        if (rb)
        {
            Vector3 movement = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("UpDown"), Input.GetAxis("Vertical"));
            pitch = Input.GetAxis("Pitch");

            rb.AddRelativeTorque((new Vector3(0, 0, -pitch) * pitchAccel) * Time.fixedDeltaTime, ForceMode.Acceleration);
            rb.AddRelativeForce((new Vector3(movement.x, movement.y, movement.z) * directionAccel) * Time.fixedDeltaTime, ForceMode.Force);
            rb.AddRelativeTorque(-(residualVelocity.y), (residualVelocity.x), 0f);
            //rb.angularVelocity = new Vector3(rb.angularVelocity.x + (residualVelocity.y / 10), rb.angularVelocity.y + (residualVelocity.x / 10), rb.angularVelocity.z);

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
