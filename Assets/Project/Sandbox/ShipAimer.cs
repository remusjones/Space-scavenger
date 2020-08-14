using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAimer : MonoBehaviour
{
    public float _Speed;
    float posX = 0, posY = 0;
    public Ship _Ship;
    Rigidbody shipRigidbody = null;
    private void Start()
    {
        shipRigidbody = _Ship.GetComponent<Rigidbody>();
    }
    // Update is called once per frame
    void Update()
    {
        Vector3 position = this.transform.InverseTransformDirection(shipRigidbody.angularVelocity);
        posX = position.y * _Speed;
        posY = -position.x * _Speed;
        transform.localPosition = new Vector2(posX , posY);
    }
}
