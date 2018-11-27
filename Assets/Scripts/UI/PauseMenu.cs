using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour {

    public static bool GameisPaused = false;
    public GameObject pauseMenuUI;
    public GameObject crossHair;
    public GameObject HUD;

	// Update is called once per frame
	void Update () {
        if (Input.GetKeyDown(KeyCode.Escape))
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

    public void Resume()
    {
        HUD.SetActive(true);
        Cursor.visible = false;
        crossHair.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameisPaused = false;
    }


    void Pause()
    {
        HUD.SetActive(false);
        crossHair.SetActive(false);
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameisPaused = true;
    }


    public void LoadMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Load Menu");
        SceneManager.LoadScene("Menu");
    }

    public void QuitGame()
    {
        Debug.Log("Quiting ");
        Application.Quit();
    }
}
