using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class GameManager : MonoBehaviour
{

    private static GameManager instance;
    public static GameManager Instance { 
        get
        {
            if (instance == null)
                instance = FindObjectOfType<GameManager>();
            return instance;
        } 
    }


    public bool newGameStart;

    public bool cursorLockedVar;
    public bool isPaused;

    [Header("Pause Game Manager")]
    public AudioClip pauseAmbientMusic;

    [Header("UI")]
    public CanvasGroup mainMenu;
    public CanvasGroup pauseMenu;
    public CanvasGroup creditsUI;
    public CanvasGroup hud;

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
        DOTween.Init();
        LoadMainMenu();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }
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
        ShowPauseMenu(true);
        isPaused = true;
    }

    public void ResumeGame()
    {
        HideCursor();

        Time.timeScale = 1f;

        AudioListener.pause = false;
        ShowPauseMenu(false);
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

    public void ShowPauseMenu(bool show)
    {
        pauseMenu.gameObject.SetActive(show);
        hud.gameObject.SetActive(!show);
    }


    #region Menu Functions

    public void LoadMainMenu()
    {

        mainMenu.gameObject.SetActive(true);
        mainMenu.alpha = 1;
        
        pauseMenu.alpha = 1;
        pauseMenu.gameObject.SetActive(false);

        creditsUI.alpha = 0;
        creditsUI.gameObject.SetActive(false);

        hud.alpha = 1;
        hud.gameObject.SetActive(false);

        Time.timeScale = 0f;
        ShowCursor();
        isPaused = true;
    }

    public void StartGame()
    {
        Time.timeScale = 1f;

        isPaused = false;
        mainMenu.alpha = 0;
        mainMenu.gameObject.SetActive(false);
        hud.gameObject.SetActive(true);


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

    public void ExitToDesktop()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }

    public void RollCredits()
    {
        Time.timeScale = 1f;
        mainMenu.alpha = 0;
        mainMenu.gameObject.SetActive(false);

        creditsUI.alpha = 1;
        creditsUI.gameObject.SetActive(true);
        creditsUI.DOFade(1, 0.5f);
    }
    public void CloseCredits()
    {
        creditsUI.alpha = 0;
        creditsUI.gameObject.SetActive(false);

        mainMenu.alpha = 0;
        mainMenu.gameObject.SetActive(true);
        Sequence sequence = DOTween.Sequence()
            .Append(mainMenu.DOFade(1, 0.5f))
            .AppendCallback(() => { Time.timeScale = 0f; });
    }
#endregion
}
