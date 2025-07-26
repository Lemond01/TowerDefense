using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class VictoryTrigger : MonoBehaviour
{
    [SerializeField] private string victorySceneName = "Win";
    [SerializeField] private float delayBeforeWin = 30f;

    private bool isWaiting = false;

    void Update()
    {
        if (isWaiting) return;

        Enemy[] enemies = FindObjectsOfType<Enemy>();
        if (enemies.Length == 0)
        {
            isWaiting = true;
            StartCoroutine(WinAfterDelay());
        }
    }

    IEnumerator WinAfterDelay()
    {
        yield return new WaitForSeconds(delayBeforeWin);
        SceneManager.LoadScene(victorySceneName);
    }
}