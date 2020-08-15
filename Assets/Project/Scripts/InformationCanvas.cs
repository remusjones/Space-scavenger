using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InformationCanvas : MonoBehaviour
{
    [SerializeField]
    TMP_Text oxygenValue = null;
    [SerializeField]
    TMP_Text speedValue = null;
    private string speedAppend = "m/s";

    [SerializeField]
    Rigidbody rb = null;

    bool isRunning = false;
    

    private void Start()
    {
        isRunning = true;
        StartCoroutine(UpdateInformationUI());
    }
    IEnumerator UpdateInformationUI()
    {
        while(isRunning)
        {
            yield return new WaitForEndOfFrame();

            float playerSpeed = rb.velocity.magnitude;

            string printString;
            if (playerSpeed <= 99.9f)
            {
                printString = playerSpeed.ToString("0.0")+speedAppend;
            }else
            {
                printString = playerSpeed.ToString("0") + speedAppend;
            }

            speedValue.text = printString;
        }
    }
}
