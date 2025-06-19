using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TowerSelectionManager : MonoBehaviour
{
    public Transform availableContainer;
    public Transform selectedContainer;
    public GameObject towerButtonPrefab;
    public int maxSelectedTowers = 6;
    public Button startButton;

    public List<TowerData> allTowers;
    private List<TowerData> selectedTowers = new List<TowerData>();

    void Start()
    {
        LoadAvailableTowers();
        startButton.onClick.AddListener(StartLevel);
        startButton.interactable = false;
    }

    void LoadAvailableTowers()
    {
        foreach (TowerData tower in allTowers)
        {
            TowerData currentTower = tower; // <- Esto soluciona el bug
            GameObject btn = Instantiate(towerButtonPrefab, availableContainer);
            btn.GetComponent<Image>().sprite = currentTower.icon;

            btn.GetComponent<Button>().onClick.AddListener(() => SelectTower(currentTower));
        }

    }

    void SelectTower(TowerData tower)
    {
        if (selectedTowers.Contains(tower) || selectedTowers.Count >= maxSelectedTowers)
            return;

        selectedTowers.Add(tower);

        TowerData currentTower = tower;  // VARIABLE TEMPORAL para el lambda
        GameObject btn = Instantiate(towerButtonPrefab, selectedContainer);
        btn.GetComponent<Image>().sprite = currentTower.icon;

        btn.GetComponent<Button>().onClick.AddListener(() => {
            selectedTowers.Remove(currentTower);
            Destroy(btn);
        });

        startButton.interactable = selectedTowers.Count > 0;
    }


    void StartLevel()
    {
        TowerSelectionStorage.selectedTowers = new List<TowerData>(selectedTowers);
        UnityEngine.SceneManagement.SceneManager.LoadScene("Level1");
    }
}


