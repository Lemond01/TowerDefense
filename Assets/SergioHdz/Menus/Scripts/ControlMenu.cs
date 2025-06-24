using UnityEngine;

public class ControlMenu : MonoBehaviour
{
    [Header("Menús")]
    public GameObject MainMenu;
    public GameObject OptionsMenu;
    public GameObject HowToPlayMenu;

    [Header("Subpaneles")]
    public GameObject AudioPanel;
    public GameObject DisplayPanel;

    private void Start()
    {
        ShowMainMenu();
    }

    public void OnClickReturn()
    {
        ShowMainMenu();
    }

    public void OnClickOptions()
    {
        HideAllMenus();
        if (OptionsMenu != null) OptionsMenu.SetActive(true);
        ShowAudioSettings();
    }

    public void OnClickHowToPlay()
    {
        HideAllMenus();
        if (HowToPlayMenu != null) HowToPlayMenu.SetActive(true);
    }

    public void ShowAudioSettings()
    {
        if (AudioPanel != null) AudioPanel.SetActive(true);
        if (DisplayPanel != null) DisplayPanel.SetActive(false);
    }

    public void ShowDisplaySettings()
    {
        if (AudioPanel != null) AudioPanel.SetActive(false);
        if (DisplayPanel != null) DisplayPanel.SetActive(true);
    }

    private void ShowMainMenu()
    {
        HideAllMenus();
        if (MainMenu != null) MainMenu.SetActive(true);
    }

    private void HideAllMenus()
    {
        if (MainMenu != null) MainMenu.SetActive(false);
        if (OptionsMenu != null) OptionsMenu.SetActive(false);
        if (HowToPlayMenu != null) HowToPlayMenu.SetActive(false);
    }
}
