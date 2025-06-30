using UnityEngine;
using System.Collections.Generic;

public abstract class TowerBase : MonoBehaviour
{
    [Header("Tower Settings")]
    public float range = 5f;
    public float fireRate = 1f;
    public int cost = 100;
    public Transform turretPivot;
    public Transform firePoint;

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
            FireCountdown = 1f / fireRate;
        }

        FireCountdown -= Time.deltaTime;
    }

    /// <summary>
    /// Busca el enemigo más cercano dentro del rango
    /// </summary>
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

    /// <summary>
    /// Gira la torreta para mirar al objetivo
    /// </summary>
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


    /// <summary>
    /// Método abstracto que se implementa en clases hijas
    /// </summary>
    protected abstract void Fire();

    /// <summary>
    /// Método visual para mostrar el rango en la escena
    /// </summary>
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, range);
    }
}

