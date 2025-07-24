// TowerData.cs
using UnityEngine;

[CreateAssetMenu(fileName = "NewTower", menuName = "TowerDefense/Tower")]
public class TowerData : ScriptableObject
{
    public string towerName;
    public int towerCost;
    public Sprite icon;
    public GameObject towerPrefab;
    
    public Material  placementMaterial;
    
    [Header("Offset Local de Colocación")]
    public Vector3 spawnLocalPosition;    // pequeño ajuste en XYZ
    public Vector3 spawnLocalEuler;       // rotación específica para esta torre
    public Vector3 spawnLocalScale = Vector3.one;  // escala local deseada
}



