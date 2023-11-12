using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfiniteBackground : MonoBehaviour {
    [SerializeField] public GameObject cameraObj;
    float startPositionX;
    float startPositionY;
    [SerializeField] public float parallaxEffectX = 0.3f; // Default parallax effect
    [SerializeField] public float parallaxEffectY = 0.3f; // Default parallax effect

    void Start () {
        startPositionX = transform.position.x;
        startPositionY = transform.position.y;
    }

    void FixedUpdate () {  
        // Calculate the distance with the camera position in the X axis (horizontal) multiplied by the parallax effect Y variable.
        float distanceX = (cameraObj.transform.position.x * parallaxEffectX);
        // Calculate the distance with the camera position in the Y axis (vertical) multiplied by the parallax effect X variable.
        float distanceY = (cameraObj.transform.position.y * parallaxEffectY);

        // Change the position of the current background gameObject
        transform.position = new Vector3 (startPositionX + distanceX, startPositionY + distanceY, transform.position.z);
    }
}
