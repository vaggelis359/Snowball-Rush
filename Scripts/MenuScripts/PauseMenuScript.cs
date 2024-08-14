using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class PauseMenuScript : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject optionsMenuUI;
    public GameObject EndGameMenuUI;
    public AudioMixer audioMixer;

    private bool inputDisabled = false;

    public GameObject skiiMan;  

    void Start()
    {   
        pauseMenuUI.SetActive(false);
        optionsMenuUI.SetActive(false);
        EndGameMenuUI.SetActive(false);
    }

    void Update()
    {
        if (inputDisabled)
            return;

        if (Input.GetKeyUp(KeyCode.Escape))     // we call OptionMenuUI whith Esc button  
        {
            if (optionsMenuUI.activeSelf)
            {
                CloseOptionsMenu();
            }
            else if (GameIsPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
        if (skiiMan == null)                    // we call EndGameMenuUI when we lose
        {
            EndGameMenuUI.SetActive(true);  
        }
    }

    public void Resume() 
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
    }

    public void OpenOptionsMenu()
    {
        optionsMenuUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        inputDisabled = true;
    }

    public void CloseOptionsMenu()
    {
        optionsMenuUI.SetActive(false);
        pauseMenuUI.SetActive(true);
        inputDisabled = false;
    }
    public void SetVolume(float volume)
    {
        audioMixer.SetFloat("volume", volume);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting game");
        Application.Quit();
    }
}
