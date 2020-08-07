using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Interactive_Button : MonoBehaviour, IObjectInteract, IDescription
{
    public UnityEvent _ButtonEvent;
    public Animator _ButtonAnimator;

    private string displayText = "Button";
    public string GetPrintable()
    {
        // potentially display connected wires 
        /*
         * - Button
         * - Pin1: Door
         * - Pin2: Oxygen
         */
        return displayText;
    }

    public void Interact(PlayerController player)
    {
        _ButtonAnimator.SetTrigger("Trigger");
        _ButtonEvent.Invoke();
    }
}
