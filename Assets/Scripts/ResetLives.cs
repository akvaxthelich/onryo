using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ResetLives : MonoBehaviour
{

    private void Start()
    {
        Application.targetFrameRate = 60;
        GameData.ResetLives();
        GameData.ResetHope();
    }
}

