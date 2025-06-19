using System.Collections.Generic;
using UnityEngine;

public class TowerManagerInGame : MonoBehaviour
{
    public Transform towerHolder; // Donde se colocarán las torres o botones (opcional)
    public GameObject towerButtonPrefab; // Si deseas mostrar botones otra vez

    void Start()
    {
        List<TowerData> selected = TowerSelectionStorage.selectedTowers;

        foreach (TowerData tower in selected)
        {
            Debug.Log("Torre cargada: " + tower.towerName);
            
        }
    }
}

