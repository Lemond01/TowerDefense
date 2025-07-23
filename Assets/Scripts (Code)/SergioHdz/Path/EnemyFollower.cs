using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFollower : Enemy
{
    [Header("Path")]
    public Path path;

    [Header("Waypoint donde cambia el modelo")]
    public Transform waypointDeCambio;

    [Header("Waypoint donde ataca")]
    public Transform waypointDeAtaque; // Nuevo: punto donde comenzará a atacar

    [Header("Modelos")]
    public GameObject modeloInicialPrefab;
    public GameObject modeloEvolucionadoPrefab;

    [Header("Efectos")]
    public float tiempoDeEvolucion = 1.5f;
    public Material materialBrillante;
    private Material[] materialesOriginales;
    private Renderer modeloRenderer;

    [Header("Animaciones")]
    public Animator animator; // Referencia al Animator

    private List<Transform> waypoints;
    private int currentIndex = 0;
    private bool esperandoCambio = false;
    private bool haLlegadoAlAtaque = false; // Nuevo: controla si llegó al punto de ataque

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
            animator = instanciaActual.GetComponent<Animator>(); // Obtener el Animator del modelo instanciado
        }

        // Iniciar animación de caminar
        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    private void Update()
    {
        if (waypoints == null || currentIndex >= waypoints.Count || esperandoCambio)
            return;

        Transform target = waypoints[currentIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * (Speed * Time.deltaTime);

        // Rotar hacia la dirección del movimiento
        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            // ¿Llegó al waypoint de cambio?
            if (target == waypointDeCambio)
            {
                StartCoroutine(CambiarModelo());
            }

            // ¿Llegó al waypoint de ataque?
            if (target == waypointDeAtaque && !haLlegadoAlAtaque)
            {
                haLlegadoAlAtaque = true;
                StartCoroutine(IniciarAtaque());
            }

            currentIndex++;
        }
    }

    IEnumerator CambiarModelo()
    {
        esperandoCambio = true;

        Debug.Log("[EnemyFollower] Iniciando evolución...");

        // Detener animación de caminar
        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
        }

        float duracion = tiempoDeEvolucion;
        float tiempo = 0f;

        Vector3 posicionInicial = instanciaActual.transform.localPosition;
        Quaternion rotacionInicial = instanciaActual.transform.localRotation;

        float altura = 1.5f;
        float velocidadRotacion = 360f;

        if (instanciaActual != null)
        {
            modeloRenderer = instanciaActual.GetComponentInChildren<Renderer>();

            if (modeloRenderer != null && materialBrillante != null)
            {
                materialesOriginales = modeloRenderer.materials;
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

        if (instanciaActual != null)
        {
            Destroy(instanciaActual);
        }

        if (modeloEvolucionadoPrefab != null)
        {
            instanciaActual = Instantiate(modeloEvolucionadoPrefab, transform.position, Quaternion.identity, transform);
            animator = instanciaActual.GetComponent<Animator>(); // Obtener el Animator del nuevo modelo

            // Continuar caminando si no ha llegado al punto de ataque
            if (!haLlegadoAlAtaque && animator != null)
            {
                animator.SetBool("IsWalking", true);
            }

            Debug.Log("[EnemyFollower] Evolución completada, nuevo modelo activo.");
        }

        esperandoCambio = false;
    }

    IEnumerator IniciarAtaque()
    {
        // Detener el movimiento
        SetVelocidad(0f);

        if (animator != null)
        {
            // Detener animación de caminar y comenzar ataque
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("Attack");

            // Esperar a que termine la animación de ataque
            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            // Aquí puedes poner lógica de daño al jugador
            Debug.Log("Enemigo atacando al jugador!");

            // Opcional: repetir el ataque
            while (true)
            {
                yield return new WaitForSeconds(2f); // Esperar 2 segundos entre ataques
                animator.SetTrigger("Attack");
                // Aplicar daño al jugador aquí
            }
        }
    }
}





