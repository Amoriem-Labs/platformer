using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class CameraController : MonoBehaviour
{
    public Transform player;
    [HideInInspector] public CinemachineVirtualCamera vcam; // The vcam component
    public float movementSmoothSpeed = 10f; // The speed at which the camera follows the player
    public float lookDownOrUpSmoothSpeed = 5f; // The speed at which the camera looks down or up
    public LayerMask wallMask;  // Layers considered as obstacles
    public float y_offset = 0.5f; // The y offset from the player
    public float lookDownOffset = 1f; 
    public float lookUpOffset = 2.5f;
    public float timeThreshold; // The time the player needs to press the down or up arrow keys until the camera moves down or up
    private float timeSinceDownOrUpHeldDown = 0; // The time the player has pressed the down or up arrow keys
    public Vector3 positionBeforeHittingWall;

    void Start(){
        vcam = GetComponent<CinemachineVirtualCamera>();
    }

    // Perhaps clean this code up later
    void FixedUpdate()
    {
        float vcamOrthoSize = vcam.m_Lens.OrthographicSize;
        Vector3 desiredPosition;
        Vector3 smoothedPosition;

        // If player hits wall
        if (Physics2D.Raycast(player.position, Vector2.left, vcamOrthoSize, wallMask) || Physics2D.Raycast(player.position, Vector2.right, vcamOrthoSize, wallMask))
        {
            
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
                if (IsAbleToLookUpOrDown()){ 
                    desiredPosition = new Vector3(positionBeforeHittingWall.x, positionBeforeHittingWall.y - y_offset - lookDownOffset, -10); 
                } else {
                    desiredPosition = new Vector3(positionBeforeHittingWall.x, positionBeforeHittingWall.y + y_offset, -10);
                }
            }
            else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){ 
                if (IsAbleToLookUpOrDown()){
                    desiredPosition = new Vector3(positionBeforeHittingWall.x, positionBeforeHittingWall.y + y_offset + lookUpOffset, -10); 
                } else {
                    desiredPosition = new Vector3(positionBeforeHittingWall.x, positionBeforeHittingWall.y + y_offset, -10);
                }
            }
            else { 
                timeSinceDownOrUpHeldDown = 0;
                desiredPosition = new Vector3(positionBeforeHittingWall.x, player.position.y + y_offset, -10); 
            }
        }
        else // If player doesn't hit a wall
        {
            if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
                if (IsAbleToLookUpOrDown()){
                    desiredPosition = new Vector3(player.position.x, player.position.y - y_offset - lookDownOffset, -10);
                } else {
                    desiredPosition = new Vector3(player.position.x, player.position.y + y_offset, -10);
                }
            } else if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
                if (IsAbleToLookUpOrDown()){
                    desiredPosition = new Vector3(player.position.x, player.position.y + y_offset + lookUpOffset, -10);
                } else {
                    desiredPosition = new Vector3(player.position.x, player.position.y + y_offset, -10);
                }
            } else {
                timeSinceDownOrUpHeldDown = 0;
                desiredPosition = player.position + new Vector3(0, y_offset, -10);
            }
            positionBeforeHittingWall = new Vector3(player.position.x, player.position.y, -10);
        }
        smoothedPosition = Vector3.Lerp(transform.position, desiredPosition, lookDownOrUpSmoothSpeed * Time.deltaTime);
        vcam.transform.position = smoothedPosition;
    }

    bool IsAbleToLookUpOrDown(){
        timeSinceDownOrUpHeldDown += Time.deltaTime;
        if (timeSinceDownOrUpHeldDown >= timeThreshold){
            return true;
        }
        return false;
    }
}
