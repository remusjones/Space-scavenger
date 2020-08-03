using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AirlockDocking : MonoBehaviour, IDock
{
    [SerializeField]
    float m_magnitizeValue = 1f;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    bool IDock.CanDock(Vector3 location, float checkDistance, Transform closestDock)
    {
        return false;
    }
    void IDock.InitateDock(Transform ourObject, Transform otherObject)
    {

    }
}
