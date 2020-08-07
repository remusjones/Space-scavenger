using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BGrabbable : MonoBehaviour, IGrabbable
{
    private Rigidbody rb = null;
    private void Start()
    {
        rb = this.GetComponent<Rigidbody>();
    }
    public Rigidbody GetRigidbody()
    {
        return rb;
    }
}
