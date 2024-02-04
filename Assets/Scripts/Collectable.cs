using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float moveBetweenFrames = 0.02f;
    public float timeBetweenFrames = 0.01f;

    [ContextMenu("Move")]
    void Start(){
        StartCoroutine(Move());
    }

    IEnumerator Move(){
        while (true){
            for (int i = 0; i < 10; i++){
                transform.position = new Vector3(transform.position.x, transform.position.y + moveBetweenFrames, transform.position.z);
                yield return new WaitForSeconds(timeBetweenFrames);
            }
            for (int i = 0; i < 10; i++){
                transform.position = new Vector3(transform.position.x, transform.position.y - moveBetweenFrames, transform.position.z);
                yield return new WaitForSeconds(timeBetweenFrames);
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
         // Assuming you have a reference to the PlayerInventory script on your player object.
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

            if (playerInventory != null)
            {
                playerInventory.ItemCollected(); // Call the ItemCollected method in the PlayerInventory script.
                gameObject.SetActive(false);
            }
        
    }
}
