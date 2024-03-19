using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collision : MonoBehaviour
{
    public LayerMask solidLayer;

    public bool onGround;
    public bool onWall;

    public bool touchLeft;
    public bool touchRight;
    public bool squish;
    public enum WallSide { None, Right, Left };
    public WallSide wallSide;

    public float collisionRadius = .25f;
    public Vector2 bottomOffset, ltOffset, lbOffset, rtOffset, rbOffset;

    void Update()
    {
        onGround = Physics2D.OverlapCircle((Vector2)transform.position + bottomOffset, collisionRadius, solidLayer);

        touchLeft = Physics2D.OverlapCircle((Vector2)transform.position + lbOffset, collisionRadius, solidLayer) ||
            Physics2D.OverlapCircle((Vector2)transform.position + ltOffset, collisionRadius, solidLayer);
        touchRight = Physics2D.OverlapCircle((Vector2)transform.position + rbOffset, collisionRadius, solidLayer) ||
            Physics2D.OverlapCircle((Vector2)transform.position + rtOffset, collisionRadius, solidLayer);

        //Debug.Log

        onWall = touchLeft || touchRight;

        squish = touchLeft && touchRight;
        wallSide = touchLeft ? WallSide.Left : touchRight ? WallSide.Right : WallSide.None;
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;

        var positions = new Vector2[] { bottomOffset, lbOffset, ltOffset, rbOffset, rtOffset };

        Gizmos.DrawWireSphere((Vector2)transform.position + bottomOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + lbOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + ltOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rbOffset, collisionRadius);
        Gizmos.DrawWireSphere((Vector2)transform.position + rtOffset, collisionRadius);
    }
}
