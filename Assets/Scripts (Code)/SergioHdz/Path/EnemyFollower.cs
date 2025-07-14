using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFollower : Enemy
{
    [Header("Path")]
    public Path path;
    //public float moveSpeed = 2f;

    [Header("Waypoint donde cambia el modelo")]
    public Transform waypointDeCambio;

    [Header("Modelos")]
    public GameObject modeloInicialPrefab;
    public GameObject modeloEvolucionadoPrefab;

    [Header("Efectos")]
    public float tiempoDeEvolucion = 1.5f;
    public Material materialBrillante;             // Material brillante temporal
    private Material[] materialesOriginales;       // Para restaurar si es necesario
    private Renderer modeloRenderer;


    private List<Transform> waypoints;
    private int currentIndex = 0;
    private bool esperandoCambio = false;

    private GameObject instanciaActual;

    private void Start()
    {
        if (path == null || waypointDeCambio == null)
        {
            Debug.LogError("[EnemyFollower] Asigna el path y el waypoint de cambio.");
            enabled = false;
            return;
        }

        waypoints = path.GetWaypoints();

        // Instanciamos el modelo inicial
        if (modeloInicialPrefab != null)
        {
            instanciaActual = Instantiate(modeloInicialPrefab, transform.position, Quaternion.identity, transform);
        }
    }

    private void Update()
    {
        if (waypoints == null || currentIndex >= waypoints.Count || esperandoCambio)
            return;

        Transform target = waypoints[currentIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * (Speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // ¿Llegó al waypoint de cambio?
            if (target == waypointDeCambio)
            {
                StartCoroutine(CambiarModelo());
            }

            currentIndex++;
        }
    }

    IEnumerator CambiarModelo()
    {
        esperandoCambio = true;

        Debug.Log("[EnemyFollower] Iniciando evolución...");

        float duracion = tiempoDeEvolucion;
        float tiempo = 0f;

        Vector3 posicionInicial = instanciaActual.transform.localPosition;
        Quaternion rotacionInicial = instanciaActual.transform.localRotation;

        float altura = 1.5f;
        float velocidadRotacion = 360f;

        // Obtener el renderer del modelo actual
        if (instanciaActual != null)
        {
            modeloRenderer = instanciaActual.GetComponentInChildren<Renderer>();

            if (modeloRenderer != null && materialBrillante != null)
            {
                // Guardar materiales originales
                materialesOriginales = modeloRenderer.materials;

                // Reemplazar materiales por uno brillante
                Material[] nuevos = new Material[modeloRenderer.materials.Length];
                for (int i = 0; i < nuevos.Length; i++)
                    nuevos[i] = materialBrillante;

                modeloRenderer.materials = nuevos;
            }
        }

        while (tiempo < duracion)
        {
            tiempo += Time.deltaTime;
            float t = tiempo / duracion;

            if (instanciaActual != null)
            {
                float alturaActual = Mathf.Sin(t * Mathf.PI) * altura;
                instanciaActual.transform.localPosition = posicionInicial + Vector3.up * alturaActual;
                instanciaActual.transform.Rotate(Vector3.up, velocidadRotacion * Time.deltaTime, Space.Self);
            }

            yield return null;
        }

        // Destruir modelo anterior
        if (instanciaActual != null)
        {
            Destroy(instanciaActual);
        }

        // Instanciar el modelo evolucionado
        if (modeloEvolucionadoPrefab != null)
        {
            instanciaActual = Instantiate(modeloEvolucionadoPrefab, transform.position, Quaternion.identity, transform);
            Debug.Log("[EnemyFollower] Evolución completada, nuevo modelo activo.");
        }

        esperandoCambio = false;
    }


}





