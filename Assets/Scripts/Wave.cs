using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wave : MonoBehaviour
{
    float origPosY;

    public float ampMod = .25f;
    public float freqMod = .25f;


    void FixedUpdate()
    {
        origPosY = transform.position.y;
        transform.position = new Vector2(transform.position.x, ampMod * Mathf.Sin(Time.time * freqMod) - origPosY);

    }
}
