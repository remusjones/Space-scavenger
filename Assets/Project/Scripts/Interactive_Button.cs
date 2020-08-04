using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Interactive_Button : MonoBehaviour,IObjectInteract
{
    public UnityEvent _ButtonEvent;
    public Animator _ButtonAnimator;
    public void Interact(PlayerController player)
    {
        _ButtonAnimator.SetTrigger("Trigger");
        _ButtonEvent.Invoke();
    }
}
