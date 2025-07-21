using UnityEngine;
using System.Collections.Generic;

public abstract class TowerBase : MonoBehaviour
{
    [Header("Tower Settings")]
    public float range = 5f;
    public float fireRate = 1f;
    public Transform turretPivot;
    public Transform firePoint;

    [Header("Targeting")]
    protected Transform CurrentTarget;
    protected float FireCountdown = 0f;

    [Header("Detection")]
    public string enemyTag = "Enemy";

    [Header("Effects")]
    public ParticleSystem fireEffect;

    [Header("Rotation Settings")]
    [SerializeField] private float LookAtRotationSharpness = 5f;
    [SerializeField] private float AimRotationSharpness = 20f;
    [SerializeField] private bool mustShoot = false;
    [SerializeField] private Quaternion m_RotationWeaponForwardToPivot = Quaternion.identity;

    private Quaternion m_PreviousPivotAimingRotation = Quaternion.identity;
    private Quaternion m_PivotAimingRotation;

    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    void Update()
    {
        if (!CurrentTarget) return;

        RotateTowardsTarget();

        if (FireCountdown <= 0f)
        {
            Fire();
            PlayFireEffect();
            FireCountdown = 1f / fireRate;
        }

        FireCountdown -= Time.deltaTime;
    }
    
    /// Busca el enemigo más cercano dentro del rango
    void UpdateTarget()
    {
        GameObject[] enemies = GameObject.FindGameObjectsWithTag(enemyTag);
        float shortestDistance = Mathf.Infinity;
        GameObject nearestEnemy = null;

        foreach (GameObject enemy in enemies)
        {
            float distToEnemy = Vector3.Distance(transform.position, enemy.transform.position);
            if (distToEnemy < shortestDistance)
            {
                shortestDistance = distToEnemy;
                nearestEnemy = enemy;
            }
        }

        if (nearestEnemy != null && shortestDistance <= range)
        {
            CurrentTarget = nearestEnemy.transform;
        }
        else
        {
            CurrentTarget = null;
        }
    }
    
    /// Gira la torreta para mirar al objetivo
    void RotateTowardsTarget()
    {
        if (CurrentTarget == null) return;

        Vector3 directionToTarget = (CurrentTarget.position - turretPivot.position).normalized;
        directionToTarget.y = 0f;

        if (directionToTarget == Vector3.zero) return;

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget) * m_RotationWeaponForwardToPivot;

        float sharpness = mustShoot ? AimRotationSharpness : LookAtRotationSharpness;
        m_PivotAimingRotation = Quaternion.Slerp(m_PreviousPivotAimingRotation, targetRotation, sharpness * Time.deltaTime);

        turretPivot.rotation = m_PivotAimingRotation;
        m_PreviousPivotAimingRotation = m_PivotAimingRotation;
    }

    /// Activa el sistema de particulas
    void PlayFireEffect()
    {
        if (fireEffect != null)
        {
            fireEffect.Play();
        }
    }
    protected abstract void Fire();
    
    /// Método visual para mostrar el rango en la escena
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

