using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using TMPro;


public class WaveManager : MonoBehaviour
{
    private Rage rageScript;
    public TextMeshProUGUI waveText;
    public TextMeshProUGUI currentWaveText;
    public TextMeshProUGUI lvlTitle;
    public int currentLvl;
    public int totalEnemiesLeft;
    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
    public class Wave
    {
        public string name;
        public List<GameObject> enemies;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave = 0;
    public int NextWave
    {
        get { return nextWave + 1; }
    }

    public Transform spawner;

    public float timeBetweenWaves = 5f;
    private float waveCountdown;
    public float WaveCountdown
    {
        get { return waveCountdown; }
    }

    private float searchCountdown = 1f;
    private SpawnState state = SpawnState.COUNTING;
    public SpawnState State
    {
        get { return state; }
    }

    void Start()
    {
        lvlTitle.text = SceneManager.GetActiveScene().name;
        waveCountdown = timeBetweenWaves;
        rageScript = GetComponent<Rage>();
        currentLvl = SceneManager.GetActiveScene().buildIndex;
        //listCount = enemies.Count;
    }

    void Update()
    {
        if (state == SpawnState.WAITING)
        {
            if (!EnemyIsAlive())
            {
                WaveCompleted();
            }
            else
            {
                return;
            }
        }

        if (waveCountdown <= 0)
        {
            if (state != SpawnState.SPAWNING)
            {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else
        {
            waveCountdown -= Time.deltaTime;
        }
                totalEnemiesLeft = GameObject.FindGameObjectsWithTag("Enemy").Length;
        if (totalEnemiesLeft < 1)
        {
            //WIN
            StartCoroutine(WaitForIt(1F + timeBetweenWaves));
        }
    }

    void WaveCompleted()
    {
        Debug.Log("Wave Completed!");
        

        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 > waves.Length - 1)
        {
            nextWave = 0;
            Debug.Log("ALL WAVES COMPLETE! Looping...");
        }
        else
        {
            nextWave++;
        }
    }

    bool EnemyIsAlive()
    {
        searchCountdown -= Time.deltaTime;
        if (searchCountdown <= 0f)
        {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null)
            {
                return false;
            }
        }
        return true;
    }

    IEnumerator SpawnWave(Wave _wave)
    {
        int count = _wave.enemies.Count;
        Debug.Log("Spawning Wave: " + _wave.name);
        rageScript.ResetRageCounter();
        waveText.text = _wave.name;

        state = SpawnState.SPAWNING;
        
        for (int i = 0; i < count; i++)
        {
            SpawnEnemy(_wave.enemies[0]);
            currentWaveText.text = "Wave " + (nextWave + 1) + " / " + waves.Length;
            _wave.enemies.Remove(_wave.enemies[0]);
            yield return new WaitForSeconds(1f / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(GameObject _enemies)
    {
        //Debug.Log("Spawning Enemy: " + _enemies.name);
        Instantiate(_enemies, spawner.transform.position, spawner.transform.rotation, transform);

        //if (listCount > 0)
        //{
        //    GameObject newEnemy = Instantiate(_wave.enemies[0], transform.position, transform.rotation) as GameObject;

        //    listCount -= 1;
        //}
    }
    IEnumerator WaitForIt(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        if (totalEnemiesLeft < 1)
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex); // +1
        }

    }


}
