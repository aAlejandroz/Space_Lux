using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimap : MonoBehaviour {

     public static bool GameisPaused = false;
    public GameObject miniMapUI;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            if (GameisPaused)
            {
                Resume();

            }
            else
            {
                Pause();

            }
        }

    }

    void Resume()
    {
        miniMapUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;

    }

    void Pause()
    {
        miniMapUI.SetActive(true);
        Time.timeScale = 0.3f;
        GameisPaused = true;

    }
}
