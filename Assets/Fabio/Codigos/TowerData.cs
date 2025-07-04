// TowerData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewTower", menuName = "TowerDefense/Tower")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public Sprite icon;
    public GameObject towerPrefab;
}



