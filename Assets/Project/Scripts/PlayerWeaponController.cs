using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponController : MonoBehaviour
{
    [Tooltip("Assign for now, will be delegated to system mechanic later.")]
    [SerializeField]
    private BTool activeTool = null;
    public void OnPrimaryInputDown() { if (activeTool) activeTool.OnPrimaryInputDown(); }
    public void OnSecondaryInputDown() { if (activeTool) activeTool.OnSecondaryInputDown(); }
    public void OnPrimaryInputRelease() { if (activeTool) activeTool.OnPrimaryInputRelease(); }
    public void OnSecondaryInputRelease() { if (activeTool) activeTool.OnSecondaryInputRelease(); }
    public void OnReloadInput() { if (activeTool) activeTool.OnReloadInput(); }

}
