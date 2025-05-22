using UnityEngine;
using System.Collections;

public class PlayerController2d : MonoBehaviour
{
    [Header("Movement Settings")]
    public float moveSpeed = 5f;
    private float moveInput;

    [Header("Jump Settings")]
    public float jumpForce = 10f;
    private bool isGrounded;
    private bool hasDoubleJumped;

    [Header("Power-Ups")]
    public bool canJump = false;
    public bool canDoubleJump = false;

    [Header("Ground Check")]
    public Transform groundCheck;
    public float groundCheckRadius = 0.2f;
    public LayerMask whatIsGround;

    private Rigidbody2D rb;

    [Header("Dash Settings")]
    public bool canDash = false;
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool isDashing = false;
    private float dashCooldownTimer = 0f;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        moveInput = Input.GetAxisRaw("Horizontal");

        if (!isDashing)
        {
            rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        }

        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, whatIsGround);

        if (isGrounded)
        {
            hasDoubleJumped = false;
        }

        if (canJump && Input.GetKeyDown(KeyCode.Space))
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (canDoubleJump && !hasDoubleJumped)
            {
                Jump();
                hasDoubleJumped = true;
            }
        }

        if (canDash && Input.GetKeyDown(KeyCode.LeftShift) && dashCooldownTimer <= 0)
        {
            StartCoroutine(DoDash());
            dashCooldownTimer = dashCooldown;
        }

        if (dashCooldownTimer > 0)
        {
            dashCooldownTimer -= Time.deltaTime;
        }
    }

    void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
    }

    public void EnableJump()
    {
        canJump = true;
    }

    public void EnableDoubleJump()
    {
        canDoubleJump = true;
    }

    public void EnableDash()
    {
        canDash = true;
    }

    private IEnumerator DoDash()
    {
        isDashing = true;

        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0;

        float direction = moveInput != 0 ? Mathf.Sign(moveInput) : transform.localScale.x;
        rb.linearVelocity = new Vector2(direction * dashForce, 0f);

        yield return new WaitForSeconds(dashDuration);

        rb.gravityScale = originalGravity;
        isDashing = false;
    }
}
