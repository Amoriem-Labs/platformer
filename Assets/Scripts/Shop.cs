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
    private int[] itemsCost = {15, 0, 0, 0, 0, 0, 0, 0, 0, 0}; //array with cost of each item

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X)) {
            
            ShopPanel.gameObject.SetActive(false);
           
        }

        if (Input.GetKeyDown(KeyCode.Alpha0)) {
            if (CoinManager.Instance.numCoinsPlayerHas >= itemsCost[0]) {
                CoinManager.Instance.numCoinsPlayerHas -= itemsCost[0];
                success.Play();
                SleepManager.Instance.maxTime += 10;
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha1)) { 
            if (CoinManager.Instance.numCoinsPlayerHas >= itemsCost[1]) {
                CoinManager.Instance.numCoinsPlayerHas -= itemsCost[1];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha2)) { 
            if (CoinManager.Instance.numCoinsPlayerHas >= itemsCost[2]) {
                CoinManager.Instance.numCoinsPlayerHas -= itemsCost[2];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha3)) { 
            if (CoinManager.Instance.numCoinsPlayerHas >= itemsCost[3]) {
                CoinManager.Instance.numCoinsPlayerHas -= itemsCost[3];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha4)) { 
            if (CoinManager.Instance.numCoinsPlayerHas >= itemsCost[4]) {
                CoinManager.Instance.numCoinsPlayerHas -= itemsCost[4];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha5)) { 
            if (CoinManager.Instance.numCoinsPlayerHas >= itemsCost[5]) {
                CoinManager.Instance.numCoinsPlayerHas -= itemsCost[5];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha6)) { 
            if (CoinManager.Instance.numCoinsPlayerHas >= itemsCost[6]) {
                CoinManager.Instance.numCoinsPlayerHas -= itemsCost[6];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha7)) { 
            if (CoinManager.Instance.numCoinsPlayerHas >= itemsCost[7]) {
                CoinManager.Instance.numCoinsPlayerHas -= itemsCost[7];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha8)) { 
            if (CoinManager.Instance.numCoinsPlayerHas >= itemsCost[8]) {
                CoinManager.Instance.numCoinsPlayerHas -= itemsCost[8];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

        if (Input.GetKeyDown(KeyCode.Alpha9)) { 
            if (CoinManager.Instance.numCoinsPlayerHas >= itemsCost[9]) {
                CoinManager.Instance.numCoinsPlayerHas -= itemsCost[9];
                success.Play();
                //what the player is buying
            }
            else {
                fail.Play();
            }
        }

    }
}


