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
}