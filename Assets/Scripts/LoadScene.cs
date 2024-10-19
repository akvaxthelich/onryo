using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    float timer = 0;
    float maxTime = 1f; //seconds till we let pass purple
    public int index; //pass in next level
    Animator anim;
    private void Start()
    {
        if (SceneManager.GetActiveScene().buildIndex == 0) {
            anim = GetComponent<Animator>();
        }
        
    }

    private void Update()
    {
        timer += Time.deltaTime;   
        if (Input.anyKeyDown && SceneManager.GetActiveScene().buildIndex == 0)
        {
            LoadSceneNext(SceneManager.GetActiveScene().buildIndex + 1);
        }
        else if(Input.anyKeyDown && timer >= maxTime) {
            LoadSceneNext(index);
        }
    }
    public void LoadSceneNext(int buildIndex) {

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            anim = GetComponent<Animator>();
            anim.SetTrigger("Fade");
        }
        
        StartCoroutine(LoadLevel(buildIndex));

    }

    IEnumerator LoadLevel(int bi) { 
    
        yield return new WaitForSeconds(1);
        SceneManager.LoadScene(bi);
    
    }
}
