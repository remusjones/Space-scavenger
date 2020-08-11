using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[System.Serializable]
public class ChangeToolTypeEvent : UnityEvent<BTool> { }

public class PlayerWeaponController : MonoBehaviour
{
    [Tooltip("Assign for now, will be delegated to system mechanic later.")]
    [SerializeField]
    private BTool activeTool = null;
    [SerializeField]
    private Transform toolLocationTransform = null;


    [SerializeField]
    private Rigidbody playerRigidbody;
    [SerializeField]
    private Camera playerCamera;
    [SerializeField]
    private LayerMask layermask;
    public void OnChangeTool(BTool newTool)
    {
        Vector3 newToolScale = newTool.transform.localScale;
        // replace old tool, with our current.. 
        if (activeTool)
        {
            activeTool.transform.parent = newTool.transform.parent;
            activeTool.transform.position = newTool.transform.position;
            activeTool.transform.rotation = newTool.transform.rotation;
            activeTool.transform.localScale = newToolScale;
        }
        if (newTool) 
        {
            activeTool = newTool;
            activeTool.transform.parent = toolLocationTransform;
            activeTool.transform.position = toolLocationTransform.position;
            activeTool.transform.rotation = toolLocationTransform.rotation;


            // setup defaults.. 
            activeTool.SetDefaults(playerRigidbody, playerCamera, layermask);
        }

    }
    public void OnDropTool(Transform positionToDrop) { } // will place at location and parent.
    public void OnDropTool() { } // will drop from hand. 
    public void OnDropTool(Vector3 pos) { } // will place at location 
    private void Update()
    {
        if (activeTool)
            activeTool.UpdateTool();
    }
    
    public void OnPrimaryInputDown() { if (activeTool) activeTool.OnPrimaryInputDown(); }
    public void OnSecondaryInputDown() { if (activeTool) activeTool.OnSecondaryInputDown(); }
    public void OnPrimaryInputRelease() { if (activeTool) activeTool.OnPrimaryInputRelease(); }
    public void OnSecondaryInputRelease() { if (activeTool) activeTool.OnSecondaryInputRelease(); }
    public void OnReloadInput() { if (activeTool) activeTool.OnReloadInput(); }

}
