using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;



[System.Serializable]
public class InputHeldEvent : UnityEvent<float>
{

}
public class PlayerInputController : MonoBehaviour
{

    [Header("Keybinds")]
    [Tooltip("The Reload Key")]
    [SerializeField]
    private KeyCode reloadKey = KeyCode.R;
    [Tooltip("The switch weapon Key")]
    [SerializeField]
    private KeyCode changeToolKey = KeyCode.Tab;
    [Tooltip("The interaction Key")]
    [SerializeField]
    private KeyCode InteractionKey = KeyCode.F;

    [Tooltip("The primary mouse button Key")]
    [SerializeField]
    private int primaryMouseKey = 0;
    [Tooltip("The secondary mouse button Key")]
    [SerializeField]
    private int secondaryMouseKey = 1;

    [Header("Events")]
    [Tooltip("Event fired when the user presses the primary mouse key")]
    [SerializeField]
    UnityEvent PrimaryMouseKeyEvent;
    [Tooltip("Event fired when the user presses the secondaru mouse key")]
    [SerializeField]
    UnityEvent SecondaryMouseKeyEvent;
    [Tooltip("Event fired when the user presses the interact key")]
    [SerializeField]
    UnityEvent OnInteractKeyEvent;
    [Tooltip("Event fired when the user presses the change weapon key")]
    [SerializeField]
    UnityEvent ChangeWeaponEvent;
    [Tooltip("Event fired when the user holds the interact key")]
    [SerializeField]
    UnityEvent OnInteractHoldComplete;
    [Tooltip("Event fired when the user primary mouse key")]
    [SerializeField]
    UnityEvent OnPrimaryHoldComplete;
    [Tooltip("Event fired when the user secondary mouse key")]
    [SerializeField]
    UnityEvent OnSecondaryHoldComplete;

    [Tooltip("Event fired with the how long the user has held the key")]
    [SerializeField]
    InputHeldEvent OnInteractHold;
    [Tooltip("Event fired with the how long the user has held the key")]
    [SerializeField]
    InputHeldEvent OnPrimaryHold;
    [Tooltip("Event fired with the how long the user has held the key")]
    [SerializeField]
    InputHeldEvent OnSecondaryHold;

    [Header("Other Settings")]
    [Tooltip("How long the user will have to hold an input key, for the holdcomplete events to trigger")]
    [SerializeField]
    private float interactionTime = 1f;


    private bool interactionKeyDown = false;
    private bool primaryButtonDown = false;
    private bool secondaryButtonDown = false;


    private float interactionKeyTimer = 0f;
    private float primaryButtonTimer = 0f;
    private float secondaryButtonTimer = 0f;

 


    private void Update()
    {
        if (Input.GetMouseButton(primaryMouseKey))
        {
            if (!primaryButtonDown)
            {
                PrimaryMouseKeyEvent?.Invoke();
                primaryButtonDown = true;
                StartCoroutine(PrimaryMouseHeldCoroutine());
            }
        }
        else primaryButtonDown = false;

        if (Input.GetMouseButton(secondaryMouseKey))
        {
            if (!secondaryButtonDown)
            {
                SecondaryMouseKeyEvent?.Invoke();
                secondaryButtonDown = true;
                StartCoroutine(SecondaryMouseHeldCoroutine());
            }
        }
        else secondaryButtonDown = false;
        if (Input.GetKey(InteractionKey))
        {
            if (!interactionKeyDown)
            {
                OnInteractKeyEvent?.Invoke();
                interactionKeyDown = true;
                StartCoroutine(InteractionHeldCoroutine());
            }
        }
        else interactionKeyDown = false;
    }

    IEnumerator PrimaryMouseHeldCoroutine()
    {
        while(primaryButtonDown)
        {
            primaryButtonTimer += Time.deltaTime;

            OnPrimaryHold?.Invoke(secondaryButtonTimer);
            if (primaryButtonTimer >= interactionTime)
            {
                OnPrimaryHoldComplete?.Invoke();
                primaryButtonTimer = 0f;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        primaryButtonTimer = 0f;
        yield return null;

    }
    IEnumerator SecondaryMouseHeldCoroutine()
    {
        while (secondaryButtonDown)
        {
            secondaryButtonTimer += Time.deltaTime;

            OnSecondaryHold?.Invoke(primaryButtonTimer);
            if (secondaryButtonTimer >= interactionTime)
            {
                OnSecondaryHoldComplete?.Invoke();
                secondaryButtonTimer = 0f;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        secondaryButtonTimer = 0f;
        yield return null;
    }
    IEnumerator InteractionHeldCoroutine()
    {
        while (interactionKeyDown)
        {
            interactionKeyTimer += Time.deltaTime;

            OnInteractHold?.Invoke(interactionKeyTimer);
            if (interactionKeyTimer >= interactionTime)
            {
                OnInteractHoldComplete?.Invoke();
                interactionKeyTimer = 0f;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        interactionKeyTimer = 0f;
        yield return null;
    }

}
