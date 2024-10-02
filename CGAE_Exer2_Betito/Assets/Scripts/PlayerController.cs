using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float jumpForce = 5f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private bool isGrounded;
    private SpriteRenderer spriteRenderer;

    // Gun mechanics variables
    private bool isHoldingGun = false;  // This tracks whether the player is holding a gun
    public Sprite idleSprite;
    public Sprite holdingGunSprite;
    public Sprite shootingSprite;

    // Input actions reference
    private PlayerControls inputActions;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();

        inputActions = new PlayerControls(); // Initialize the input actions
    }

    private void OnEnable()
    {
        inputActions.Enable();

        // Subscribe to action events
        inputActions.Player.Movement.performed += OnMove;
        inputActions.Player.Movement.canceled += OnMove;
        inputActions.Player.Jump.performed += OnJump;
        inputActions.Player.Shoot.performed += OnShoot;
        inputActions.Player.ToggleGun.performed += OnToggleGun;
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        InputAction.Movement = context.ReadValue<float>();  // Read the float value directly for horizontal movement
    }

    private void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded)
        {
            rb.AddForce(new Vector2(0f, jumpForce), ForceMode2D.Impulse);
        }
    }

    private void OnShoot(InputAction.CallbackContext context)
    {
        if (isHoldingGun)
        {
            Shoot();
        }
    }

    private void OnToggleGun(InputAction.CallbackContext context)
    {
        isHoldingGun = !isHoldingGun;
        UpdateSprite();
    }

    private void UpdateSprite()
    {
        if (isHoldingGun)
        {
            spriteRenderer.sprite = holdingGunSprite;
        }
        else
        {
            spriteRenderer.sprite = idleSprite;
        }
    }

    private void Shoot()
    {
        spriteRenderer.sprite = shootingSprite;
        Invoke("ResetToHoldingGunSprite", 0.1f);
    }

    private void ResetToHoldingGunSprite()
    {
        spriteRenderer.sprite = holdingGunSprite;
    }

    void FixedUpdate()
    {
        rb.velocity = new Vector2(movement.x * moveSpeed, rb.velocity.y);  // Apply movement
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;  // Player is grounded
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;  // Player is no longer grounded
        }
    }
}
