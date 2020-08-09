using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MatchPosition : MonoBehaviour
{
    public Transform _Target;

    // Update is called once per frame
    void FixedUpdate()
    {
        transform.SetPositionAndRotation(_Target.position, _Target.rotation);
    }
}
