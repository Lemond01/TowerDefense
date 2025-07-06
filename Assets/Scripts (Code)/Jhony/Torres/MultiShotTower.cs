using UnityEngine;

public class MultiShotTower : TowerBase
{
    [Header("Multi Shot Settings")]
    public Transform[] firePoints;
    public GameObject bulletPrefab;

    protected override void Fire()
    {
        if (CurrentTarget == null || firePoints.Length == 0) return;

        float reducedDamage = 1f / firePoints.Length; // Divide el daño entre todos los puntos

        foreach (Transform point in firePoints)
        {
            GameObject bulletGO = Instantiate(bulletPrefab, point.position, point.rotation);
            Bullet bullet = bulletGO.GetComponent<Bullet>();
            
            if (bullet != null)
            {
                bullet.Seek(CurrentTarget);
                bullet.damage *= reducedDamage;
            }
        }
    }
}