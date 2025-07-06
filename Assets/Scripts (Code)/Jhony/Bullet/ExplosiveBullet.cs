using UnityEngine;
using System.Collections;

// Bala explosiva
public class ExplosiveBullet : Bullet
{
    public float explosionRadius = 3f;

    protected override void HitTarget()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach (Collider collider in colliders)
        {
            if (collider.CompareTag("Enemy"))
            {
                // Aquí deberías tener un  método ApplyDamage checarlo con Fabio
            }
        }

        gameObject.SetActive(false);
    }
}