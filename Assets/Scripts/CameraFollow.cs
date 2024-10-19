using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public PlayerMovement player;
    public float boundsX = 200; //irrelevant rn gurrjjgjj
    // Start is called before the first frame update
    void Awake()
    {
        player = GameObject.Find("GKnight").GetComponent<PlayerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Mathf.Abs(player.gameObject.transform.position.x) <= boundsX) {
            transform.position = new Vector3(player.transform.position.x, 4f, -10);
        }
        
    }
}
