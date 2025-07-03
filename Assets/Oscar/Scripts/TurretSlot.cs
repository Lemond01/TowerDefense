using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

/// <summary>
/// Solo gestiona la lógica de arrastre UI → casilla 2D.
/// Toma sus datos desde TurretData.
/// </summary>
public class TurretSlot : MonoBehaviour,
                          IBeginDragHandler,
                          IDragHandler,
                          IEndDragHandler
{
    [Header("Arrastre UI")]
    [Range(0.2f,1f)] public float iconAlpha = 0.8f;

    [Header("Mapa 2D")]
    public LayerMask tileMask2D;

    // Internals
    private TurretData  turretData;
    private Canvas      canvas;
    private RectTransform canvasRect;
    private GameObject  dragIcon;

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
        if (turretData == null) return;

        // Creamos icono UI flotante
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
        if (turretData != null)
        {
            // Pantalla→mundo 2D
            Vector2 world2D = Camera.main.ScreenToWorldPoint(e.position);
            var hit = Physics2D.OverlapPoint(world2D, tileMask2D);
            if (hit)
            {
                var tile = hit.GetComponent<Tile>();
                if (tile != null && !tile.hasturet)
                {
                    // Pintamos icono en la casilla
                    var sr = hit.GetComponent<SpriteRenderer>();
                    if (sr != null)
                    {
                        sr.sprite  = turretData.Icon;
                        sr.enabled = true;
                    }
                    tile.hasturet = true;
                }
            }
        }

        if (dragIcon != null)
            Destroy(dragIcon);
    }

    private void UpdateIconPosition(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            e.position,
            e.pressEventCamera,
            out Vector2 lp
        );
        dragIcon.GetComponent<RectTransform>().localPosition = lp;
    }
}

