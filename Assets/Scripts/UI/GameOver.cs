using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOver : MonoBehaviour
{
    void Start()
    {
        Cursor.visible = true;

    }

    
    public void QuitGame()
    {

        Application.Quit();
    }

    public void Menu()
    {
        SceneManager.LoadScene("Menu");
    }




}