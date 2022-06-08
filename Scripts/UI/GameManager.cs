using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    const string mapName_LostCity = "LostCity";
    const string mapName_Cemetery = "Cemetery";

    private void Awake()
    {
        Application.targetFrameRate = 60;
    }
    public void StartGame(string SceneName)
    {
        SceneManager.LoadSceneAsync(SceneName);
    }

    public void GameChoice(Dropdown dropdown)
    {
        if (dropdown.options[dropdown.value].text == mapName_LostCity) {
            StartGame("map1");
        }
        //else if (dropdown.options[dropdown.value].text == mapName_Cemetery)
        //{
        //    StartGame("map2");
        //}
    }

    public void UnLockMouse()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Restart()
    {
        StartGame(SceneManager.GetActiveScene().name);
    }

    public void GoMenu()
    {
        StartGame("Menu");
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
