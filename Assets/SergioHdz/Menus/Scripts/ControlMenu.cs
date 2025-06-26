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
        Debug.Log("[ControlMenu] Start - Mostrando MainMenu");
        ShowMainMenu();
    }

    public void OnClickReturn()
    {
        Debug.Log("[ControlMenu] Click en Volver");
        ShowMainMenu();
    }

    public void OnClickOptions()
    {
        Debug.Log("[ControlMenu] Click en Opciones");
        HideAllMenus();
        if (OptionsMenu != null)
        {
            OptionsMenu.SetActive(true);
            Debug.Log("[ControlMenu] OptionsMenu activado");
        }
        ShowAudioSettings();
    }

    public void OnClickHowToPlay()
    {
        Debug.Log("[ControlMenu] Click en Cómo jugar");
        HideAllMenus();
        if (HowToPlayMenu != null)
        {
            HowToPlayMenu.SetActive(true);
            Debug.Log("[ControlMenu] HowToPlayMenu activado");
        }
    }

    public void ShowAudioSettings()
    {
        Debug.Log("[ControlMenu] Mostrando ajustes de Audio");
        if (AudioPanel != null) AudioPanel.SetActive(true);
        if (DisplayPanel != null) DisplayPanel.SetActive(false);
    }

    public void ShowDisplaySettings()
    {
        Debug.Log("[ControlMenu] Mostrando ajustes de Pantalla");
        if (AudioPanel != null) AudioPanel.SetActive(false);
        if (DisplayPanel != null) DisplayPanel.SetActive(true);
    }

    private void ShowMainMenu()
    {
        Debug.Log("[ControlMenu] Mostrando MainMenu");
        HideAllMenus();
        if (MainMenu != null)
        {
            MainMenu.SetActive(true);
            Debug.Log("[ControlMenu] MainMenu activado");
        }
    }

    private void HideAllMenus()
    {
        if (MainMenu != null)
        {
            MainMenu.SetActive(false);
            Debug.Log("[ControlMenu] MainMenu desactivado");
        }
        if (OptionsMenu != null)
        {
            OptionsMenu.SetActive(false);
            Debug.Log("[ControlMenu] OptionsMenu desactivado");
        }
        if (HowToPlayMenu != null)
        {
            HowToPlayMenu.SetActive(false);
            Debug.Log("[ControlMenu] HowToPlayMenu desactivado");
        }
    }
}
