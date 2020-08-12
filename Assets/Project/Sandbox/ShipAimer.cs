using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShipAimer : MonoBehaviour
{
    RectTransform _Transform;
    // Start is called before the first frame update
    void Start()
    {
        _Transform = GetComponent<RectTransform>();
    }

    // Update is called once per frame
    void Update()
    {
        float posX = 0,posY = 0;
        posX += Input.GetAxis("Mouse X") * 30 * Time.deltaTime;
        posY += Input.GetAxis("Mouse Y") * 30 * Time.deltaTime;

        posX = Mathf.Clamp(posX, -100, 100);
        posY = Mathf.Clamp(posY, -100, 100);
        transform.position = new Vector2(posX , posY);
    }
}
