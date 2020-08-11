using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
public class FlashlightController : MonoBehaviour
{
    Light flashlight = null;


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


    public void OnInputDown()
    {
        flashlight.enabled = !flashlight.enabled;
    }
    private void Start()
    {
        flashlight = this.GetComponent<Light>();

        defaultIntensity = flashlight.intensity;
        if (flicker)
            StartCoroutine(LightFlickerTimer(flickerMinTime, flickerMaxTime));
        flashlight.enabled = false;
    }
    IEnumerator LightFlickerTimer(float rangeMin, float rangeMax)
    {
        while(flicker)
        {
            float delay = Random.Range(rangeMin, rangeMax);
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
