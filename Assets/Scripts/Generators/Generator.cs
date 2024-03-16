using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public float spawnRegionWidth; // width of the region in which objects can spawn 
    public float timeBetweenSpawns; // time until next object spawns
    private float timeUntilNextSpawn = 0; // time until next object spawns
    public bool isSpawning = true; // whether or not objects are spawning
    public GameObject objectPrefab; // prefab for object

    void Update(){
        if (isSpawning && !GameManager.Instance.isGamePaused){ 
            timeUntilNextSpawn += Time.deltaTime;
            if (timeUntilNextSpawn >= timeBetweenSpawns){
                SpawnObject();
                timeUntilNextSpawn = 0;
            }
        }
    }

    public virtual void SpawnObject(){
        float randX = Random.Range(-spawnRegionWidth / 2, spawnRegionWidth / 2);
        Vector3 spawnPos = new Vector3(transform.position.x + randX, transform.position.y, transform.position.z);
        Instantiate(objectPrefab, spawnPos, Quaternion.identity);
    }
}
