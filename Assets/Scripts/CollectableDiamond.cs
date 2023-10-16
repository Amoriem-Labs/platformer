using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CollectableDiamond : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
         // Assuming you have a reference to the PlayerInventory script on your player object.
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();

            if (playerInventory != null)
            {
                playerInventory.DiamondCollected(); // Call the CollectDiamond method in the PlayerInventory script.
                gameObject.SetActive(false);
            }
        
    }
}
