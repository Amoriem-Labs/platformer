using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Collectable : MonoBehaviour
{
    public float moveBetweenFrames = 0.02f;
    public int points = 30;

    [Header("Exit Animation")]
    public float scaleDownPercentPerFrame = .07f;
    public float moveUpPerFrame = 0.05f;


    Collider2D collider;
    Animator animator;
    Coroutine moveCoroutine;
    float scaleDownPerFrame;

    [ContextMenu("Move")]
    void Start(){
        moveCoroutine = StartCoroutine(Move());
        collider = GetComponent<Collider2D>();
        // Larger objects should scale down at the same rate as smaller objects
        scaleDownPerFrame = scaleDownPercentPerFrame * transform.localScale.x;
        print(scaleDownPerFrame);
    }

    IEnumerator Move(){
        while (true){
            for (int i = 0; i < 10; i++){
                transform.position = new Vector3(transform.position.x, transform.position.y + moveBetweenFrames, transform.position.z);
                yield return new WaitForEndOfFrame();
            }
            for (int i = 0; i < 10; i++){
                transform.position = new Vector3(transform.position.x, transform.position.y - moveBetweenFrames, transform.position.z);
                yield return new WaitForEndOfFrame();
            }
        }
    }

    IEnumerator AnimateOut()
    {
        while (transform.localScale.x > 0)
        {
            transform.localScale -= new Vector3(scaleDownPerFrame, scaleDownPerFrame, 0);
            transform.position += moveUpPerFrame * Vector3.up;
            yield return new WaitForEndOfFrame();
        }
        gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")){
            PlayerInventory playerInventory = other.GetComponent<PlayerInventory>();
            if (playerInventory == null) return;

            collider.enabled = false;
            playerInventory.ItemCollected(this.gameObject);
            ScoreManager.Instance.AddPoints(points);
            StopCoroutine(moveCoroutine);
            StartCoroutine(AnimateOut());
        }
    }
}
