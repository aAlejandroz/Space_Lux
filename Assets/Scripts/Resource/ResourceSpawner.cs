using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceSpawner : MonoBehaviour {

    public float timeBetweenRespawn = 25;
    public float countdown;
    public float dropRate = 0.65f;
    public bool isSpawningItems;    
    public List<GameObject> resources;
    public List<Transform> spawnPoints;
    public System.Random rand;

	// Use this for initialization
	void Start () {
        isSpawningItems = true;
        countdown = timeBetweenRespawn;
        rand = new System.Random();
        SpawnItems();
	}
	
	// Update is called once per frame
	void Update () {
		if (isSpawningItems) {
            SpawnItems();
        } else {
            countdown -= Time.deltaTime;
        }

        if (countdown <= 0f) {
            isSpawningItems = true;
            countdown = timeBetweenRespawn;
        }
	}

    public void SpawnItems() {
        // Choose rand num to determine how many spawnPoints to spawn items from
        int randResource = rand.Next(resources.Count);   // Random resource       

        for (int i = 0; i < spawnPoints.Count; i++) {
            if (Random.Range(0f, 1f) <= dropRate) {               
                Instantiate(resources[randResource], spawnPoints[i].position, Quaternion.identity);
                randResource = rand.Next(resources.Count);
            }
        }

        isSpawningItems = false;
    }

}
