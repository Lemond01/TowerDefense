using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveProgressBar : MonoBehaviour
{
    [Header("Referencias UI")]
    public Slider waveSlider;          
    public TMP_Text   waveText;            

    [Header("Configuración oleadas")]
    public int    totalWaves = 5;

    private int currentWave = 0;
    private int enemiesRemaining = 0;

    void Start()
    {
       
        NextWave();
    }
    
    public void NextWave()
    {
        currentWave++;
        enemiesRemaining = CalculateEnemiesForWave(currentWave);

        waveSlider.maxValue = enemiesRemaining;
        waveSlider.value    = enemiesRemaining;

        if (waveText)
            waveText.text = $"Oleada {currentWave}/{totalWaves}";
    }
    
    public void OnEnemyKilled()
    {
        enemiesRemaining = Mathf.Max(0, enemiesRemaining - 1);
        waveSlider.value = enemiesRemaining;

        if (enemiesRemaining == 0)
        {
           
            if (currentWave < totalWaves)
                NextWave();
            else
                EndAllWaves();
        }
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

