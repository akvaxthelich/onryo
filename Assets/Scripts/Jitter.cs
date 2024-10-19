using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Jitter : MonoBehaviour
{

    private void Start()
    {
        StartCoroutine(Back());
    }
    void Update()
    {
        transform.localPosition = new Vector2(0 + Random.Range(0,.15f),0);
    }

    IEnumerator Back() { 
    
        yield return new WaitForSeconds(1.3f);
        SceneManager.LoadScene(0); //load menu.

    }
}
