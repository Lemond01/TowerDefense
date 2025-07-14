using UnityEngine;
using System.Collections.Generic;

public abstract class TowerBase : MonoBehaviour
{
    [Header("Tower Settings")]
    public float range = 5f;
    public float fireRate = 1f;
    //public int cost = 100;
    public Transform turretPivot;
    public Transform firePoint;
    public ParticleSystem fireEffect;

    [Header("Targeting")]
    protected Transform CurrentTarget;
    protected float FireCountdown = 0f;

    [Header("Detection")]
    public string enemyTag = "Enemy";

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
        Vector3 dir = CurrentTarget.position - turretPivot.position;
        dir.y = 0f;
        if (dir != Vector3.zero)
        {
            Quaternion lookRotation = Quaternion.LookRotation(dir);
            turretPivot.rotation = Quaternion.Lerp(turretPivot.rotation, lookRotation, Time.deltaTime * 10f);
        }
    }

    /// Activa el sistema de particulas
    void PlayFireEffect()
    {
        if (fireEffect != null)
        {
            fireEffect.Play();
        }
    }
    
    /// Método abstracto que se implementa en clases hijas
    protected abstract void Fire();
    
    /// Método visual para mostrar el rango en la escena
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

