 using UnityEngine;
 using UnityEngine.SceneManagement;
 using System.Collections;

    public class VegaCore : MonoBehaviour
    {
        [Header("El Pentagono")]
        [SerializeField] private float baseHealth = 100f;
        [SerializeField] private string gameOverScene = "GameOver";

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Enemy"))
            {
                Enemy enemy = other.GetComponent<Enemy>();
                if (enemy != null)
                {
                    StartCoroutine(HandleEnemyCollision(enemy));
                }
            }
        }

        private IEnumerator HandleEnemyCollision(Enemy enemy)
        {
            float damage = enemy.Damage;
            yield return new WaitForSeconds(2f);

            if (enemy != null)
            {
                Destroy(enemy.gameObject);
                TakeDamage(damage);
            }
        }

        private void TakeDamage(float amount)
        {
            baseHealth -= amount;
            if (baseHealth <= 0f)
            {
                SceneManager.LoadScene(gameOverScene);
            }
        }
    }
 
