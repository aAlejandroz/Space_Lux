using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameOver : MonoBehaviour {

    public GameObject player;
    public GameObject playerBase;
    public bool isPlaying;

	// Use this for initialization
	void Start () {
        isPlaying = true;
        player = GameObject.FindGameObjectWithTag("Player");
        playerBase = GameObject.FindGameObjectWithTag("Base");
    }

    // Update is called once per frame
    void Update () {
        if (isPlaying) {
            if (player == null || playerBase == null) {
                Debug.Log("GAME OVER");
                isPlaying = false;
            }
        }        
	}
}
