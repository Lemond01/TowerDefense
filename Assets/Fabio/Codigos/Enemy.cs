using UnityEngine;

public class Enemy : MonoBehaviour
{
    [Header("Estad�sticas del Enemigo")]
    [SerializeField] protected float vida = 100f;
    [SerializeField] protected float velocidad = 2f;
    [SerializeField] protected float da�o = 10f;

    [Header("Prefab del pr�ximo enemigo para evoluci�n")]
    [SerializeField] protected GameObject enemigoEvolucionado;

    // Acceso p�blico a las variables
    public float Vida => vida;
    public float Velocidad => velocidad;
    public float Da�o => da�o;

    // Cambiar valores
    public void SetVida(float nuevaVida)
    {
        vida = nuevaVida;
    }

    public void SetVelocidad(float nuevaVelocidad)
    {
        velocidad = nuevaVelocidad;
    }

    public void RecibirDa�o(float cantidad)
    {
        vida -= cantidad;
        if (vida <= 0)
        {
            Morir();
        }
    }

    protected virtual void Morir()
    {
        // Aqu� puedes agregar efectos, part�culas, sonido, etc.
        Destroy(gameObject);
    }

    // Llamar cuando quieras que el enemigo evolucione
    public void Evolucionar()
    {
        if (enemigoEvolucionado != null)
        {
            // Instanciamos al nuevo enemigo en la misma posici�n y rotaci�n
            Instantiate(enemigoEvolucionado, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}

