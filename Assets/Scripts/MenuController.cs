using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public void goMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void goEndScreen()
    {
        SceneManager.LoadScene("EndScreen");
    }

    public void goOptions()
    {
        SceneManager.LoadScene("OptionsScene");
    }

    public void goStart()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void goInstructions()
    {
        SceneManager.LoadScene("InstructionsScene");
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
