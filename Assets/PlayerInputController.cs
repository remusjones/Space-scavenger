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
    [SerializeField]
    private KeyCode reloadKey = KeyCode.R;
    [SerializeField]
    private KeyCode changeToolKey = KeyCode.Tab;
    [SerializeField]
    private KeyCode InteractionKey = KeyCode.F;

    [SerializeField]
    private int primaryMouseKey = 0;
    [SerializeField]
    private int secondaryMouseKey = 1;

    [Header("Events")]
    [SerializeField]
    UnityEvent PrimaryMouseKeyEvent;
    [SerializeField]
    UnityEvent SecondaryMouseKeyEvent;
    [SerializeField]
    UnityEvent OnInteractKeyEvent;
    [SerializeField]
    UnityEvent ChangeWeaponEvent;
    [SerializeField]
    UnityEvent OnInteractHoldComplete;
    [SerializeField]
    UnityEvent OnPrimaryHoldComplete;
    [SerializeField]
    UnityEvent OnSecondaryHoldComplete;

    [SerializeField]
    InputHeldEvent OnInteractHold;
    [SerializeField]
    InputHeldEvent OnPrimaryHold;
    [SerializeField]
    InputHeldEvent OnSecondaryHold;

    [Header("Other Settings")]
    [Tooltip("Assign this to the default weapon, this will be modified at runtime.")]
    [SerializeField]
    BTool selectedTool = null;

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
            }
        }
        else primaryButtonDown = false;

        if (Input.GetMouseButton(secondaryMouseKey))
        {
            if (!secondaryButtonDown)
            {
                SecondaryMouseKeyEvent?.Invoke();
                secondaryButtonDown = true;
            }
        }
        else secondaryButtonDown = false;
        if (Input.GetKey(InteractionKey))
        {
            if (!interactionKeyDown)
            {
                OnInteractKeyEvent?.Invoke();
                interactionKeyDown = true;
                Debug.Log("I have pressed the key once.. ");
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
            Debug.Log("Interaction held: " + interactionKeyTimer);
            if (interactionKeyTimer >= interactionTime)
            {
                Debug.Log("Interaction finsished: " + interactionKeyTimer);
                OnInteractHoldComplete?.Invoke();
                interactionKeyTimer = 0f;
                break;
            }
            yield return new WaitForEndOfFrame();
        }
        Debug.Log("Interaction Early Exit: " + interactionKeyTimer);
        interactionKeyTimer = 0f;
        yield return null;
    }

}
