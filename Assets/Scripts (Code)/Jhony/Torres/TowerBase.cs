using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;
using UnityEngine.VFX;

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
    public VisualEffect visualFireEffect;
    
    [Header("Rotation Settings")]
    [SerializeField] private float lookAtRotationSharpness = 5f;
    [SerializeField] private float aimRotationSharpness = 20f;
    [SerializeField] private bool mustShoot = false;
    [SerializeField] private Quaternion mRotationWeaponForwardToPivot = Quaternion.identity;

    private Quaternion _mPreviousPivotAimingRotation = Quaternion.identity;
    private Quaternion _mPivotAimingRotation;
    
   public Animator animator;
   private static readonly int KAnimIsActiveParameter = Animator.StringToHash("IsActive");

    void Start()
    {
        animator = GetComponent<Animator>();
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    void Update()
    {
        if (!CurrentTarget)
        {
            if (animator) animator.SetBool(KAnimIsActiveParameter, false);
            return;
        }
        RotateTowardsTarget();
        if (FireCountdown <= 0f)
        {
            if (animator)
            {
                animator.SetTrigger(KAnimIsActiveParameter);
            }
            else
            {
                Fire();
            }
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

        Quaternion targetRotation = Quaternion.LookRotation(directionToTarget) * mRotationWeaponForwardToPivot;

        float sharpness = mustShoot ? aimRotationSharpness : lookAtRotationSharpness;
        _mPivotAimingRotation = Quaternion.Slerp(_mPreviousPivotAimingRotation, targetRotation, sharpness * Time.deltaTime);

        turretPivot.rotation = _mPivotAimingRotation;
        _mPreviousPivotAimingRotation = _mPivotAimingRotation;
    }

    /// Activa el sistema de particulas
    void PlayFireEffect()
    {
        if (fireEffect != null)
        {
            fireEffect.Play();
            Invoke(nameof(StopParticleEffect), 1f);
        }
        else if (visualFireEffect != null)
        {
            visualFireEffect.Play();
            Invoke(nameof(StopVisualEffect), 1f);
        }
    }
    
    void StopParticleEffect()
    {
        if (fireEffect != null)
        {
            fireEffect.Stop();
        }
    }

    void StopVisualEffect()
    {
        if (visualFireEffect != null)
        {
            visualFireEffect.Stop();
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

