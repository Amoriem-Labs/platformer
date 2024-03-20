using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Collision collision;
    private BoxCollider2D playerColl; 

    public Rigidbody2D rb;

    private List<KeyCode> sprintKeys = new List<KeyCode> { KeyCode.Z };
    private List<KeyCode> dashKeys = new List<KeyCode> { KeyCode.X };
    private List<KeyCode> jumpKeys = new List<KeyCode> { KeyCode.Space, KeyCode.UpArrow, KeyCode.W };
    private List<KeyCode> crouchKeys = new List<KeyCode> { KeyCode.S, KeyCode.DownArrow };

    public bool checkKeyListPressed(List<KeyCode> keyList) //PUT THIS IN SOME OTHER UTIL SCRIPT
    {
        foreach (KeyCode key in keyList)
        {
            if (Input.GetKeyDown(key))
            {
                return true;
            }
        }
        return false;
    }
    public bool checkKeyListHeld(List<KeyCode> keyList) //PUT THIS IN SOME OTHER UTIL SCRIPT
    {
        foreach (KeyCode key in keyList)
        {
            if (Input.GetKey(key))
            {
                return true;
            }
        }
        return false;
    }


    public enum MovementState
    {
        NORMAL,
        AIR,
        DASH,
        SPRINT,
        REVERSE_SPRINT,
        AIR_SPRINT,
        BONK,
        TUMBLE,
        CROUCH,
        WALL_SLIP,
        WALL_CLIMB,
        WALL_JUMP
    }

    public enum Dir
    {
        LEFT,
        RIGHT,
        NONE
    }

    public MovementState movementState;
    public Dir faceDir = Dir.NONE;

    [Space]
    [Header("Info")]
    public float currSpeed = 0f;
    public float vertForce = 0f;

    [Space]
    [Header("Stats")]
    public float maxSpeed = 10f;
    public float maxSprintSpeed = 17f;
    public float minSprintSpeed = 5f;
    public float beserkThreshold = 13f;
    public float jumpForce = 50f;
    public float slideSpeed = 5f;
    public float climbSpeed = 10f;
    public float wallJumpLerp = 10f;
    public float dashSpeed = 20f;
    public float passiveDecelSpeed = 15f;
    public float reverseDecelSpeed = 20f;
    public float accelerationSpeed = 5f;
    public float beserkAcceleration = 2f;
    public float minSpeed = 4f;
    public float crouchSpeed = 4f;

    [Space]

    public float fallMultiplier = 2.5f;
    public float lowJumpMultiplier = 2f;

    [Space]

    public float dashTime = .3f;
    public float wallJumpTime = .2f;
    public float sprintGraceTime = .5f;

    [Space]

    public float normalPlayerHeight = 2f;
    public float crouchPlayerHeight = 1f;

    [Space]

    public int maxNumExtraJumps = 1;
    public int maxNumDashes = 1;
    private int numJumpsLeft = 1;
    private int numDashesLeft = 1;

    [Space]
    [Header("Booleans")]
    public bool canMove;
    //public bool wallGrab;
    //public bool wallJumped;
    //public bool wallSlide;
    //public bool isDashing;
    //public bool shouldJumpPhysics;

    //private bool inAir;
    //private bool hasDashed;
    private float dashCooldown = .7f;
    private float dashCooldownTimer = 0f;
    private float dashTimer;
    private float wallJumpTimer;
    private float sprintGraceTimer;
    private bool isSprintGrace;
    private bool graceOver;

    // Sprite variables
    public SpriteRenderer bodySprite;
    public SpriteRenderer sweaterSprite;
    public SpriteRenderer hairSprite;

    // Trail renderer variable
    private TrailRenderer tr;

    private void Awake()
    {
        if (!rb)
        {
            rb = GetComponent<Rigidbody2D>();
        }
        rb.gravityScale = 3;
        if (!collision)
        {
            collision = GetComponent<Collision>();
        }
        if (!playerColl)
        {
            playerColl = GetComponent<BoxCollider2D>();
        }
    }

    private void Update()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float y = Input.GetAxisRaw("Vertical");
        Vector2 dir = new Vector2(x, y);

        //PhysicsTick();

        dashCooldownTimer -= Time.deltaTime;

        if (dashCooldownTimer <= 0)
        {
            dashCooldownTimer = 0;
        }

        if (movementState == MovementState.CROUCH || movementState == MovementState.TUMBLE)
        {
            if (playerColl.size.y != 1)
            {
                playerColl.size = new Vector2(playerColl.size.x, 1);
                playerColl.offset = new Vector2(0, -0.5f);
            }
        }
        else
        {
            if (playerColl.size.y != 2)
            {
                playerColl.size = new Vector2(playerColl.size.x, 2);
                playerColl.offset = new Vector2(0, 0);
            }
        }

        if (movementState != MovementState.AIR && movementState != MovementState.AIR_SPRINT && movementState != MovementState.DASH)
        {
            numJumpsLeft = maxNumExtraJumps;
            numDashesLeft = maxNumDashes;
        }

        int horizMult = faceDir == Dir.LEFT ? -1 : (faceDir == Dir.RIGHT ? 1 : 0);

        Dir startFaceDir;

        switch (movementState)
        {
            case MovementState.NORMAL:
                CalcMovementAcceleration(dir);
                Move();
                if (CheckJump())
                {
                    Jump(Vector2.up);
                }

                if (checkKeyListHeld(crouchKeys))
                {
                    movementState = MovementState.CROUCH;
                }

                if (currSpeed > minSprintSpeed)
                {
                    if (checkKeyListHeld(sprintKeys))
                    {
                        movementState = MovementState.SPRINT;
                        break;
                    }
                }

                if (checkKeyListPressed(dashKeys) && dashCooldownTimer <= 0)
                {
                    dashCooldownTimer = dashCooldown;
                    Dash(new Vector2(horizMult, 0));
                    movementState = MovementState.DASH;
                    break;
                }

                if (!collision.onGround)
                {
                    movementState = MovementState.AIR;
                    break;
                }
                break;
            case MovementState.AIR:
                CalcMovementAcceleration(dir);
                Move();

                if (checkKeyListPressed(dashKeys) && dashCooldownTimer <= 0 && numDashesLeft > 0)
                {
                    dashCooldownTimer = dashCooldown;
                    Dash(new Vector2(horizMult, 0));
                    movementState = MovementState.DASH;
                    break;
                }

                if (collision.onGround)
                {
                    movementState = MovementState.NORMAL;
                    break;
                }
                if (!collision.onGround && collision.onWall)
                {
                    if ((dir.x > 0 && collision.touchRight) || (dir.x < 0 && collision.touchLeft))
                    {
                        movementState = MovementState.WALL_CLIMB;
                    }
                    else
                    {
                        movementState = MovementState.WALL_SLIP;
                    }
                    break;
                }
                break;
            case MovementState.DASH:
                dashTimer -= Time.deltaTime;
                if (dashTimer <= 0)
                {
                    rb.gravityScale = 3;
                    if (!collision.onGround)
                    {
                        movementState = MovementState.AIR;
                    }
                    else
                    {
                        movementState = MovementState.NORMAL;
                    } // ADD SPRINT/AIRSPRINT WHEN STILL MOVING
                    break;
                }

                if (!collision.onGround && collision.onWall)
                {
                    if ((dir.x > 0 && collision.touchRight) || (dir.x < 0 && collision.touchLeft))
                    {
                        rb.gravityScale = 3;
                        movementState = MovementState.WALL_CLIMB;
                    }
                    else
                    {
                        rb.gravityScale = 3;
                        movementState = MovementState.WALL_SLIP; //REPLACE WITH BONK
                    }
                    break;
                }
                break;
            case MovementState.SPRINT:

                startFaceDir = faceDir;

                CalcSprintAcceleration(dir);

                if (faceDir != startFaceDir)
                {
                    movementState = MovementState.REVERSE_SPRINT;
                    break;
                }

                Move();

                if (checkKeyListHeld(crouchKeys))
                {
                    movementState = MovementState.TUMBLE;
                }
                if (CheckJump())
                {
                    Jump(Vector2.up); // implement sprint_jump?
                }

                if (currSpeed < minSprintSpeed || !checkKeyListHeld(sprintKeys))
                {
                    movementState = MovementState.NORMAL;
                    break;
                }

                if (!collision.onGround)
                {
                    movementState = MovementState.AIR_SPRINT;
                    break;
                }

                break;
            case MovementState.REVERSE_SPRINT:
                if (dir.x == horizMult && checkKeyListHeld(sprintKeys))
                {
                    rb.velocity += new Vector2(horizMult, 0) * Time.deltaTime * reverseDecelSpeed;
                    if (rb.velocity.x * horizMult * -1 < 0)
                    {
                        movementState = MovementState.SPRINT;
                        break;
                    }
                }
                else
                {
                    movementState = MovementState.NORMAL;
                    break;
                }
                break;
            case MovementState.AIR_SPRINT:

                startFaceDir = faceDir;

                CalcSprintAcceleration(dir);

                if (faceDir != startFaceDir)
                {
                    movementState = MovementState.AIR;
                    break;
                }

                if (collision.onGround)
                {
                    movementState = MovementState.SPRINT;
                    break;
                }
                if (!collision.onGround && collision.onWall)
                {
                    if ((dir.x > 0 && collision.touchRight) || (dir.x < 0 && collision.touchLeft))
                    {
                        movementState = MovementState.WALL_CLIMB;
                    }
                    else
                    {
                        movementState = MovementState.WALL_SLIP;
                    }
                    break;
                }
                break;
            case MovementState.BONK:
                break;
            case MovementState.TUMBLE:

                Move();

                if (!checkKeyListHeld(crouchKeys) && CheckCanUncrouch())
                {
                    movementState = MovementState.SPRINT;
                }

                if (!collision.onGround)
                {
                    movementState = MovementState.AIR_SPRINT;
                    break;
                }

                break;
            case MovementState.CROUCH:
                CalcCrouchMovement(dir);
                Move();
                
                if (!checkKeyListHeld(crouchKeys) && CheckCanUncrouch())
                {
                    movementState = MovementState.NORMAL;
                }

                if (!collision.onGround)
                {
                    movementState = MovementState.AIR;
                    break;
                }
                break;
            case MovementState.WALL_SLIP:
                currSpeed = 0;
                WallSlide(dir);
                if (CheckJump())
                {
                    WallJump();
                    movementState = MovementState.WALL_JUMP;
                    wallJumpTimer = wallJumpTime;
                    break;
                }

                if (collision.onGround)
                {
                    movementState = MovementState.NORMAL;
                    break;
                }
                if (!collision.onGround && !collision.onWall)
                {
                    movementState = MovementState.AIR;
                    break;
                }
                if (collision.onWall && ((dir.x > 0 && collision.touchRight) || (dir.x < 0 && collision.touchLeft)))
                {
                    movementState = MovementState.WALL_CLIMB;
                    break;
                }
                break;
            case MovementState.WALL_JUMP:
                wallJumpTimer -= Time.deltaTime;

                if (collision.onGround)
                {
                    movementState = MovementState.NORMAL;
                    break;
                }

                if (wallJumpTimer < 0)
                {
                    if (!collision.onGround && collision.onWall)
                    {
                        if ((dir.x > 0 && collision.touchRight) || (dir.x < 0 && collision.touchLeft))
                        {
                            movementState = MovementState.WALL_CLIMB;
                        }
                        else
                        {
                            movementState = MovementState.WALL_SLIP;
                        }
                        break;
                    }
                    if (!collision.onGround)
                    {
                        movementState = MovementState.AIR;
                        break;
                    }
                }
                break;
            case MovementState.WALL_CLIMB:
                if ((dir.x < 0 && collision.touchLeft) || (dir.x > 0 && collision.touchRight))
                {
                    rb.velocity = new Vector2(0, climbSpeed);
                    if (!collision.onWall)
                    {
                        faceDir = dir.x < 0 ? Dir.LEFT : Dir.RIGHT;
                        rb.velocity = new Vector2(0, 0);
                        movementState = MovementState.NORMAL;
                    }
                }
                else if (dir.x == 0)
                {
                    rb.velocity = new Vector2(0, 0);
                    movementState = MovementState.WALL_SLIP;
                    break;
                }
                else
                {
                    faceDir = dir.x < 0 ? Dir.LEFT : Dir.RIGHT;
                    rb.velocity = new Vector2(0, 0);
                    movementState = MovementState.NORMAL;
                    break;
                }
                if (CheckJump())
                {
                    WallJump();
                    movementState = MovementState.WALL_JUMP;
                    wallJumpTimer = wallJumpTime;
                    break;
                }
                break;
            default:
                break;
        }

        //if (isDashing)
        //{
        //    dashTimer -= Time.deltaTime;
        //    if (dashTime < 0)
        //    {
        //        rb.gravityScale = 3;
        //        shouldJumpPhysics = true;
        //        wallJumped = false;
        //        isDashing = false;
        //    }
        //}

        // Decide stuff with the keys and buttons and allat

        //if (collision.onWall && canMove && Input.GetKey(grabKey))
        //{
        //    wallGrab = true;
        //    wallSlide = false;
        //}

        //if (!collision.onWall || !canMove || Input.GetKeyUp(grabKey))
        //{
        //    wallGrab = false;
        //    wallSlide = false;
        //}

        //if (collision.onGround && !isDashing)
        //{
        //    wallJumped = false;
        //}

        //if (wallGrab && !isDashing)
        //{
        //    rb.gravityScale = 0;
        //    if (x > .2f || x < -.2f) // probs change this to 1
        //        rb.velocity = new Vector2(rb.velocity.x, 0);

        //    float speedModifier = y > 0 ? .5f : 1;

        //    rb.velocity = new Vector2(rb.velocity.x, y * (currSpeed * speedModifier));
        //}
        //else
        //{
        //    rb.gravityScale = 3;
        //}

        //if (collision.onWall && !collision.onGround)
        //{
        //    if (x != 0 && !wallGrab)
        //    {
        //        wallSlide = true;
        //        WallSlide();
        //    }
        //}

        //if (Input.GetKeyDown(dashKey) && !hasDashed)
        //{
        //    if (x != 0 || y != 0)
        //        Dash(new Vector2(x, y));
        //}
    }

    //private void PhysicsTick()
    //{
    //    // Jump Physics
    //    if (shouldJumpPhysics)
    //    {
    //        if (rb.velocity.y < 0)
    //        {
    //            rb.velocity += Vector2.up * Physics2D.gravity.y * (fallMultiplier - 1) * Time.deltaTime;
    //        }
    //        else if (rb.velocity.y > 0 && !Input.GetButton("Jump"))
    //        {
    //            rb.velocity += Vector2.up * Physics2D.gravity.y * (lowJumpMultiplier - 1) * Time.deltaTime;
    //        }
    //    }

    //    // Checks

    //    if (collision.onGround && inAir)
    //    {
    //        hasDashed = false;
    //        isDashing = false;
    //        inAir = false;
    //    }

    //    if (!collision.onGround && !inAir)
    //    {
    //        inAir = true;
    //    }
    //}

    private bool CheckJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            return true;
        }
        return false;
    }

    private bool CheckCanUncrouch()
    {
        Vector2 topLeft = transform.TransformPoint(
            new Vector2(playerColl.offset.x - playerColl.size.x * 0.5f, playerColl.offset.y + playerColl.size.y * 0.5f));
        Vector2 topRight = transform.TransformPoint(
            new Vector2(playerColl.offset.x + playerColl.size.x * 0.5f, playerColl.offset.y + playerColl.size.y * 0.5f));

        RaycastHit2D hitLeft = Physics2D.Raycast(topLeft, Vector2.up, normalPlayerHeight - crouchPlayerHeight, LayerMask.GetMask("Ground"));
        RaycastHit2D hitRight = Physics2D.Raycast(topRight, Vector2.up, normalPlayerHeight - crouchPlayerHeight, LayerMask.GetMask("Ground"));

        if (hitLeft.collider != null || hitRight.collider != null)
        {
            return false;
        }
        return true;
    }

    private void CalcCrouchMovement(Vector2 dir)
    {
        if (dir.x != 0)
        {
            Dir sideDir = dir.x > 0 ? Dir.RIGHT : Dir.LEFT;
            currSpeed = crouchSpeed;
            faceDir = sideDir;
        }
        else
        {
            currSpeed = 0;
        }
    }

    private void CalcMovementAcceleration(Vector2 dir)
    {
        if (dir.x != 0)
        {
            Dir sideDir = dir.x > 0 ? Dir.RIGHT : Dir.LEFT;
            //print(faceDir + "|" + sideDir);
            if (faceDir == sideDir || faceDir == Dir.NONE)
            {
                currSpeed += accelerationSpeed * Time.deltaTime;
                if (currSpeed < minSpeed)
                {
                    currSpeed = minSpeed;
                }
                faceDir = sideDir;
            }
            else
            {
                //currSpeed -= (accelerationSpeed + passiveDecelSpeed) * Time.deltaTime;
                //if (currSpeed < 0)
                //{
                //    faceDir = sideDir;
                //}
                faceDir = sideDir;
            }
        }
        else
        {
            if (currSpeed < passiveDecelSpeed * Time.deltaTime)
            {
                currSpeed = 0;
            }
            else
            {
                currSpeed -= passiveDecelSpeed * Time.deltaTime;
            }
        }

        //if ((faceDir == Dir.RIGHT && collision.wallSide == Collision.WallSide.Right) ||
        //    (faceDir == Dir.LEFT && collision.wallSide == Collision.WallSide.Left))
        //{
        //    currSpeed = 0;
        //    faceDir = Dir.NONE;
        //}

        if (currSpeed > maxSpeed)
        {
            currSpeed = maxSpeed;
        }
        else if (currSpeed < 0)
        {
            currSpeed = 0;
        }
    }

    private void CalcSprintAcceleration(Vector2 dir)
    {
        if (dir.x != 0)
        {
            isSprintGrace = false;
            graceOver = false;
            Dir sideDir = dir.x > 0 ? Dir.RIGHT : Dir.LEFT;
            //print(faceDir + "|" + sideDir);
            if (faceDir == sideDir || faceDir == Dir.NONE)
            {
                if (currSpeed <= beserkThreshold)
                {
                    currSpeed += (accelerationSpeed + beserkAcceleration) * Time.deltaTime;
                }
                else
                {
                    currSpeed += beserkAcceleration * Time.deltaTime;
                }
                faceDir = sideDir;
            }
            else
            {
                //currSpeed -= (accelerationSpeed + passiveDecelSpeed) * Time.deltaTime;
                //if (currSpeed < 0)
                //{
                //    faceDir = sideDir;
                //}
                faceDir = sideDir;
            }
        }
        else
        {
            if (currSpeed < passiveDecelSpeed * Time.deltaTime)
            {
                currSpeed = 0;
            }
            else
            {
                if (graceOver)
                {
                    currSpeed -= passiveDecelSpeed * Time.deltaTime;
                }
                else if (!isSprintGrace)
                {
                    isSprintGrace = true;
                    graceOver = false;
                    sprintGraceTimer = sprintGraceTime;
                }
                else
                {
                    sprintGraceTimer -= Time.deltaTime;
                    if (sprintGraceTimer < 0)
                    {
                        graceOver = true;
                        isSprintGrace = false;
                    }
                }
            }
        }

        //if ((faceDir == Dir.RIGHT && collision.wallSide == Collision.WallSide.Right) ||
        //    (faceDir == Dir.LEFT && collision.wallSide == Collision.WallSide.Left))
        //{
        //    currSpeed = 0;
        //    faceDir = Dir.NONE;
        //}

        if (currSpeed > maxSprintSpeed)
        {
            currSpeed = maxSprintSpeed;
        }
        else if (currSpeed < 0)
        {
            currSpeed = 0;
        }
    }

    //private void Dash(Vector2 dir)
    //{
    //    hasDashed = true;

    //    rb.velocity = Vector2.zero;

    //    rb.velocity = dir.normalized * dashSpeed;

    //    isDashing = true;
    //    dashTimer = dashTime;

    //    rb.gravityScale = 0;
    //    shouldJumpPhysics = false;
    //    wallJumped = true;
    //    isDashing = true;
    //}

    private void Jump(Vector2 dir)
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        //rb.velocity += dir * jumpForce;
    }

    private void WallJump()
    {
        Vector2 wallDir = collision.touchRight ? Vector2.left : Vector2.right;
        faceDir = collision.touchRight ? Dir.LEFT : Dir.RIGHT;

        rb.velocity = wallDir * minSpeed + Vector2.up * jumpForce;
        if (currSpeed < Mathf.Abs(rb.velocity.x))
        {
            currSpeed = Mathf.Abs(rb.velocity.x);
        }
        else
        {
            rb.velocity = new Vector2(wallDir.x * currSpeed, rb.velocity.y);
        }
    }

    private void Dash(Vector2 dir)
    {
        numDashesLeft -= 1;

        rb.velocity = Vector2.zero;
        rb.velocity = dir.normalized * dashSpeed;
        dashTimer = dashTime;
        rb.gravityScale = 0;
    }

    private void WallSlide(Vector2 dir)
    {
        if (!canMove)
        {
            return;
        }

        bool pushingWall = false;
        if ((dir.x > 0 && collision.touchRight) || (dir.x < 0 && collision.touchLeft))
        {
            pushingWall = true;
        }
        float push = pushingWall ? 0 : (minSpeed * dir.x);

        rb.velocity = new Vector2(push, -slideSpeed);
    }

    private void Move()
    {
        if (!canMove)
        {
            return;
        }

        int horizMult = faceDir == Dir.LEFT ? -1 : (faceDir == Dir.RIGHT ? 1 : 0);

        //print(new Vector2(horizMult * currSpeed, rb.velocity.y));

        rb.velocity = new Vector2(horizMult * currSpeed, rb.velocity.y);
    }
}
