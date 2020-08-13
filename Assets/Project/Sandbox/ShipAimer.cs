using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAimer : MonoBehaviour
{
    public float _Speed;
    RectTransform _Transform;
    float posX = 0, posY = 0;
    public Ship _Ship;
    // Start is called before the first frame update
    void Start()
    {
        _Transform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 AngularVelocity = Vector3.Cross(_Ship.transform.forward, _Ship.GetComponent<Rigidbody>().angularVelocity);
        //Vector2 AngularVelocity = _Ship.GetComponent<Rigidbody>().angularVelocity;
        posX = AngularVelocity.x * _Speed;//-Input.GetAxis("Mouse X") * _Speed * Time.deltaTime;
        posY = AngularVelocity.y * _Speed;//-Input.GetAxis("Mouse Y") * _Speed * Time.deltaTime;

        //posX = Mathf.Clamp(posX, -200, 200);
        //posY = Mathf.Clamp(posY, -200, 200);
        transform.localPosition = new Vector2(posX , posY);
        //transform.localPosition = Input.mousePosition;
    }
}
