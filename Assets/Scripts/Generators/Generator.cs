using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Generator : MonoBehaviour
{
    public float spawnRegionWidth; // width of the region in which objects can spawn 
    public float timeBetweenSpawns; // time until next object spawns
    private float timeUntilNextSpawn = 0; // time until next object spawns
    public bool isSpawning = false; // whether or not objects are spawning
    public GameObject objectPrefab; // prefab for object

    void Update(){
        if (isSpawning){ 
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

    // Activates spawning only when camera enters render region
    void OnTriggerEnter2D(Collider2D collider){
        if (collider.tag == "VirtualCamera"){
            isSpawning = true;
        }
    }

    // Deactivates chainsaw spawning when camera leaves render region
    void OnTriggerExit2D(Collider2D collider){
        if (collider.tag == "VirtualCamera"){
            isSpawning = false;
        }
    }
}
