using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IDock
{
    bool CanDock(Vector3 location, float checkDistance, Transform closestDock);
    void HandleMagnatise(Rigidbody rigidbody, Transform target);
}
