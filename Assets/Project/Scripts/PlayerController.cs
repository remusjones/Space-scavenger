using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class PlayerController : MonoBehaviour
{
    public float _InteractRange;
    public LayerMask _InteractLayer;

    public Image _InteractMarker;
    public Image _DefaultMarker;
    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.forward, out hit, _InteractRange, _InteractLayer))
        {
            if (hit.collider.GetComponent<IObjectInteract>() != null)
            {
                _InteractMarker.enabled = true;
                if(MouseDown())
                    hit.collider.GetComponent<IObjectInteract>().Interact();
            }
            else
            {
                _InteractMarker.enabled = false;
            }
        }
        Debug.DrawRay(transform.position, transform.forward);
    }

    bool MouseDown()
    {
        return Input.GetMouseButtonDown(0);
    }
}
