using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Build de slots a partir de una lista de TowerData.
/// </summary>
public class TurretSelectorBuilder : MonoBehaviour
{
    [Header("Slot Prefab & Parent")]
    public GameObject      slotPrefab;      // Prefab que contiene TurretData + TurretSlot + UI hijos
    public Transform       slotsParent;     // Panel con LayoutGroup

    [Header("Torres Seleccionadas")]
    public List<TowerData> availableTowers;

    void Start()
    {
        if (slotPrefab == null || slotsParent == null || availableTowers == null)
        {
            Debug.LogError("TurretSelectorBuilder: revisa asignaciones en el Inspector.");
            return;
        }
        BuildSlots();
    }

    void BuildSlots()
    {
        // Limpiar viejos
        for (int i = slotsParent.childCount - 1; i >= 0; i--)
            Destroy(slotsParent.GetChild(i).gameObject);

        // Instanciar nuevos
        foreach (var td in availableTowers)
        {
            if (td == null) continue;
            var go = Instantiate(slotPrefab, slotsParent);
            var tdComp = go.GetComponent<TurretData>();
            if (tdComp == null)
            {
                Debug.LogError("SlotPrefab no tiene TurretData.");
                continue;
            }
            tdComp.Configure(td);
            // TurretSlot sigue listo para arrastrar usando tdComp.Icon
        }
    }
}