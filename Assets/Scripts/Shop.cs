using System.Collections;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;


/* HOW TO ADD SOMETHING TO THE SHOP:
1) Decide which of the 10 spots your item will take
2) Put the cost of the item at that index in the itemCost array
3) Put the item itself where the "what the player is buying" comment is 
   in the if statement of that spot
4) Add the item's name, cost, and image to the shop panel
*/



public class Shop : MonoBehaviour
{
   
    public Transform ShopPanel;
    public AudioSource success;
    public AudioSource fail;
    public int numCoins; //drag and drop the numCoins game object into this variable slot
    private int[] itemsCost = {15, 0, 0, 0, 0, 0, 0, 0, 0, 0}; //array with cost of each item
    public float sleepTimer; //item 0: increase sleepTimer by 10

    


    void Start()
    {
      ShopPanel.gameObject.SetActive(true);
      numCoins = 100;
      sleepTimer = 0;
        
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) {
            
            ShopPanel.gameObject.SetActive(false);
           
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            if (numCoins >= itemsCost[0]) {
                numCoins -= itemsCost[0];
                success.Play();
                sleepTimer += 10;
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { 
            if (numCoins >= itemsCost[1]) {
                numCoins -= itemsCost[1];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) { 
            if (numCoins >= itemsCost[2]) {
                numCoins -= itemsCost[2];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) { 
            if (numCoins >= itemsCost[3]) {
                numCoins -= itemsCost[3];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) { 
            if (numCoins >= itemsCost[4]) {
                numCoins -= itemsCost[4];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) { 
            if (numCoins >= itemsCost[5]) {
                numCoins -= itemsCost[5];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha6)) { 
            if (numCoins >= itemsCost[6]) {
                numCoins -= itemsCost[6];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha7)) { 
            if (numCoins >= itemsCost[7]) {
                numCoins -= itemsCost[7];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha8)) { 
            if (numCoins >= itemsCost[8]) {
                numCoins -= itemsCost[8];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha9)) { 
            if (numCoins >= itemsCost[9]) {
                numCoins -= itemsCost[9];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

    }
}


