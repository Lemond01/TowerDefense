using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class TurretSlot : MonoBehaviour,IBeginDragHandler,IDragHandler, IEndDragHandler
{
    [Header("Configuración de la torreta")]
    public GameObject TurretPrefab;   
    public Image      Icon;           
    public TMP_Text   CostText;       

    [Header("Ajustes de arrastre")]
    [Range(0.2f,1f)] public float iconAlpha = 0.8f;
    public LayerMask  tileMask;       

    TurretData    data;
    Canvas        canvas;
    RectTransform canvasRect;
    GameObject    dragIcon;

    void Awake()
    {
        
        canvas     = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();

        data = TurretPrefab.GetComponent<TurretData>();
        if (data == null) Debug.LogError("TurretPrefab necesita TurretData!");

       
        Icon.sprite   = data.icon;
        CostText.text = data.cost.ToString();
    }

    public void OnBeginDrag(PointerEventData e)
    {
        
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);
        var img = dragIcon.AddComponent<Image>();
        img.sprite        = data.icon;
        img.raycastTarget = false;
        
        var rt = img.rectTransform;
        rt.sizeDelta = Icon.rectTransform.sizeDelta;
        rt.pivot     = Icon.rectTransform.pivot;
        
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
        
        Vector3 sp = new Vector3(e.position.x, e.position.y, -Camera.main.transform.position.z);
        Vector3 wp = Camera.main.ScreenToWorldPoint(sp);

        
        Collider2D hit = Physics2D.OverlapPoint(wp, tileMask);
        if (hit)
        {
            var tile = hit.GetComponent<Tile>();
            if (tile != null && !tile.hasturet)
            {
               
                var sr = hit.GetComponent<SpriteRenderer>();
                if (sr != null)
                {
                    sr.sprite  = data.icon;
                    sr.enabled = true;        
                }

                tile.hasturet = true;
            }
        }

        // limpiamos el icono flotante
        if (dragIcon != null) Destroy(dragIcon);
    }


    void UpdateIconPosition(PointerEventData e)
    {
        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            e.position,
            e.pressEventCamera,
            out Vector2 lp);
        dragIcon.GetComponent<RectTransform>().localPosition = lp;
    }
}
