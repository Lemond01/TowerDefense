using UnityEngine;
using UnityEngine.SceneManagement;

public class Delay : MonoBehaviour
{
    [SerializeField] private float delay = 3f; 

    void Start()
    {
        Invoke(nameof(LoadCredits), delay);
    }

    void LoadCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}