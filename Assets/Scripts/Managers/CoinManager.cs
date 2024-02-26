using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CoinManager : MonoBehaviour
{
    private static CoinManager _instance;
	public static CoinManager Instance { get { return _instance; } }
    public int numCoinsPlayerHas; // This will store the number of collected coins the player has in their inventory.
    public int numCoinsCollectedInLevel; // This will store the number of coins collected in specifically just this level, to be used in the LevelGradingManager.
    public int numCoinsInLevel; // This will store the number of coins in the level, to be used in the LevelGradingManager.
    public TextMeshProUGUI coinText; // This is the text that will be displayed in the UI to show the number of coins the player has.

    void Awake() {
        if (_instance != null && _instance != this) {
            Destroy(this.gameObject);
        }
        else {
            _instance = this;
            DontDestroyOnLoad(_instance);

            numCoinsPlayerHas = 0;
            numCoinsCollectedInLevel = 0;
        }
    }

    void Start(){
        numCoinsInLevel = GameObject.FindGameObjectsWithTag("Coin").Length;
    }

    public void AddCoin(){
        numCoinsPlayerHas++;
        numCoinsCollectedInLevel++;
        UpdateCoinText();
    }

    public void UpdateCoinText(){
        coinText.text = numCoinsPlayerHas.ToString();
    }

    public void ResetNumCoinsCollectedInLevel(){
        numCoinsCollectedInLevel = 0;
    }
}
