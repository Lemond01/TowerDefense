using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class WaveManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public Slider   waveSlider;
    public TMP_Text waveText;

    [Header("Oleadas")]
    public List<int> enemiesPerWave = new List<int>();

    [Header("Spawn Settings")]
    public float          spawnInterval = 1f;
    public List<GameObject> enemyPrefabs;
    public Transform[]      spawnPoints;

    private int currentWave      = 0;
    private int enemiesRemaining = 0;

    void Start()
    {
        // Validaciones
        if (enemiesPerWave == null || enemiesPerWave.Count == 0)
        {
            Debug.LogError("[WaveManager] No hay configuración de oleadas.");
            enabled = false;
            return;
        }
        if (waveSlider == null || waveText == null ||
            enemyPrefabs == null || enemyPrefabs.Count == 0 ||
            spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("[WaveManager] Faltan referencias en el Inspector.");
            enabled = false;
            return;
        }

        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        for (currentWave = 1; currentWave <= enemiesPerWave.Count; currentWave++)
        {
            enemiesRemaining = enemiesPerWave[currentWave - 1];

            waveSlider.maxValue = enemiesRemaining;
            waveSlider.value    = enemiesRemaining;
            waveText.text       = $"Oleada {currentWave}/{enemiesPerWave.Count}";

            // Spawneo escalonado
            for (int i = 0; i < enemiesRemaining; i++)
            {
                SpawnRandomEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            // Espera hasta que todos los enemigos mueran
            while (enemiesRemaining > 0)
            {
                waveSlider.value = enemiesRemaining;
                yield return null;
            }
        }

        EndAllWaves();
    }

    private void SpawnRandomEnemy()
    {
        // Elegir un prefab y punto de spawn al azar
        var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        var sp     = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var go     = Instantiate(prefab, sp.position, sp.rotation);

        // Suscripción al evento de muerte
        var enemy = go.GetComponent<Enemy>();
        if (enemy != null)
        {
            enemy.OnDeath += HandleEnemyDeath;
        }
    }

    private void HandleEnemyDeath(Enemy dead)
    {
        enemiesRemaining = Mathf.Max(0, enemiesRemaining - 1);
        dead.OnDeath    -= HandleEnemyDeath;
    }

    private void EndAllWaves()
    {
        SceneManager.LoadScene("Win");
        Debug.Log("¡Has completado todas las oleadas!");
        
    }
}

