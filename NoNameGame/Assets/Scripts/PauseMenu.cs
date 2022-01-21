using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour


//Created by Lukas Reeves//

{

    Scene currentScene;
    GameObject[] pauseMenu;
    GameObject[] settingsMenu;
    GameObject[] playerUI;

    // Start is called before the first frame update
    void Start()
    {
        Time.timeScale = 1;
        pauseMenu = GameObject.FindGameObjectsWithTag("ShowOnPause");
        settingsMenu = GameObject.FindGameObjectsWithTag("ShowOnSettings");
        playerUI = GameObject.FindGameObjectsWithTag("PlayerUI");
        HidePaused();
        HideSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            currentScene = SceneManager.GetActiveScene();

            if (Time.timeScale == 1)
            {
                Time.timeScale = 0;
                ShowPaused();
                Debug.Log("Showing menu");
                Debug.Log(pauseMenu[0]);
            }
            else if (Time.timeScale == 0)
            {
                Time.timeScale = 1;
                HidePaused();
                HideSettings();
            }
        }
    }

    // Reloads current scene
    public void Reload()
    {
        SceneManager.LoadScene(currentScene.name);
    }

    // Controls pause
    public void PauseControl()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = 0;
            ShowPaused();
        }
        else if (Time.timeScale == 0)
        {
            Time.timeScale = 1;
            HidePaused();
        }
    }

    // Shows all objects with ShowOnPause tag
    public void ShowPaused()
    {
        foreach (GameObject g in pauseMenu)
        {
            g.SetActive(true);
        }
        foreach (GameObject g in playerUI)
        {
            g.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    // Hides all objects with ShowOnPause tag
    public void HidePaused()
    {
        foreach (GameObject g in pauseMenu)
        {
            g.SetActive(false);
        }
        foreach (GameObject g in playerUI)
        {
            g.SetActive(true);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Shows all objects with ShowOnSettings tag
    public void ShowSettings()
    {
        foreach (GameObject g in settingsMenu)
        {
            g.SetActive(true);
        }
        foreach (GameObject g in playerUI)
        {
            g.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Confined;
        Cursor.visible = true;
    }

    // Hides all objects with ShowOnSettings tag
    public void HideSettings()
    {
        foreach (GameObject g in settingsMenu)
        {
            g.SetActive(false);
        }
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Loads inputted scene
    public void LoadLevel(string scene)
    {
        SceneManager.LoadScene(scene);
    }

    public void CloseGame()
    {
        Application.Quit();
    }
}
