using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class WaveSpawner : MonoBehaviour
{
    public GameManager gameManager;
    public NavMeshManager navmeshManager;

    [Header("EnemySpawn")]
    public static int EnemiesAlive = 0;
    public Wave[] waves;
    public Transform EnemyPrefab;
    public Transform SpawnPoint;

    [Header("ReadyTime")]
    public float countdown = 1f;

    [Header("TopCanvas")]
    public TMP_Text WaveText;
    public TMP_Text NextWaveCountText;

    private int waveIndex = 0;

    void Update()
    {
        if (EnemiesAlive > 0)
        {
            return;
        }


        if (countdown <= 0f)
        {
            if (waveIndex == waves.Length)
            {
                gameManager.WinLevel();
                StopCoroutine(SpawnWave());
                this.enabled = false;
            }
            else
            {
                StartCoroutine(SpawnWave());
                navmeshManager.UpdateBuildNavmesh();
                return;
            }
        }

        if (EnemiesAlive <= 0)
        {
            UpdatePreparationTimer();
        }
       
    }

    void UpdatePreparationTimer()
    {
        countdown -= Time.deltaTime;
        countdown = Mathf.Clamp(countdown, 0f, Mathf.Infinity);

        NextWaveCountText.text = "PreparationTime:" + countdown.ToString("0");
    }

    IEnumerator SpawnWave()
    {
        PlayerStats.Rounds++;

        //計算當前波數
        WaveText.text = "Wave:" + PlayerStats.Rounds.ToString();

        Wave wave = waves[waveIndex];
        
        EnemiesAlive = wave.count;

        for (int i = 0; i < wave.count; i++)
        {
            SpawnEnemy(wave.enemy);
            yield return new WaitForSeconds(1f / wave.rate);
        }
        waveIndex++;
    }

    void SpawnEnemy(GameObject enemy)
    {
        //adjust spawn localPosition
        Vector3 adjustPos = new Vector3(SpawnPoint.transform.localPosition.x, 1.5f, SpawnPoint.transform.localPosition.z);

        Instantiate(enemy, adjustPos, SpawnPoint.rotation);
    }
}
