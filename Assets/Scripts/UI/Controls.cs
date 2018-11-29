using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {

    public GameObject GeneralHUD;
    public GameObject CrossHair;
    public GameObject ControlHUD;
    
	// Use this for initialization
	void Start () {
        Cursor.visible = true;
        GeneralHUD.SetActive(false);
        CrossHair.SetActive(false);
        ControlHUD.SetActive(true);
        Time.timeScale = 0f;
    }
	
	// Update is called once per frame
	public void PlayGame()
    {
        Cursor.visible = false;
        GeneralHUD.SetActive(true);
        CrossHair.SetActive(true);
        ControlHUD.SetActive(false);
        Time.timeScale = 1f;

    }
}
