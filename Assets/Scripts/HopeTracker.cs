using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using Unity.VisualScripting;

public class HopeTracker : MonoBehaviour
{
    public GameObject gate; //attach gate here! spaghetti code at its absolute finest!
    public int hopeQuota;
    bool flagFinish = false;
    TextMeshProUGUI text;
    public TextMeshProUGUI text2;
    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<TextMeshProUGUI>();
    }
    void Update()
    {
        text.text = "HOPE: " + GameData.GetHope(); //please note that, andrew, that this code and the existence of this file is a fucking felony
        //you should be SHOT
        if (GameData.GetHope() >= hopeQuota && !flagFinish) {
            flagFinish = true;
            ActivateGate();
        }
    }

    private void ActivateGate()
    {
        gate.GetComponent<Animator>().SetTrigger("Open");
        gate.GetComponent<AudioSource>().Play();
        gate.GetComponent<BoxCollider2D>().enabled = true;
    }
}
