using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveProgressBar : MonoBehaviour
{
    [Header("Referencias UI")]
    public Slider   waveSlider;
    public TMP_Text waveText;

    [Header("Configuración de Oleadas")]
    public int    totalWaves     = 5;     // Numero total de oleadas
    public int    baseEnemies    = 5;     // Enemigos en la primer oleada 
    public float  spawnInterval  = 1f;    // Tiempo entre cada enemigo

    [Header("Spawn Settings")]
    public GameObject enemyPrefab;        
    public Transform[] spawnPoints;       // Puntos desde donde salen los enemigos

    private int currentWave = 0;

    void Start()
    {
        StartCoroutine(RunWaves());
    }

    private IEnumerator RunWaves()
    {
        while (currentWave < totalWaves)
        {
            currentWave++;

            int toSpawn = baseEnemies * currentWave;
            waveSlider.maxValue = toSpawn;
            waveSlider.value    = toSpawn;

            if (waveText != null)
                waveText.text = $"Oleada {currentWave}/{totalWaves}";

            // Spawneo escalonado
            for (int i = 0; i < toSpawn; i++)
            {
                SpawnEnemy();
                yield return new WaitForSeconds(spawnInterval);
            }

            // Esperar a que todos los enemigos con tag "Enemy" sean destruidos
            while (GameObject.FindGameObjectsWithTag("Enemy").Length > 0)
            {
                waveSlider.value = GameObject.FindGameObjectsWithTag("Enemy").Length;
                yield return null;
            }
        }

        EndAllWaves();
    }

    private void SpawnEnemy()
    {
        if (enemyPrefab == null)
        {
            Debug.LogError("WaveProgressBar: enemyPrefab no asignado.");
            return;
        }
        if (spawnPoints == null || spawnPoints.Length == 0)
        {
            Debug.LogError("WaveProgressBar: spawnPoints no asignados.");
            return;
        }

        // Escoge un punto aleatorio
        Transform sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        var go = Instantiate(enemyPrefab, sp.position, Quaternion.identity);
        
        go.tag = "Enemy";
    }

    private void EndAllWaves()
    {
        Debug.Log("¡Has completado todas las oleadas!");
        
    }
}
