using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class TurretSlot : MonoBehaviour,
                          IBeginDragHandler,
                          IDragHandler,
                          IEndDragHandler
{
    [Header("Arrastre UI")]
    [Range(0.2f,1f)] public float iconAlpha = 0.8f;
    [Range(0.1f,1f)] public float iconScale = 0.8f;

    [Header("Tiles 3D")]
    public LayerMask tileLayer;

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
        if (turretData?.Prefab == null) return;

        // crea icono UI
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas.transform,false);
        var img = dragIcon.AddComponent<Image>();
        img.sprite        = turretData.Icon;
        img.raycastTarget = false;

        // tamaño y transparencia
        var slotRT = turretData.iconImage.rectTransform;
        var drt    = dragIcon.GetComponent<RectTransform>();
        drt.pivot     = slotRT.pivot;
        drt.sizeDelta = slotRT.sizeDelta * iconScale;
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
        // destruye icono
        if (dragIcon != null) Destroy(dragIcon);
        if (turretData?.Prefab == null || Camera.main == null) return;

        // raycast al collider hijo
        Ray ray = Camera.main.ScreenPointToRay(e.position);
        if (!Physics.Raycast(ray, out RaycastHit hit, 200f, tileLayer))
            return;

        // localiza Tile en padres (position3D)
        var tile = hit.collider.GetComponentInParent<Tile>();
        if (tile == null || tile.hasturet) return;

        // gastar dinero
        if (!MoneyManager.Instance.Spend(turretData.Cost))
        {
            Debug.Log("TurretSlot: fondos insuficientes.");
            return;
        }
        tile.hasturet = true;

        // highlight
        var mr = tile.GetComponent<MeshRenderer>();
        if (mr != null && turretData.PlacementMaterial != null)
            mr.material = turretData.PlacementMaterial;

        // spawn en centro del collider
        Vector3 spawnWorldPos = hit.collider.bounds.center;

        // instanciar como hijo de position3D sin conservar posición mundial
        var go = Instantiate(turretData.Prefab, tile.transform, false);

        // calcular posición local exacta
        Vector3 localPos = tile.transform.InverseTransformPoint(spawnWorldPos);
        go.transform.localPosition = localPos + turretData.SpawnLocalPosition;

        // aplicar rotación y escala específicas
        go.transform.localEulerAngles = turretData.SpawnLocalEuler;
        go.transform.localScale       = turretData.SpawnLocalScale;
    }

    private void UpdateIconPosition(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, e.position, e.pressEventCamera, out Vector2 lp);
        dragIcon.GetComponent<RectTransform>().localPosition = lp;
    }
}

