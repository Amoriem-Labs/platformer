using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class PlayerInventory : MonoBehaviour
{
    // This will store the number of collected diamonds.
    public int NumberOfDiamonds
    {
        get;
        private set;
    }

    public UnityEvent<PlayerInventory> OnDiamondCollected;

   

    public void DiamondCollected()
    {
        NumberOfDiamonds++;
        OnDiamondCollected.Invoke(this);
    }






}

