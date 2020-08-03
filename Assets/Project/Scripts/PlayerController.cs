using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float _InteractRange;
    public LayerMask _InteractLayer;


    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _InteractRange, _InteractLayer))
        {
            Debug.Log(hit.collider.name);
            if (hit.collider.GetComponent<IObjectInteract>() != null)
            {
                if(MouseDown())
                    hit.collider.GetComponent<IObjectInteract>().Interact();
            }
        }
        Debug.DrawRay(transform.position, transform.forward);
    }

    bool MouseDown()
    {
        return Input.GetMouseButtonDown(0);
    }
}
