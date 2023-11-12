using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    public CinemachineVirtualCamera vcam; // The vcam component
    public float smoothTime = 0.3f;
    public LayerMask wallMask;  // Layers considered as obstacles
    public LayerMask ceilingMask; // Layers considered as ceiling

    void Start(){
        vcam = GetComponent<CinemachineVirtualCamera>();
        vcam.LookAt = player;
        vcam.Follow = player;
    }

    void FixedUpdate()
    {
        float vcamOrthoSize = vcam.m_Lens.OrthographicSize;

        // Use raycasting to check for obstacles
        if (Physics2D.Raycast(player.position, Vector2.left, vcamOrthoSize, wallMask) || Physics2D.Raycast(player.position, Vector2.right, vcamOrthoSize, wallMask))
        {
            // Adjust the camera position to be just before the obstacle
            vcam.LookAt = null;
            vcam.Follow = null;
            // Continue following y position of player, unless player hits ceiling
            if (Physics2D.Raycast(player.position, Vector2.up, vcamOrthoSize, ceilingMask)){
                vcam.transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
            }
            else {
                vcam.transform.position = new Vector3(transform.position.x, player.transform.position.y, transform.position.z);
            }
        }
        else
        {
            // Smoothly follow the target if no obstacles are in the way
            vcam.LookAt = player;
            vcam.Follow = player;
        }
    }
}
