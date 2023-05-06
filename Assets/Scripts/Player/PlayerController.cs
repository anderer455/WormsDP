using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CW.Common;
using URandom = UnityEngine.Random;
using TMPro;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private float movementSpeed;
    public float MovementSpeed
    {
        get { return movementSpeed; }
        set { movementSpeed = value; }
    }
    public TeamColor teamColor;
    private int health = 100;

    [SerializeField]
    private float groundCheckRadius;
    [SerializeField]
    private float jumpForce;
    [SerializeField]
    private float slopeCheckDistance;
    [SerializeField]
    private float maxSlopeAngle;
    [SerializeField]
    private Transform groundCheck;
    [SerializeField]
    private LayerMask whatIsGround;
    [SerializeField]
    private PhysicsMaterial2D noFriction;
    [SerializeField]
    private PhysicsMaterial2D fullFriction;

    private float xInput;
    private float slopeDownAngle;
    private float slopeSideAngle;
    private float lastSlopeAngle;

    private int facingDirection;

    private bool isGrounded;
    private bool isOnSlope;
    private bool isJumping;
    private bool isWalking;
    private bool canWalkOnSlope;
    private bool canJump;

    private Vector2 newVelocity;
    private Vector2 newForce;
    private Vector2 capsuleColliderSize;

    private Vector2 slopeNormalPerp;

    private Rigidbody2D rb;
    private CapsuleCollider2D cc;

    //Animation
    public GameObject wormGraphics;
    public GameObject armGraphics;
    public GameObject weaponGraphics;
    private Animator wormAnimator;
    private Animator armAnimator;

    //Weapon
    [SerializeField]
    public Weapon weapon;
    private Vector2 initialMousePosition;
    public bool isMouseDown = false;
    public Camera mainCamera;
    public static WeaponType activeWeapon = WeaponType.ROCKETLAUNCHER;
    public static ProjectileType activeProjectile = ProjectileType.ROCKET;
    private Transform pointOfLaunch;

    //Launch indicator
    public GameObject indicator;
    private float maxDistance = 4f;
    private float minDistance = 0.1f;
    private float minDistanceUp = 0.4f;
    private float maxScale = 2f;

    private void Start()
    {
        InitializeWorm();
    }

    private void Update()
    {
        MyUpdate();
    }

    private void FixedUpdate()
    {
        MyFixedUpdate(xInput);
    }

    public void MyUpdate()
    {
        if (Gameplay.activeTeamColor == teamColor) {
            CheckInput();
        }
    }

    public void MyFixedUpdate(float xAxisInput)
    {
        if (Gameplay.activeTeamColor == teamColor) {
            CheckGround();
            SlopeCheck();
            ApplyMovement(xAxisInput);
        }
    }

    private void InitializeWorm() {
        rb = GetComponent<Rigidbody2D>();
        cc = GetComponent<CapsuleCollider2D>();
        wormAnimator = wormGraphics.GetComponent<Animator>();
        armAnimator = armGraphics.GetComponent<Animator>();
        capsuleColliderSize = cc.size;
        pointOfLaunch = transform.Find("Arm Graphics/Point Of Launch");
        facingDirection = URandom.Range(0f, 1f) < 0.5f ? -1 : 1;
        if (facingDirection == -1) {
            facingDirection = 1;
            Flip();
        }
        teamColor = Gameplay.validTeamColors[1];
    }

    private void CheckInput()
    {
        if (!PauseMenu.isPaused && !InventoryMenu.isOpened) {
            if (Input.GetMouseButtonDown(1)) {
                if (isMouseDown == true) {
                    StopLaunching();
                }
            }

            xInput = Input.GetAxisRaw("Player_Move");

            if (xInput == 1 && facingDirection == -1 && isMouseDown != true) {
                Flip();
            }
            else if (xInput == -1 && facingDirection == 1 && isMouseDown != true) {
                Flip();
            }

            if (Input.GetButtonDown("Player_Jump")) {
                Jump();
            }

            if ((Input.GetMouseButtonDown(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)) && isMouseDown == false) {
                isMouseDown = true;
                initialMousePosition = GetWorldPosition();
                weapon.SetSprite();
            }

            if (isMouseDown == true) {
                indicator.SetActive(true);
                Vector2 currentMousePosition = GetWorldPosition();

                var angle = CwHelper.Atan2(currentMousePosition - initialMousePosition) * Mathf.Rad2Deg;
                var scale = Mathf.Clamp01(Vector3.Distance(currentMousePosition, initialMousePosition) / maxDistance) * maxScale;

                if (scale > maxScale) {
                    scale = maxScale;
                }

                
                indicator.transform.position = pointOfLaunch.position;
                indicator.transform.localScale = new Vector3(scale, scale, 1);
                indicator.transform.rotation = Quaternion.Euler(0.0f, 0.0f, -angle);

                if (((currentMousePosition.x < initialMousePosition.x) && facingDirection == 1) || ((currentMousePosition.x > initialMousePosition.x) && facingDirection == -1)) {
                    Flip();
                    armGraphics.transform.rotation = Quaternion.Euler(armGraphics.transform.rotation.x, armGraphics.transform.rotation.y, -(angle+90));
                } else {
                    armGraphics.transform.rotation = Quaternion.Euler(armGraphics.transform.rotation.x, armGraphics.transform.rotation.y, -(angle-90));
                }
                
                if (facingDirection == -1) {
                    weaponGraphics.transform.localRotation = Quaternion.Euler(180.0f, 0.0f, 0.0f);
                } else if (facingDirection == 1) {
                    weaponGraphics.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
                }
            }

            if ((Input.GetMouseButtonUp(0) || (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Ended)) && isMouseDown == true) {
                Launching();
            }
        }
    }

    public void Launching() {
        Vector2 currentMousePosition = GetWorldPosition();
        Vector2 direction = (currentMousePosition - initialMousePosition).normalized;
        float distance = Vector2.Distance(initialMousePosition, currentMousePosition);

        if (direction == Vector2.zero) {
            StopLaunching();
            return;
        }

        if (distance < minDistance) {
            if (direction == Vector2.up) {
                distance = minDistanceUp;
            } else {
                distance = minDistance;
            }
        } else if (distance > maxDistance) {
            distance = maxDistance;
        }

        weapon.Launch(direction, distance, maxDistance, activeProjectile, activeWeapon);
        StopLaunching();
    }

    public void StopLaunching() {
        isMouseDown = false;
        weapon.ClearSprite();
        indicator.SetActive(false);
        armGraphics.transform.localRotation = Quaternion.Euler(armGraphics.transform.rotation.x, armGraphics.transform.rotation.y, 0.0f);
        weaponGraphics.transform.localRotation = Quaternion.Euler(0.0f, 0.0f, 0.0f);
    }

    private void CheckGround()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if(rb.velocity.y <= 0.0f) {
            isJumping = false;
            
            if (wormAnimator != null) {
                wormAnimator.ResetTrigger("Worm Jump");
                wormAnimator.SetTrigger("Worm Falling");
            }
        }

        if(isGrounded && !isJumping && slopeDownAngle <= maxSlopeAngle) {
            canJump = true;
            
            if (wormAnimator != null) {
                wormAnimator.ResetTrigger("Worm Falling");
            }

            if (!isWalking) {
                if (wormAnimator != null) {
                    wormAnimator.ResetTrigger("Worm Walking");
                    wormAnimator.SetTrigger("Worm Idle");
                }
                if (armAnimator != null) {
                    armAnimator.ResetTrigger("Arm Walking");
                    armAnimator.SetTrigger("Arm Idle");
                }
            }
        }
    }

    private void SlopeCheck()
    {
        Vector2 checkPos = transform.position - (Vector3)(new Vector2(0.0f, capsuleColliderSize.y / 2));
        SlopeCheckHorizontal(checkPos);
        SlopeCheckVertical(checkPos);
    }

    private void SlopeCheckHorizontal(Vector2 checkPos)
    {
        RaycastHit2D slopeHitFront = Physics2D.Raycast(checkPos, transform.right, slopeCheckDistance, whatIsGround);
        RaycastHit2D slopeHitBack = Physics2D.Raycast(checkPos, -transform.right, slopeCheckDistance, whatIsGround);

        if (slopeHitFront) {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitFront.normal, Vector2.up);
        } else if (slopeHitBack) {
            isOnSlope = true;
            slopeSideAngle = Vector2.Angle(slopeHitBack.normal, Vector2.up);
        } else {
            slopeSideAngle = 0.0f;
            isOnSlope = false;
        }

    }

    private void SlopeCheckVertical(Vector2 checkPos)
    {      
        RaycastHit2D hit = Physics2D.Raycast(checkPos, Vector2.down, slopeCheckDistance, whatIsGround);

        if (hit) {
            slopeNormalPerp = Vector2.Perpendicular(hit.normal).normalized;            
            slopeDownAngle = Vector2.Angle(hit.normal, Vector2.up);

            if(slopeDownAngle != lastSlopeAngle) {
                isOnSlope = true;
            }                       

            lastSlopeAngle = slopeDownAngle;
           
            Debug.DrawRay(hit.point, slopeNormalPerp, Color.blue);
            Debug.DrawRay(hit.point, hit.normal, Color.green);
        }

        if (slopeDownAngle > maxSlopeAngle || slopeSideAngle > maxSlopeAngle) {
            canWalkOnSlope = false;
        } else {
            canWalkOnSlope = true;
        }

        if (isOnSlope && canWalkOnSlope && xInput == 0.0f) {
            rb.sharedMaterial = fullFriction;
        } else {
            rb.sharedMaterial = noFriction;
        }
    }

    public void Jump()
    {
        if (Gameplay.activeTeamColor == teamColor) {
            if (canJump) {
                canJump = false;
                isJumping = true;
                if (wormAnimator != null) {
                    wormAnimator.SetTrigger("Worm Jump");
                }
                newVelocity.Set(0.0f, 0.0f);
                rb.velocity = newVelocity;
                newForce.Set(0.0f, jumpForce);
                rb.AddForce(newForce, ForceMode2D.Impulse);
            }
        }
    }   

    public void ApplyMovement(float xAxisInput)
    {
        if (isGrounded && !isOnSlope && !isJumping) { //if not on slope
            newVelocity.Set(movementSpeed * xAxisInput, 0.0f);
            rb.velocity = newVelocity;
            if (wormAnimator != null && newVelocity != Vector2.zero) {
                if (!isWalking) {
                    isWalking = true;
                    wormAnimator.ResetTrigger("Worm Idle");
                    wormAnimator.SetTrigger("Worm Walking");
                    if (armAnimator != null) {
                        armAnimator.ResetTrigger("Arm Idle");
                        armAnimator.SetTrigger("Arm Walking");
                    }
                }
            } else {
                isWalking = false;
            }
        } else if (isGrounded && isOnSlope && canWalkOnSlope && !isJumping) { //If on slope
            newVelocity.Set(movementSpeed * slopeNormalPerp.x * -xAxisInput, movementSpeed * slopeNormalPerp.y * -xAxisInput);
            rb.velocity = newVelocity;
            if (wormAnimator != null && newVelocity != Vector2.zero) {
                if (!isWalking) {
                    isWalking = true;
                    wormAnimator.ResetTrigger("Worm Idle");
                    wormAnimator.SetTrigger("Worm Walking");
                    if (armAnimator != null) {
                        armAnimator.ResetTrigger("Arm Idle");
                        armAnimator.SetTrigger("Arm Walking");
                    }
                }
            } else {
                isWalking = false;
            }
        } else if (!isGrounded) { //If in air
            canJump = false;
            newVelocity.Set(movementSpeed * xAxisInput, rb.velocity.y);
            rb.velocity = newVelocity;
            if (wormAnimator != null) {
                if (newVelocity.y > 0) {
                    wormAnimator.SetTrigger("Worm Jumping");
                    wormAnimator.ResetTrigger("Worm Falling");
                } else if (newVelocity.y < 0) {
                    wormAnimator.SetTrigger("Worm Falling");
                    wormAnimator.ResetTrigger("Worm Jumping");
                }
            }
        }
    }

    private void Flip()
    {
        facingDirection *= -1;
        transform.Rotate(0.0f, 180.0f, 0.0f);
    }

    public void TakeDamage(int damage) {
        health -= damage;
        transform.Find("HealthCanvas/HealthBar").GetComponent<TextMeshProUGUI>().text = health.ToString();
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
 
    private Vector2 GetWorldPosition()
    {
        Vector2 screenPosition = Input.mousePosition;
        Vector2 worldPosition = mainCamera.ScreenToWorldPoint(screenPosition);
        return worldPosition;
    }
}