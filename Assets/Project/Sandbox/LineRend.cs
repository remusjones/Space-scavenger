using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRend : MonoBehaviour
{
    public Transform _Point1, _Point2;
    private LineRenderer _LR;

    private void Start()
    {
        _LR = GetComponent<LineRenderer>();
    }
    // Update is called once per frame
    void LateUpdate()
    {
        _LR.SetPosition(0,_Point1.position);
        _LR.SetPosition(1, _Point2.position);
    }
}
