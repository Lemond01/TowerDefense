using UnityEngine;
public class TowerBasic : TowerBase
{
    public GameObject bulletPrefab;
    protected override void Fire()
    {
        GameObject bulletGO = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Bullet bullet = bulletGO.GetComponent<Bullet>();
        if (bullet != null && CurrentTarget != null)
        {
            bullet.Seek(CurrentTarget);
        }
    }
}