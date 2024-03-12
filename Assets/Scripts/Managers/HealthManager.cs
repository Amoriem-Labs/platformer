using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HealthManager : MonoBehaviour
{
    // DEPRECATED SCRIPT: NO LONGER USING HEALTH SYSTEM
    /*
    private static HealthManager _instance;
    public static HealthManager Instance { get { return _instance;} }
    public GameObject heartPrefab;
    public Transform heartBarParent;
    public int totalNumHearts;
    public float currentNumHearts;
    public Sprite fullHeart;
    public Sprite halfHeart;
    public Sprite emptyHeart;
    public Vector2 worldCoordFirstHeart;
    public Vector2 distanceBetweenHearts;
    public List<GameObject> heartObjs;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        } else {
            _instance = this;
        }
    }

    void Start(){
        heartObjs = new List<GameObject>();
        for (int i = 0; i < totalNumHearts; i++){
            GameObject heart = Instantiate(heartPrefab, worldCoordFirstHeart + (i * distanceBetweenHearts), Quaternion.identity, heartBarParent);
            heartObjs.Add(heart);
        }
        currentNumHearts = totalNumHearts;
    }

    #region Unit Testing
    [ContextMenu("Add two hearts without new hearts")]
    public void AddTwoHeartsWithoutNewHearts(){
        AddHearts(2, false);
    }

    [ContextMenu("Remove one heart")]
    public void RemoveOneHeart(){
        ChangeHealth(1, false);
    }

    [ContextMenu("Remove one and a half hearts")]
    public void RemoveOneAndAHalfHearts(){
        ChangeHealth(1.5f, false);
    }

    [ContextMenu("Add one and a half heart")]
    public void AddOneAndAHalfHearts(){
        ChangeHealth(1.5f, true);
    }
    #endregion

    public void AddHearts(int numHeartsToAdd, bool addImmediately = false){
        for (int i = 0; i < numHeartsToAdd; i++){
            GameObject heart = Instantiate(heartPrefab, worldCoordFirstHeart + ((totalNumHearts + i) * distanceBetweenHearts) - new Vector2(0f, 0.795f), Quaternion.identity, heartBarParent);
            heartObjs.Add(heart);
        }
        totalNumHearts += numHeartsToAdd;
        if (addImmediately){
            ChangeHealth(numHeartsToAdd, true);
        } else {
            ChangeHealth(0, true);
        }
    }

    // Input numHearts to ChangeHealth method should be a multiple of a half heart.
    public void ChangeHealth(float numHearts, bool add){
        if (add){
            if (currentNumHearts + numHearts > totalNumHearts){
                currentNumHearts = totalNumHearts;
            } else {
                currentNumHearts += numHearts;
            }
        }
        else{
            if (currentNumHearts <= 0){
                Player.Faint();
            } else {
                currentNumHearts -= numHearts;
            }
        }
        for (int i = 0; i < System.Math.Floor(currentNumHearts); i++){
            heartObjs[i].GetComponent<Image>().sprite = fullHeart;
        }
        if (currentNumHearts % 1 == 0.5f){
            heartObjs[(int)System.Math.Floor(currentNumHearts)].GetComponent<Image>().sprite = halfHeart;
        }
        for (int i = (int)System.Math.Ceiling(currentNumHearts); i < totalNumHearts; i++){
            heartObjs[i].GetComponent<Image>().sprite = emptyHeart;
        }
    }*/
}
