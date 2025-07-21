using UnityEngine;

public class ExplosiveBullet : Bullet
{
    public float explosionRadius = 3f;

    protected override void HitTarget()
    {
        base.HitTarget();
        
        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);
        foreach (Collider nearbyObject in colliders)
        {
            if (nearbyObject.CompareTag("Enemy") && nearbyObject.transform != Target)
            {
                Enemy enemy = nearbyObject.GetComponent<Enemy>();
                if (enemy != null)
                {
                    enemy.TakeDamage(damage);
                }
            }
        }

        // Agregar VFX
    }
}