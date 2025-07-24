using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenu : MonoBehaviour
{
    [Header("UI")]
    public GameObject pauseUI;
    public GameObject UIoptions;


    [Header("Opciones")]
    public string mainMenuSceneName = "MainMenu";

    private bool isPaused = false;
    private AudioSource[] allAudioSources;

    void Start()
    {
        if (pauseUI != null)
            pauseUI.SetActive(false);
        if (UIoptions != null) UIoptions.SetActive(false);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
                ResumeGame();
            else
                PauseGame();
        }
    }

    public void PauseGame()
    {
        Debug.Log("[PauseMenu] Pausando juego");
        Time.timeScale = 0f;
        isPaused = true;

        if (pauseUI != null)
            pauseUI.SetActive(true);

        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;

        allAudioSources = FindObjectsByType<AudioSource>(FindObjectsSortMode.None);
        foreach (AudioSource audio in allAudioSources)
        {
            audio.Pause();
        }
    }

    public void ResumeGame(bool keepCursorVisible = false)
    {
        Debug.Log("[PauseMenu] Reanudando juego");
        Time.timeScale = 1f;
        isPaused = false;

        if (pauseUI != null)
            pauseUI.SetActive(false);

        if (keepCursorVisible)
        {
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }
        else
        {
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
        }

        if (allAudioSources != null)
        {
            foreach (AudioSource audio in allAudioSources)
            {
                audio.UnPause();
            }
        }
    }


    public void LoadMainMenu()
    {
        Debug.Log("[PauseMenu] Cargando menú principal");
        Time.timeScale = 1f;
        SceneManager.LoadScene(mainMenuSceneName);
    }
}



