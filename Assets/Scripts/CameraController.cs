using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    [HideInInspector] public CinemachineVirtualCamera vcam; // The vcam component
    public float smoothSpeed = 10f; // The speed at which the camera follows the player
    public LayerMask wallMask;  // Layers considered as obstacles
    public LayerMask ceilingMask; // Layers considered as ceiling
    public float y_offset = 0.5f; // The y offset from the player

    void Start(){
        vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.transform.LookAt(player);
    }

    void FixedUpdate()
    {
        float vcamOrthoSize = vcam.m_Lens.OrthographicSize;

        // Use raycasting to check for obstacles
        if (Physics2D.Raycast(player.position, Vector2.left, vcamOrthoSize, wallMask) || Physics2D.Raycast(player.position, Vector2.right, vcamOrthoSize, wallMask))
        {
            Vector3 smoothedPosition;
            // Continue following y position of player, unless player hits ceiling
            if (Physics2D.Raycast(player.position, Vector2.up, vcamOrthoSize, ceilingMask)){
                Vector3 desiredPosition = new Vector3(transform.position.x, transform.position.y, transform.position.z);
                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            }
            else {
                Vector3 desiredPosition = new Vector3(transform.position.x, player.transform.position.y + y_offset, transform.position.z);
                smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            }
            vcam.transform.position = smoothedPosition;
        }
        else
        {
            // Smoothly follow the target if no obstacles are in the way
            Vector3 desiredPosition = player.position + new Vector3(0, y_offset, -10);
            Vector3 smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, smoothSpeed * Time.deltaTime);
            vcam.transform.position = smoothedPosition;
            
        }
    }
}
