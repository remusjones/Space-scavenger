using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(Rigidbody))]
public class Ship : MonoBehaviour
{

    public bool isPlayerDriving = false;

    // Start is called before the first frame update
    Vector2 lastMouseFrame = new Vector2();
    Rigidbody rb = null;

    [SerializeField]
    Vector3 input;
    [SerializeField]
    Vector2 currMouse;
    [SerializeField]
    Vector2 mouseInput;

    [SerializeField]
    float directionAccel = 100f;
    [SerializeField]
    float pitchAccel = 80f;

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
        input = new Vector3(Input.GetAxis("Horizontal"), Input.GetAxis("UpDown"), Input.GetAxis("Vertical"));
        float pitch = Input.GetAxis("Pitch");
        currMouse = Input.mousePosition;
        mouseInput = (lastMouseFrame - currMouse);


        rb.AddRelativeTorque((new Vector3(0, pitch, 0) * pitchAccel) * Time.fixedDeltaTime, ForceMode.Force);
        rb.AddRelativeForce((new Vector3(input.x, input.y, input.z) * directionAccel) * Time.fixedDeltaTime, ForceMode.Force);
        lastMouseFrame = currMouse;
        if (Input.GetKey(KeyCode.LeftAlt))
        {
            rb.velocity = rb.velocity / 4;
        }
    }
}
