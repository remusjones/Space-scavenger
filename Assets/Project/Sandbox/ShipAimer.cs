using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAimer : MonoBehaviour
{
    public float _Speed;
    float posX = 0, posY = 0;
    public Ship _Ship;
    // Update is called once per frame
    void Update()
    {
        Vector3 position = this.transform.InverseTransformDirection(_Ship.GetComponent<Rigidbody>().angularVelocity);
        posX = position.y * _Speed;
        posY = position.x * _Speed;
        transform.localPosition = new Vector2(posX , posY);
    }
}
