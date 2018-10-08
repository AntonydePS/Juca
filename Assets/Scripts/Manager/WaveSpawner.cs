using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class WaveSpawner : MonoBehaviour
{

    public Transform enemyPrefab;
    public Transform spawnPointLeft;
    public Transform spawnPointRight;
    public Transform spawnPointMid;

    public float timeBetweenWaves = 5f;
    private float countdown = 2f;

    public Text waveCountdownText;
    public Text waveAmountText;

    private int waveIndex = 0;

    void Update()
    {
        if (countdown <= 0f)
        {
            StartCoroutine(SpawnWave());
            countdown = timeBetweenWaves;
        }

        countdown -= Time.deltaTime;

        waveCountdownText.text = Mathf.Round(countdown).ToString();
        waveAmountText.text = Mathf.Round(waveIndex).ToString();

        if (waveIndex >= 4)
        {
            GameObject GC = GameObject.FindGameObjectWithTag("GameController");
            GC.SendMessage("YoureWinner");
        }

    }

IEnumerator SpawnWave()
    {
        waveIndex++;

        for (int i = 0; i < waveIndex; i++)
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        Instantiate(enemyPrefab, spawnPointLeft.position, spawnPointLeft.rotation);
        Instantiate(enemyPrefab, spawnPointRight.position, spawnPointRight.rotation);
        Instantiate(enemyPrefab, spawnPointMid.position, spawnPointMid.rotation);
    }

}