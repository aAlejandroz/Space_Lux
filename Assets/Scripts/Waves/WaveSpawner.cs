using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveSpawner : MonoBehaviour {

    public enum SpawnState { SPAWNING, WAITING, COUNTING };

    [System.Serializable]
	public class Wave {        
        public string name;
        public List<Transform> enemyList;   // Posiible enemies to spawn 
        public Transform curEnemy;        
        public int count;
        public float rate;
    }

    public Wave[] waves;
    private int nextWave;

    public Transform[] spawnPoints;

    //public int maxWaitTime; // Max wait time in seconds
    //public float timeBetweenWaves = 30f;
    System.Random rand = new System.Random();
    public float timeBetweenWaves = 30f;
    public float waveCountdown { get; private set; }

	public WaveTimerUI WaveTimerUI;

    private float searchCountdown = 1f;

    private SpawnState state = SpawnState.COUNTING;

    public SpawnState getState() {
        return state;
    }

    void Start() {
        //timeBetweenWaves = RandomNum(maxWaitTime); 
        nextWave = 0;
        waveCountdown = timeBetweenWaves;
		WaveTimerUI.StartCountdown(timeBetweenWaves);
    }

    void Update() {
        if (state == SpawnState.WAITING) {
            if (!EnemyIsAlive()) {
                //new round               
                WaveCompleted();
            } else {
                return; 
            }
        }

        if (waveCountdown <= 0f) {
            if (state == SpawnState.COUNTING) {
                StartCoroutine(SpawnWave(waves[nextWave]));
            }
        }
        else {         
            waveCountdown -= Time.deltaTime;
        }
    }

    bool EnemyIsAlive() { 
        searchCountdown -= Time.deltaTime;
        if(searchCountdown <= 0f) {
            searchCountdown = 1f;
            if (GameObject.FindGameObjectWithTag("Enemy") == null) {
                return false;
            }
        }   
        return true;
    }

    public int RandomNum(int maxSeconds) {
        return rand.Next(maxSeconds);
    }

    void WaveCompleted() {
        WaveTimerUI.StartCoroutine(WaveTimerUI.DisplayEndRound());
        state = SpawnState.COUNTING;
        waveCountdown = timeBetweenWaves;

        if (nextWave + 1 == waves.Length - 1) {            
            Debug.Log("All waves completed");
        }
        else {
            nextWave++;
        }        

		WaveTimerUI.StartCountdown(timeBetweenWaves);
    }

    // Pick a random enemy from list and spawn

    private IEnumerator SpawnWave(Wave _wave) {        
        state = SpawnState.SPAWNING;

        //spawn
        for (int i = 0; i < _wave.count; i++) {
            int enemyIndex = rand.Next(_wave.enemyList.Count);
            _wave.curEnemy = _wave.enemyList[enemyIndex];
            SpawnEnemy(_wave.curEnemy);
            yield return new WaitForSeconds(1 / _wave.rate);
        }

        state = SpawnState.WAITING;

        yield break;
    }

    void SpawnEnemy(Transform _enemy) {
        Transform _sp = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(_enemy, _sp.position, _sp.rotation);
    }

}
