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
    [Range(0.1f, 1f)] public float iconScale = 0.8f;

    [Header("Map 3D (Tiles)")]
    
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

        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);
        var img = dragIcon.AddComponent<Image>();
        img.sprite        = turretData.Icon;
        img.raycastTarget = false;

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
        if (dragIcon != null) Destroy(dragIcon);
        if (turretData?.Prefab == null || Camera.main == null) return;

        // Lanza un rayo desde la cámara.
        var ray = Camera.main.ScreenPointToRay(e.position);
        if (!Physics.Raycast(ray, out RaycastHit hit, 200f, tileLayer))
            return;
        
        GameObject cubeObject = hit.collider.gameObject;

       
        Transform position3DTransform = cubeObject.transform.parent;
        if (position3DTransform == null)
        {
            Debug.LogError($"El objeto '{cubeObject.name}' no tiene un padre. No se puede encontrar 'position3D'.");
            return;
        }

       
        if (cubeObject.transform.childCount > 0)
        {
            Debug.Log("TurretSlot: Este espacio ya está ocupado.");
            return;
        }

        
        int cost = turretData.Cost;
        if (!MoneyManager.Instance.Spend(cost))
        {
            Debug.Log("TurretSlot: Dinero insuficiente.");
            return;
        }

        
        var position3DRenderer = position3DTransform.GetComponent<MeshRenderer>();
        if (position3DRenderer != null && turretData.PlacementMaterial != null)
        {
            position3DRenderer.material = turretData.PlacementMaterial;
        }
        else
        {
            Debug.LogWarning($"El padre '{position3DTransform.name}' no tiene MeshRenderer o falta el PlacementMaterial.");
        }

        
        var turretGO = Instantiate(turretData.Prefab, cubeObject.transform);
        turretGO.transform.localPosition = Vector3.zero;
        turretGO.transform.localRotation = Quaternion.identity;
    }

    private void UpdateIconPosition(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect, e.position, e.pressEventCamera, out Vector2 lp
        );
        dragIcon.GetComponent<RectTransform>().localPosition = lp;
    }
}
