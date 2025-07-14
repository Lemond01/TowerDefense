using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretSlot : MonoBehaviour,
                          IBeginDragHandler,
                          IDragHandler,
                          IEndDragHandler
{
    [Header("Arrastre UI")]
    [Range(0.2f, 1f)] public float iconAlpha = 0.8f;

    [Header("Map 3D")]
    public LayerMask Tile;

    // Internals
    private TurretData   turretData;
    private Canvas       canvas;
    private RectTransform canvasRect;
    private GameObject   dragIcon;

    void Awake()
    {
        canvas     = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        turretData = GetComponent<TurretData>();
        if (turretData == null)
            Debug.LogError("TurretSlot: falta TurretData en el SlotPrefab.");
    }

    public void OnBeginDrag(PointerEventData e)
    {
        if (turretData == null || turretData.Prefab == null)
        {
            Debug.LogError("TurretData o Prefab no asignado.");
            return;
        }

        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);
        var img = dragIcon.AddComponent<Image>();
        img.sprite        = turretData.Icon;
        img.raycastTarget = false;
        img.SetNativeSize();
        var c = img.color; c.a = iconAlpha; img.color = c;
        UpdateIconPosition(e);
    }

    public void OnDrag(PointerEventData e)
    {
        if (dragIcon != null)
            UpdateIconPosition(e);
    }

    public void OnEndDrag(PointerEventData e)
    {
        // Destruimos el icono de arrastre
        if (dragIcon != null)
            Destroy(dragIcon);

        if (turretData == null) return;
        if (Camera.main == null)
        {
            Debug.LogError("No se encontró MainCamera.");
            return;
        }

        // Raycast 3D
        Ray ray = Camera.main.ScreenPointToRay(e.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, Tile))
        {
            var tile = hit.collider.GetComponent<Tile>();
            if (tile == null)
            {
                Debug.LogError($"'{hit.collider.name}' está en capa Tile pero no tiene Tile.cs.");
                return;
            }

            if (tile.hasturet)
            {
                Debug.LogWarning($"La casilla '{tile.name}' ya tiene torreta.");
                return;
            }

            // Aquí gastamos: si no hay dinero, cancelamos
            int cost = turretData.Cost;
            if (!MoneyManager.Instance.Spend(cost))
            {
                Debug.LogWarning($"Fondos insuficientes (coste = {cost}).");
                return;
            }

            // Marcamos la casilla y colocamos la torreta
            tile.hasturet = true;

            var turretGO = Instantiate(
                turretData.Prefab,
                hit.collider.transform   // lo hacemos hijo
            );
            turretGO.transform.localPosition = Vector3.zero;
            turretGO.transform.localRotation = Quaternion.identity;
            turretGO.transform.localScale    = Vector3.one;

            Debug.Log($"Torreta '{turretGO.name}' colocada. Gastaste {cost}.");
        }
        else
        {
            Debug.LogWarning("Raycast Miss: no golpeó ninguna casilla.");
        }
    }

    private void UpdateIconPosition(PointerEventData e)
    {
        if (dragIcon == null) return;
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            e.position,
            e.pressEventCamera,
            out Vector2 lp
        );
        dragIcon.GetComponent<RectTransform>().localPosition = lp;
    }
}
