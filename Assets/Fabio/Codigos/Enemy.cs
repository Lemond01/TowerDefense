using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Estadísticas del Enemigo")]
    [SerializeField] protected float vida = 100f;
    [SerializeField] protected float velocidad = 2f;
    [SerializeField] protected float daño = 10f;

    [Header("Prefab del próximo enemigo para evolución")]
    [SerializeField] protected GameObject enemigoEvolucionado;

    // Acceso público a las variables
    public float Vida => vida;
    public float Velocidad => velocidad;
    public float Daño => daño;

    // Cambiar valores
    public void SetVida(float nuevaVida)
    {
        vida = nuevaVida;
    }

    public void SetVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }

    public void RecibirDaño(float cantidad)
    {
        vida -= cantidad;
        if (vida <= 0)
        {
            Morir();
        }
    }

    protected virtual void Morir()
    {
        // Aquí puedes agregar efectos, partículas, sonido, etc.
        Destroy(gameObject);
    }

    // Llamar cuando quieras que el enemigo evolucione
    public void Evolucionar()
    {
        if (enemigoEvolucionado != null)
        {
            // Instanciamos al nuevo enemigo en la misma posición y rotación
            Instantiate(enemigoEvolucionado, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}

