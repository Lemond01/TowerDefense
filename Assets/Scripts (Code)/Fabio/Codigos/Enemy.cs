using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Estad�sticas del Enemigo")]
    [SerializeField] public float _lifePoints = 100f;
    [SerializeField] public float _speed = 2f;
    [SerializeField] public float _damage = 10f;

    public float LifePoints => _lifePoints;
    public float Speed => _speed;
    public float Damage => _damage;

    // Cambiar valores
    public void SetVida(float nuevaVida)
    {
        _lifePoints = nuevaVida;
    }

    public void SetVelocidad(float nuevaVelocidad)
    {
        _speed = nuevaVelocidad;
    }
    
    public void TakeDamage(float amount)
    {
        _lifePoints -= amount;
        if (_lifePoints <= 0f)
        {
            Morir();
        }
    }

    protected virtual void Morir()
    {
        
        Destroy(gameObject);
    }

   
    
}


