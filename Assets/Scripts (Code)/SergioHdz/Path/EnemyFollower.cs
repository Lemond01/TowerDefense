using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class EnemyFollower : Enemy
{
    [Header("Path Settings")]
    public Path path;
    public Transform waypointDeCambio;
    public Transform waypointDeAtaque;

    [Header("Model Settings")]
    public GameObject modeloInicialPrefab;
    public GameObject modeloEvolucionadoPrefab;

    [Header("Evolution Effects")]
    public float tiempoDeEvolucion = 1.5f;
    public Material materialBrillante;
    private Material[] materialesOriginales;
    private Renderer modeloRenderer;

    [Header("Death Effects")]
    public ParticleSystem deathParticles;
    public AudioClip deathSound;

    [Header("Manual Evolution")]
    public bool forzarEvolucion = false;

    private List<Transform> waypoints;
    private int currentIndex = 0;
    private bool esperandoCambio = false;
    private bool haLlegadoAlAtaque = false;
    private GameObject instanciaActual;

    protected override void Start()
    {
        base.Start();

        if (path == null || waypointDeCambio == null)
        {
            Debug.LogError("[EnemyFollower] Asigna el path y el waypoint de cambio.");
            enabled = false;
            return;
        }

        waypoints = path.GetWaypoints();

        if (modeloInicialPrefab != null)
        {
            instanciaActual = Instantiate(modeloInicialPrefab, transform.position, Quaternion.identity, transform);
            animator = instanciaActual.GetComponentInChildren<Animator>();
        }

        if (animator != null)
        {
            animator.SetBool("IsWalking", true);
        }
    }

    private void Update()
    {
        if (isDead || waypoints == null || currentIndex >= waypoints.Count || esperandoCambio)
            return;

        if (forzarEvolucion)
        {
            forzarEvolucion = false;
            StartCoroutine(CambiarModelo());
            return;
        }

        Transform target = waypoints[currentIndex];
        Vector3 direction = (target.position - transform.position).normalized;
        transform.position += direction * (Speed * Time.deltaTime);

        if (direction != Vector3.zero)
        {
            transform.rotation = Quaternion.LookRotation(direction);
        }

        if (Vector3.Distance(transform.position, target.position) < 0.1f)
        {
            if (target == waypointDeCambio)
            {
                StartCoroutine(CambiarModelo());
            }

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
            animator = instanciaActual.GetComponentInChildren<Animator>();

            if (!haLlegadoAlAtaque && animator != null)
            {
                animator.SetBool("IsWalking", true);
            }
        }

        esperandoCambio = false;
    }

    IEnumerator IniciarAtaque()
    {
        SetVelocidad(0f);

        if (animator != null)
        {
            animator.SetBool("IsWalking", false);
            animator.SetTrigger("Attack");

            yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);

            while (!isDead)
            {
                yield return new WaitForSeconds(2f);
                animator.SetTrigger("Attack");
            }
        }
    }

    public override void TakeDamage(float amount)
    {
        if (isDead) return;

        base.TakeDamage(amount);

        if (_lifePoints <= 0f)
        {
            StopAllCoroutines();
            esperandoCambio = true;
            SetVelocidad(0f);
        }
    }

    protected override void Morir()
    {
        base.Morir();

        if (deathParticles != null)
        {
            Instantiate(deathParticles, transform.position, Quaternion.identity);
        }

        if (deathSound != null)
        {
            AudioSource.PlayClipAtPoint(deathSound, transform.position);
        }
    }
}
