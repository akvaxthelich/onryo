using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuit : MonoBehaviour
{

    private float timer = 0.0f;
    private bool isPressed = false;
    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Submit")){
            isPressed = true;
        }else if(Input.GetButtonUp("Submit")){
            isPressed = false;
        }
        if(isPressed){
            timer += Time.deltaTime;
            Debug.Log(timer);
        }else{
            timer = 0.0f;
        }
        if(timer >= 5.0f){
            Debug.Log("GAME CLOSED");
            Application.Quit();
        }
    }
}
