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
        if (Vector3.Distance(_Point1.position, _Point2.position) > 0.05f)
            _LR.enabled = true;
        else
            _LR.enabled = false;
        _LR.SetPosition(0, _Point1.position);
        _LR.SetPosition(1, _Point2.position);

    }
}
