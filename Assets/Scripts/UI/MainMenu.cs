using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour {

    public void Start()
    {
        Cursor.visible = true;
    }



    public void StartGame() {     
        SceneManager.LoadScene("Intro");
    }

    public void QuitGame() {

        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }
}
