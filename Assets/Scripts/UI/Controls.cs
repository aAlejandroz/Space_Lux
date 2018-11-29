using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour {


    public GameObject CrossHair;
    public GameObject ControlHUD;
    
    

	// Use this for initialization
	void Start () {
        
        Cursor.visible = true;
        CrossHair.SetActive(false);
        ControlHUD.SetActive(true);
        Time.timeScale = 0f;
    }
	
	// Update is called once per frame
	public void PlayGame()
    {
        
        CrossHair.SetActive(true);
        Cursor.visible = false;
        ControlHUD.SetActive(false);
        Time.timeScale = 1f;

    }
}
