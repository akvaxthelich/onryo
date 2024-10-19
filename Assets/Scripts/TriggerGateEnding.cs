using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerGateEnding : MonoBehaviour
{
    GameObject gate;
    private void Start()
    {
        gate = GameObject.Find("gate");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        gate.GetComponent<Animator>().SetTrigger("Open");
        gate.GetComponent<AudioSource>().Play();
        gate.GetComponent<BoxCollider2D>().enabled = true;

        Destroy(gameObject);

    }
}
