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

    void Start()
    {
        InvokeRepeating(nameof(UpdateTarget), 0f, 0.5f);
    }

    void Update()
    {
        if (CurrentTarget == null) return;

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
        Vector3 dir = CurrentTarget.position - transform.position;
        Quaternion lookRotation = Quaternion.LookRotation(dir);
        Vector3 rotation = Quaternion.Lerp(turretPivot.rotation, lookRotation, Time.deltaTime * 10f).eulerAngles;
        turretPivot.rotation = Quaternion.Euler(0f, rotation.y, 0f); // Solo rota en Y
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

