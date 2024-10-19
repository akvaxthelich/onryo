using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Something : MonoBehaviour
{

    private void Update()
    {
        if(Input.anyKeyDown){
            SceneManager.LoadScene(0);
        }
    }

}
