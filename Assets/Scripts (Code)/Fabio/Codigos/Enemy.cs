using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Estadísticas del Enemigo")]
    [SerializeField] public float _lifePoints = 100f;
    [SerializeField] public float _speed = 2f;
    [SerializeField] public float _damage = 10f;

    [Header("Recompensa al morir")]
    [SerializeField] protected int reward = 10;

    protected bool isDead = false;
    public Animator animator;

    public float LifePoints => _lifePoints;
    public float Speed => _speed;
    public float Damage => _damage;

    // Cambiado a virtual para permitir override
    protected virtual void Start()
    {
        animator = GetComponentInChildren<Animator>();
    }

    public void SetVida(float nuevaVida)
    {
        _lifePoints = nuevaVida;
    }

    public void SetVelocidad(float nuevaVelocidad)
    {
        _speed = nuevaVelocidad;
    }

    // Cambiado a virtual para permitir override
    public virtual void TakeDamage(float amount)
    {
        if (isDead) return;

        _lifePoints -= amount;
        if (_lifePoints <= 0f)
        {
            Morir();
        }
    }

    // Cambiado a virtual para permitir override
    protected virtual void Morir()
    {
        isDead = true;

        if (animator != null)
        {
            animator.SetTrigger("Die");
        }

        var collider = GetComponent<Collider>();
        if (collider != null) collider.enabled = false;

        Destroy(gameObject, 2f);

        if (MoneyManager.Instance != null)
            MoneyManager.Instance.Earn(reward);
    }
}


