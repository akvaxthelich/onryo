using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class LifeTracker : MonoBehaviour
{
    TextMeshProUGUI text;
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
        text.text = "LIVES: X";
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "LIVES: " + LivesToRomanNumeral(GameData.GetLives());
    }

    string LivesToRomanNumeral(int lives) {
        string lresult = "";

        if (lives <= 0) {
            return lresult;
        }
        else {
            for (int i = 0; i < lives; i++)
            {
                lresult += "I";
            }
            return lresult;

        }

    }
}
