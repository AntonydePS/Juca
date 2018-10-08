using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class HPSpawner : MonoBehaviour
{

    public Transform HPitem;

    public Transform spawnPoint1;
    public Transform spawnPoint2;
    public Transform spawnPoint3;

    public float timeBetweenWaves = 10f;
    private float countdown = 5f;

    public Text waveCountdownText;

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

    }

    IEnumerator SpawnWave()
    {
        {
            SpawnEnemy();
            yield return new WaitForSeconds(0.5f);
        }
    }

    void SpawnEnemy()
    {
        Instantiate(HPitem, spawnPoint1.position, spawnPoint1.rotation);
        Instantiate(HPitem, spawnPoint2.position, spawnPoint2.rotation);
        Instantiate(HPitem, spawnPoint3.position, spawnPoint3.rotation);
    }

}