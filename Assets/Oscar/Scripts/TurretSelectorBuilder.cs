using System.Collections.Generic;
using UnityEngine;


public class TurretSelectorBuilder : MonoBehaviour
{
    [Header("Slot Prefab & Parent")]
    public GameObject      slotPrefab;    // Prefab con TurretData + TurretSlot + UI hijos
    public Transform       slotsParent;   // Panel con LayoutGroup donde se añadirán los slots

    [Header("Torres")]
    public List<TowerData> availableTowers;

    void Start()
    {
        if (slotPrefab == null || slotsParent == null)
        {
            Debug.LogError("TurretSelectorBuilder: revisa slotPrefab y slotsParent en el Inspector.");
            return;
        }

        // VERSION Automatica
        List<TowerData> selected = TowerSelectionStorage.selectedTowers;
        if (selected != null && selected.Count > 0)
        {
            BuildSlots(selected);
        }
        else
        {
            // VERSION Manual: usa la lista publica availableTowers
            if (availableTowers == null)
            {
                Debug.LogError("TurretSelectorBuilder: availableTowers no asignado y no hay torres seleccionadas.");
                return;
            }
            BuildSlots(availableTowers);
        }
    }

    
    void BuildSlots(List<TowerData> towers)
    {
        // Limpia slots antiguos
        for (int i = slotsParent.childCount - 1; i >= 0; i--)
            Destroy(slotsParent.GetChild(i).gameObject);

        // Crea nuevos slots
        foreach (var td in towers)
        {
            if (td == null) continue;

            var go = Instantiate(slotPrefab, slotsParent);
            var tdComp = go.GetComponent<TurretData>();
            if (tdComp == null)
            {
                Debug.LogError("TurretSelectorBuilder: SlotPrefab no tiene componente TurretData.");
                continue;
            }

            
            tdComp.Configure(td);
            
        }
    }
}
