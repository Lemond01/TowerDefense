using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;

public class VegaCore : MonoBehaviour
{
    [Header("El Pentágono")]
    [SerializeField] private float baseHealth = 200f;
    [SerializeField] private string gameOverScene = "GameOver";

    [Header("UI Hearts")]
    [SerializeField] private Image[] heartImages;

    private float _maxHealth;
    private List<Enemy> _attackers = new List<Enemy>();

    private void Start()
    {
        _maxHealth = baseHealth;
        UpdateHeartUI();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && !_attackers.Contains(enemy))
            {
                _attackers.Add(enemy);
                Debug.Log($"[{enemy.name}] atacando la base por {enemy.Damage} de daño.");
                StartCoroutine(HandleEnemyDamage(enemy));
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Debug.Log($"VegaCore recibió daño. Vida restante: {baseHealth}");

            Enemy enemy = other.GetComponent<Enemy>();
            if (enemy != null && _attackers.Contains(enemy))
            {
                _attackers.Remove(enemy);
            }
        }
    }

    private IEnumerator HandleEnemyDamage(Enemy enemy)
    {
        while (enemy != null && _attackers.Contains(enemy))
        {
            TakeDamage(enemy.Damage);
            yield return new WaitForSeconds(2f);
        }
    }

    private void TakeDamage(float amount)
    {
        baseHealth -= amount;
        baseHealth = Mathf.Max(0f, baseHealth);
        UpdateHeartUI();

        if (baseHealth <= 0f)
        {
            SceneManager.LoadScene(gameOverScene);
        }
    }

    private void UpdateHeartUI()
    {
        float healthPercentage = baseHealth / _maxHealth;
        int heartsToShow = Mathf.CeilToInt(healthPercentage * heartImages.Length);

        for (int i = 0; i < heartImages.Length; i++)
        {
            heartImages[i].enabled = i < heartsToShow;
        }
    }
}
