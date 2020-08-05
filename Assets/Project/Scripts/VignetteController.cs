using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;
public class VignetteController : MonoBehaviour
{

    //[SerializeField]
    Vignette vignette;
    [SerializeField]
    GameObject volumeObject = null;

    Volume volume;


    public void Start()
    {
        volume = volumeObject.GetComponent<Volume>();
        Vignette tmp;
        if (volume.sharedProfile.TryGet<Vignette>(out tmp))
        {
            vignette = tmp;
        }
    }
    public void HandleTurnAcceleration(Vector2 accel)
    {
        if (vignette == null)
            return;
        float x = accel.x;
        float y = accel.y;

        float xOffset = 0f;
        float yOffset = 0f;

       
        // resting 0.5f
        if (x < 0f)
        {
            xOffset = Mathf.Lerp(0.5f, 0f, x / -2f);
        }
        else if (x > 0f)
        {

            xOffset = Mathf.Lerp(0.5f, 1f, x / 2f);
        }
        if (y < 0f)
        {
            yOffset = Mathf.Lerp(0.5f, 0f, y / -2f);
        }
        else if (y > 0f)
        {
            yOffset = Mathf.Lerp(0.5f, 1f, y / 2f);
        }

        if (x == 0f)
            xOffset = 0.5f;
        if (y == 0f)
            yOffset = 0.5f;

        vignette.center.SetValue(new UnityEngine.Rendering.Vector2Parameter(new Vector2(xOffset, yOffset),true));
    }

}
