using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Consumable : InventoryItem
{
    public void Use(){
        Debug.Log("Consumable used");
    }
}
