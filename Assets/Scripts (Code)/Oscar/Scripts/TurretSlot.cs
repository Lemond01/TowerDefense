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
    private TurretData turretData;
    private Canvas canvas;
    private RectTransform canvasRect;
    private GameObject dragIcon;

    void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasRect = canvas.GetComponent<RectTransform>();
        turretData = GetComponent<TurretData>();
        if (turretData == null)
            Debug.LogError("TurretSlot: falta TurretData en el SlotPrefab.");
    }

    public void OnBeginDrag(PointerEventData e)
    {
        if (turretData == null || turretData.Prefab == null)
        {
            Debug.LogError("TurretData o el Prefab de la torreta no están asignados.");
            return;
        }

        // icono UI
        dragIcon = new GameObject("DragIcon");
        dragIcon.transform.SetParent(canvas.transform, false);
        var img = dragIcon.AddComponent<Image>();
        img.sprite = turretData.Icon;
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
        Debug.Log("OnEndDrag: Finalizó el arrastre. Intentando colocar la torreta.");

        if (turretData == null) return;

        // Destruimos el ícono de arrastre al inicio para que no interfiera
        if (dragIcon != null)
            Destroy(dragIcon);

        // Validamos que exista una cámara principal
        if (Camera.main == null)
        {
            Debug.LogError("No se encontró una cámara con el tag 'MainCamera' en la escena.");
            return;
        }

        // Raycast 3D desde la cámara
        Ray ray = Camera.main.ScreenPointToRay(e.position);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f, Tile))
        {
            // Si el rayo golpea algo en la capa correcta, lo mostramos en consola.
            Debug.Log("Raycast Hit: El rayo golpeó al objeto '" + hit.collider.name + "' en la capa correcta.");

            var tile = hit.collider.GetComponent<Tile>();
            if (tile != null)
            {
                Debug.Log("¡Éxito! Se encontró el componente 'Tile' en el objeto golpeado.");
                if (!tile.hasturet)
                {
                    Debug.Log("La casilla está libre. Colocando torreta...");
                    tile.hasturet = true;

                    // Instanciamos la torreta 3D como hija de la casilla
                    var turretGO = Instantiate(
                        turretData.Prefab,
                        hit.collider.transform
                    );
                    turretGO.transform.localPosition = Vector3.zero;
                    turretGO.transform.localRotation = Quaternion.identity;
                    turretGO.transform.localScale = Vector3.one;
                    Debug.Log("Torreta '" + turretGO.name + "' instanciada correctamente.");
                }
                else
                {
                    Debug.LogWarning("La casilla '" + tile.name + "' ya tiene una torreta.");
                }
            }
            else
            {
                // El rayo golpeó un objeto en la capa correcta, pero no tiene el script 'Tile'.
                Debug.LogError("Error: El objeto '" + hit.collider.name + "' está en la capa 'Tile' pero no tiene el componente 'Tile'.");
            }
        }
        else
        {
            // El rayo no golpeó nada en la capa especificada.
            Debug.LogWarning("Raycast Miss: El rayo no golpeó ningún objeto en la capa seleccionada (Tile).");
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
