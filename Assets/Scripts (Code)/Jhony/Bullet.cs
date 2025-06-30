using UnityEngine;

public class Bullet : MonoBehaviour
{
    [Header("Bullet Settings")]
    public float speed = 20f;
    public float damage = 50f;
    public float maxRange = 10f;

    private Transform _target;
    private Vector3 _spawnPosition;

    void OnEnable()
    {
        _spawnPosition = transform.position;
    }

    public void Seek(Transform t)
    {
        _target = t;
    }

    void Update()
    {
        if (_target == null)
        {
            gameObject.SetActive(false);
            return;
        }

        if (Vector3.Distance(_spawnPosition, transform.position) > maxRange)
        {
            gameObject.SetActive(false);
            return;
        }

        Vector3 dir = _target.position - transform.position;
        float distanceThisFrame = speed * Time.deltaTime;

        if (dir.magnitude <= distanceThisFrame)
        {
            HitTarget();
            return;
        }

        transform.Translate(dir.normalized * distanceThisFrame, Space.World);
        transform.LookAt(_target);
    }

    void HitTarget()
    {

        gameObject.SetActive(false);
    }
}