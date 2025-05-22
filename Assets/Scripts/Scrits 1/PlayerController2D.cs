using UnityEngine;
using System.Collections.Generic;

public class PlayerController2D : MonoBehaviour
{

    private Queue<(Vector3 position, float timestamp)> groundedPositions = new Queue<(Vector3, float)>();

    [Header("Movement")]
    public float moveSpeed = 8f;
    private float moveInput;

    [Header("Jump")]
    public float jumpForce = 14f;
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;
    private bool isGrounded;
    private bool jumpPressed;

    [Header("Coyote Time")]
    public float coyoteTime = 0.2f;
    private float coyoteTimeCounter;

    [Header("Jump Hang Time")]
    public float jumpBufferTime = 0.15f;
    private float jumpBufferCounter;

    [Header("Abilities (Set by Power-Ups)")]
    public bool canDoubleJump = false;
    public bool canDash = false;
    public bool canShiftReality = false;

    private bool hasDoubleJumped = false;
    private bool isDashing = false;

    [Header("Dash Settings")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private float lastDashTime;

    private int facingDirection = 1; // 1 = right, -1 = left


    [Header("Reality Shift")]
    public RealityShiftManager shiftManager;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        // Ground check
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);
        if (isGrounded)
        {
            groundedPositions.Enqueue((transform.position, Time.time));
            // Keep only the last 10 seconds of history
            while (groundedPositions.Count > 0 && Time.time - groundedPositions.Peek().timestamp > 10f)
            {
                groundedPositions.Dequeue();
            }
        }

        // Coyote time
        if (isGrounded)
        {
            coyoteTimeCounter = coyoteTime;
            hasDoubleJumped = false;
        }
        else
        {
            coyoteTimeCounter -= Time.deltaTime;
        }

        // Jump buffer input
        if (Input.GetButtonDown("Jump"))
        {
            jumpPressed = true;
            jumpBufferCounter = jumpBufferTime;
        }
        else
        {
            jumpBufferCounter -= Time.deltaTime;
        }

        // Handle jump
        if (jumpBufferCounter > 0f)
        {
            if (coyoteTimeCounter > 0f)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                jumpBufferCounter = 0f;

                // Reality shift on first jump only
                if (canShiftReality && shiftManager != null)
                    shiftManager.SwapReality();
            }
            else if (canDoubleJump && !hasDoubleJumped)
            {
                rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
                hasDoubleJumped = true;
                jumpBufferCounter = 0f;
            }
        }

        // Dash
        if (canDash && Input.GetKeyDown(KeyCode.LeftShift) && Time.time > lastDashTime + dashCooldown)
        {
            StartCoroutine(Dash());
        }
        if (moveInput != 0)
        {
            facingDirection = (int)Mathf.Sign(moveInput);
            transform.localScale = new Vector3(facingDirection, 1, 1); // Optional sprite flip
        }
        


    }

    void FixedUpdate()
    {
        if (!isDashing)
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
    }

    System.Collections.IEnumerator Dash()
    {
        isDashing = true;
        lastDashTime = Time.time;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;

        Vector2 dashVelocity = new Vector2(facingDirection, 0f) * dashForce;

        rb.linearVelocity = dashVelocity;

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }

    // Power-up unlock methods
    public void UnlockDoubleJump() => canDoubleJump = true;
    public void UnlockDash() => canDash = true;
    public void UnlockRealityShift() => canShiftReality = true;

    public void ResetToFiveSecondsAgo()
    {
        foreach (var record in groundedPositions)
        {
            if (Time.time - record.timestamp >= 5f)
            {
                transform.position = record.position;
                rb.linearVelocity = Vector2.zero; // optional: reset velocity
                return;
            }
        }

        // fallback: if no record 5 seconds old, use oldest available
        if (groundedPositions.Count > 0)
        {
            transform.position = groundedPositions.Peek().position;
            rb.linearVelocity = Vector2.zero;
        }
    }

}
