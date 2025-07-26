
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 20f;
    public float damage = 50f;
    public float maxRange = 10f;

    protected Transform Target;
    private Vector3 _spawnPosition;

    void OnEnable()
    {
        _spawnPosition = transform.position;
    }

    public void Seek(Transform t)
    {
        Target = t;
    }

    void Update()
    {
        if (Target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (Vector3.Distance(_spawnPosition, transform.position) > maxRange)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 dir = Target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(Target);
    }
    
    protected virtual void HitTarget()
    {
        Collider[] hits = Physics.OverlapSphere(transform.position, 0.2f);

        foreach (var hit in hits)
        {
            Enemy enemy = hit.GetComponent<Enemy>();
            if (enemy != null)
            {
                enemy.TakeDamage(damage);
                break;
            }
        }

        Destroy(gameObject);
    }

}
