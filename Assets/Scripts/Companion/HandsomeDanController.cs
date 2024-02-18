using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HandsomeDanController : MonoBehaviour
{
    public float speed = 10.0f;
    public float targetDetectionRange = 5.0f;
    private GameObject target = null;
    public int points = 30; // The number of points the player gets for collecting this item.
    private static SpriteRenderer handsomeDanSprite; // This is Handsome Dan's sprite renderer.

    // Start is called before the first frame update
    void Start()
    {
        handsomeDanSprite = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update(){
        if (target == null){
            FindNearestTarget();

            if (target == null){
                FollowPlayer();
            }
        }
        else{
            MoveTowardsTarget();
        }
    }

    void FindNearestTarget(){
        List<GameObject> targets = new List<GameObject>();

        GameObject[] coins = GameObject.FindGameObjectsWithTag("Coin");
        GameObject[] assignments = GameObject.FindGameObjectsWithTag("Assignment");

        targets.AddRange(coins);
        targets.AddRange(assignments);
        float nearestDistance = Mathf.Infinity;
        GameObject nearestTarget = null;
        
        foreach (GameObject potentialTarget in targets){
            float distance = Vector3.Distance(potentialTarget.transform.position, transform.position);
            if (distance < nearestDistance){
                nearestDistance = distance;
                nearestTarget = potentialTarget;
            }
        }
        
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        float distanceToPlayer = Vector3.Distance(playerTransform.position, nearestTarget.transform.position);
        if (distanceToPlayer < targetDetectionRange){
            target = nearestTarget;
        }
    }

    void MoveTowardsTarget(){
        float step = speed * Time.deltaTime;
        transform.position = Vector3.MoveTowards(transform.position, target.transform.position, step);

        if (target.transform.position.x < transform.position.x)
        {
            handsomeDanSprite.flipX = false;
        }
        else
        {
            handsomeDanSprite.flipX = true;
        }

        if (Vector3.Distance(transform.position, target.transform.position) < 0.001f){
            // Get player inventory
            PlayerInventory playerInventory = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerInventory>();
            if (playerInventory != null)
            {
                playerInventory.ItemCollected(target); // Call the ItemCollected method in the PlayerInventory script.
                target.SetActive(false);
                ScoreManager.Instance.AddPoints(points);
                target = null;
            }
        }
    }

    void FollowPlayer(){
        float step = speed * Time.deltaTime;
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Vector3 newPosition = new Vector3(player.transform.position.x - 2, player.transform.position.y, player.transform.position.z);
        transform.position = Vector3.MoveTowards(transform.position, newPosition, step);
    }
}
