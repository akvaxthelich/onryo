using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public static byte upgrades; //don't do assembly, kids
    
    private void Awake() {
        if (instance != null) {

            Destroy(gameObject);
        }
        else { 

            instance = this;
        }
    }

    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
