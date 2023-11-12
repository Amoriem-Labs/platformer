using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    public float timeBeforeDisappearing = 5f; // 5 seconds is default value for projectile to last in game before disappearing

    void Start()
    {
        Invoke("DestroyProjectile", timeBeforeDisappearing);
    }

    void DestroyProjectile(){
        Destroy(gameObject);
    }
}
