using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerInventory : MonoBehaviour
{
    // This will store the number of collected diamonds.
    public int numItems
    {
        get;
        private set;
    }

    public UnityEvent<PlayerInventory> OnItemCollected;

   

    public void ItemCollected()
    {
        numItems++;
        OnItemCollected.Invoke(this);
    }






}

