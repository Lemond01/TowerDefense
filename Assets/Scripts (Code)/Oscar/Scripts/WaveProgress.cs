using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveManager : MonoBehaviour
{
    [Header("Referencias UI")]
    public Slider   waveSlider;
    public TMP_Text waveText;

    [Header("Configuración de Oleadas")]
    public int totalWaves = 5;                  // Oleadas totales
    public AnimationCurve enemiesMultiplier;     // Multiplicador por wave (x = wave/totalWaves, y = factor)
    public float spawnInterval = 1f;            // Tiempo entre spawns

    [Header("Spawn Settings")]
    public List<GameObject> enemyPrefabs;       // Hasta 5 prefabs distintos
    public Transform[]      spawnPoints;        // Múltiples spawns

    private int currentWave      = 0;
    private int enemiesRemaining = 0;

    void Start()
    {
        if (enemyPrefabs == null || enemyPrefabs.Count == 0 ||
            spawnPoints == null || spawnPoints.Length == 0 ||
            waveSlider == null || waveText == null)
        {
            Debug.LogError("[WaveManager] Faltan referencias en el Inspector.");
            return;
        }

        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        while (currentWave < totalWaves)
        {
            currentWave++;

            // Calcula cuántos enemigos en esta oleada:
            // base = totalWaves * factor en curva (0→1)
            float factor = enemiesMultiplier.Evaluate((float)(currentWave - 1) / (totalWaves - 1));
            int   toSpawn = Mathf.CeilToInt(totalWaves * factor);

            enemiesRemaining = toSpawn;
            waveSlider.maxValue = toSpawn;
            waveSlider.value    = toSpawn;
            waveText.text       = $"Oleada {currentWave}/{totalWaves}";

            // Spawnea escalonado
            for (int i = 0; i < toSpawn; i++)
            {
                SpawnRandomEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            // Espera hasta que todos los enemigos de esta oleada hayan muerto
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
        // Elige prefab y punto al azar
        var prefab = enemyPrefabs[Random.Range(0, enemyPrefabs.Count)];
        var sp     = spawnPoints[Random.Range(0, spawnPoints.Length)];

        var go = Instantiate(prefab, sp.position, sp.rotation);
        go.tag = "Enemy";

        // Suscríbete a su evento de muerte
        var e = go.GetComponent<Enemy>();
        if (e != null)
            e.OnDeath += HandleEnemyDeath;
        else
            Debug.LogWarning($"[WaveManager] El prefab {prefab.name} no tiene componente Enemy.");
    }

    private void HandleEnemyDeath(Enemy dead)
    {
        enemiesRemaining = Mathf.Max(0, enemiesRemaining - 1);
        dead.OnDeath    -= HandleEnemyDeath;
    }

    private void EndAllWaves()
    {
        Debug.Log("¡Has completado todas las oleadas!");
        
    }
}
