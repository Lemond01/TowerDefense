using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Estad�sticas del Enemigo")]
    [SerializeField] protected float _lifePoints = 100f;
    [SerializeField] protected float _speed = 2f;
    [SerializeField] protected float _damage = 10f;

    [Header("Prefab del pr�ximo enemigo para evoluci�n")]
    [SerializeField] protected GameObject enemigoEvolucionado;

    // Acceso p�blico a las variables
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

    public void RecibirDa�o(float cantidad)
    {
        _lifePoints -= cantidad;
        if (_lifePoints <= 0)
        {
            Morir();
        }
    }

    protected virtual void Morir()
    {
        
        Destroy(gameObject);
    }

    // Llamar cuando quieras que el enemigo evolucione
    public void Evolucionar()
    {
        if (enemigoEvolucionado != null)
        {
            Instantiate(enemigoEvolucionado, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}


