using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChemicalSpill : MonoBehaviour
{
    public float timePlayerIsPoisoned;
    private int playerLayer;

    void Start(){
        playerLayer = LayerMask.NameToLayer("Player");
    }

    public void OnTriggerEnter2D(Collider2D collider){
        if (collider.gameObject.layer == playerLayer){
            Player player = collider.gameObject.GetComponent<Player>();
            player.Poison();
        }
    }

    public void OnTriggerExit2D(Collider2D collider){
        if (collider.gameObject.layer == playerLayer){
            Player player = collider.gameObject.GetComponent<Player>();
            player.Invoke("CurePoison", timePlayerIsPoisoned);
        }
    }
}
