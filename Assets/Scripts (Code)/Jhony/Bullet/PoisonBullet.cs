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
        if (_target != null)
        {
            StartCoroutine(ApplyPoison(_target));
        }

        gameObject.SetActive(false);
    }

    private IEnumerator ApplyPoison(Transform enemy)
    {
        float timer = 0f;
        while (timer < poisonDuration)
        {
            // enemy.GetComponent<Enemy>()?.ApplyDamage(poisonTickDamage);
            timer += tickInterval;
            yield return new WaitForSeconds(tickInterval);
        }
    }
}
