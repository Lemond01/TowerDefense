using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveProgressBar : MonoBehaviour
{
    [Header("Referencias UI")]
    public Slider waveSlider;
    public TMP_Text waveText;

    [Header("Configuración oleadas")]
    public int totalWaves = 5;

    private int currentWave = 0;
    private bool waveInProgress = false;

    void Start()
    {
        StartNextWave();
    }

    void Update()
    {
        if (!waveInProgress) return;

        
        int enemiesAlive = GameObject.FindGameObjectsWithTag("Enemy").Length;
        waveSlider.value = enemiesAlive;

        
        if (enemiesAlive == 0)
        {
            waveInProgress = false;
            if (currentWave < totalWaves)
                StartNextWave();
            else
                EndAllWaves();
        }
    }

    void StartNextWave()
    {
        currentWave++;
        waveInProgress = true;

        int spawnCount = CalculateEnemiesForWave(currentWave);
        waveSlider.maxValue = spawnCount;
        waveSlider.value    = spawnCount;

        if (waveText != null)
            waveText.text = $"Oleada {currentWave}/{totalWaves}";

        
    }

    private int CalculateEnemiesForWave(int wave)
    {
        return wave * 5;
    }

    private void EndAllWaves()
    {
        Debug.Log("¡Has completado todas las oleadas!");
       
    }
}


