using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackground : MonoBehaviour {
    [SerializeField] public GameObject cameraObj;
    float startPosition;
    [SerializeField] public float parallaxEffect = 0.3f; // Default parallax effect

    void Start () {
        startPosition = transform.position.y;
    }

    void FixedUpdate () {  
        // Calculate the distance with the camera position in the Y axis (vertical) multiplied by the parallax effect variable.
        float distance = (cameraObj.transform.position.y * parallaxEffect);

        // Change the position in the Y axis (vertical) of the current background gameObject (small, medium, big)
        transform.position = new Vector3 (transform.position.x, startPosition + distance, transform.position.z);
    }
}
