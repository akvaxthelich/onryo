using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    float spriteLength; //to repeat sprite later
    float initPosition;
    public float parallaxMultiplier;

    SpriteRenderer sr;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        spriteLength =  sr.bounds.size.x;
        initPosition = transform.position.x;
        //print(spriteLength);
    }

    //only need to move a proportion of the distance the camera moved (determined by parallax mult)
    //if the distance this object has moved FROM THE INIT is greater than half its length (since we are looking at the center) - may have trouble on serial movement
    //increase this object's initial position over by the object's length
    private void LateUpdate()
    {
        float moveDist = Camera.main.transform.position.x * parallaxMultiplier;
        float inverse = Camera.main.transform.position.x * (1 - parallaxMultiplier);

        transform.position = new Vector3(initPosition + moveDist, transform.position.y, 0f);

        if (inverse > initPosition + (spriteLength / 2)) {
            initPosition += spriteLength;
        }
        else if (inverse < initPosition - (spriteLength / 2)) { 
            initPosition -= spriteLength;
        }
    }

}
