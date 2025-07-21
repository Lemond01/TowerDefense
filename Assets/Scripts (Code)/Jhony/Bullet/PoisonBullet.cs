using UnityEngine;
using System.Collections;

// Bala venenosa
public class PoisonBullet : Bullet
{
    public float poisonDuration = 5f;
    public float poisonTickDamage = 1f;
    public float tickInterval = 1f;

    protected override void HitTarget()
    {
        base.HitTarget();
        if (Target != null)
        {
            StartCoroutine(ApplyPoison(Target));
        }
    }

    private IEnumerator ApplyPoison(Transform enemy)
    {
        float timer = 0f;
        Enemy enemyScript = enemy.GetComponent<Enemy>();

        while (timer < poisonDuration)
        {
            if (enemyScript != null)
            {
                enemyScript.TakeDamage(poisonTickDamage);
            }
            timer += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }
    }
}