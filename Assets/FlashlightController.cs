﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlashlightController : MonoBehaviour
{
    Light flashlight = null;
    [SerializeField]
    private KeyCode lightKey = KeyCode.T;

    [SerializeField]
    bool flicker = true;

    private float defaultIntensity = 0f;
    [SerializeField]
    float flickerMinTime = 60f;
    [SerializeField]
    float flickerMaxTime = 300f;
    [SerializeField]
    int maxFlicker = 50;
    [SerializeField]
    int minFlicker = 15;


    private void Start()
    {
        flashlight = this.GetComponent<Light>();
        defaultIntensity = flashlight.intensity;
        StartCoroutine(LightFlickerTimer(flickerMinTime, flickerMaxTime));
    }
    // Update is called once per frame
    void LateUpdate()
    {
        if (Input.GetKeyDown(lightKey))
        {
            flashlight.enabled = !flashlight.enabled;
        }
    }
    IEnumerator LightFlickerTimer(float rangeMin, float rangeMax)
    {
        while(flicker)
        {
            float delay = Random.Range(rangeMin, rangeMax);
            Debug.Log(delay);
            yield return new WaitForSeconds(delay);
            StartCoroutine(LightFlicker(Random.Range(minFlicker,maxFlicker)));
        }
    }

    IEnumerator LightFlicker(int flickerAmount)
    {

        for(int i = 0; i < flickerAmount; i++)
        {
            flashlight.intensity = Random.Range(30, defaultIntensity);
            yield return new WaitForSeconds(Random.Range(0.01f, 0.1f));
        }

        flashlight.intensity = defaultIntensity;
    }
}
