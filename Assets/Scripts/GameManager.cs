using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

    public bool newGameStart;

    public bool cursorLockedVar;
    public bool isPaused;

    [Header("Pause Game Manager")]
    public AudioClip pauseAmbientMusic;

    [Header("Pause Menu UI")]
    public GameObject PauseMenu;

    // Start is called before the first frame update
    void Awake()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLockedVar = true;

        isPaused = false;
    }

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        cursorLockedVar = true;

        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void PauseToggle()
    {
        cursorLockedVar = !cursorLockedVar;

        if (!isPaused)
        {
            PauseGame();
        }
        else
        {
            ResumeGame();
        }
    }

    public void PauseGame()
    {
        ShowCursor();

        Time.timeScale = 0f;

        AudioListener.pause = true;

        isPaused = true;
    }

    public void ResumeGame()
    {
        HideCursor();

        Time.timeScale = 1f;

        AudioListener.pause = false;

        isPaused = false;
    }

    public void HideCursor()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void ShowCursor()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void EnablePauseMenu()
    {
        //PauseMenu.SetActive(true);
    }

    public void DisablePauseMenu()
    {
        //PauseMenu.SetActive(false);
    }


    #region Pause Menu Functions
    public void ResumeGameViaMenu()
    {
        PauseToggle();
        DisablePauseMenu();
    }

    public void SaveGame()
    {
        //PlayerPrefs.SetFloat("xval", pController.gameObject.transform.position.x);
        //PlayerPrefs.SetFloat("yval", pController.gameObject.transform.position.y);
        //PlayerPrefs.SetFloat("zval", pController.gameObject.transform.position.z);
        //pController.SavePlayerData();
        //dbManager.SaveDatabase();
        //inventoryManager.SaveInventorySlots();
        //StartCoroutine(playerUI.DisplayLog("Game Saved"));
    }

    public void ExitToMainMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }

    public void ExitToDesktop()
    {
        Application.Quit();
    }
    #endregion
}
