using UnityEngine;
using UnityEngine.UI;

public class TurretData : MonoBehaviour
{
    [Header("UI References (drag in Inspector)")]
    public Image iconImage;    // Image en tu SlotPrefab

    // Internals
    private TowerData data;
    
    public void Configure(TowerData td)
    {
        data = td;
        if (iconImage) iconImage.sprite = td.icon;
        if (iconImage) iconImage.SetNativeSize();
    }

    // Exposición para TurretSlot
    public Sprite    Icon   => data != null ? data.icon: null;
    public int       Cost   => data != null ? data.towerCost: 0;
    public GameObject Prefab => data != null ? data.towerPrefab: null;
    
    public Material PlacementMaterial => data != null ? data.placementMaterial : null;
    
    public Vector3   SpawnLocalPosition => data != null ? data.spawnLocalPosition : Vector3.zero;
    public Vector3   SpawnLocalEuler    => data != null ? data.spawnLocalEuler    : Vector3.zero;
    public Vector3   SpawnLocalScale    => data != null ? data.spawnLocalScale    : Vector3.one;
    
}