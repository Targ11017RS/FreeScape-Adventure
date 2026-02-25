using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class SpecialJumpFunction : MonoBehaviour
{
    [Header("Jump Settings")]
    public float normalJumpForce = 1.6f;
    public float chargedJumpMultiplier = 3f;
    public float gravity = -9.81f;

    [Header("Charge Settings")]
    public float chargeTimeRequired = 5f;
    public float staminaCostPercent = 0.5f; // 50%

    [Header("Ground Check")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundMask;

    CharacterController controller;
    FreeScapeMovement staminaSource;

    float verticalVelocity;
    bool isGrounded;
    float spaceHoldTimer;
    bool jumpExecuted;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        staminaSource = GetComponent<FreeScapeMovement>();

        if (staminaSource == null)
            Debug.LogError("PlayerChargedJump: FreeScapeMovement not found!");
    }

    void Update()
    {
        GroundCheck();
        HandleJumpInput();
        ApplyGravity();
    }

    // ---------------- GROUND CHECK ----------------
    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheckPoint.position,
            groundCheckRadius,
            groundMask
        );

        if (isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;

        if (isGrounded)
        {
            spaceHoldTimer = 0f;
            jumpExecuted = false;
        }
    }

    // ---------------- INPUT ----------------
    void HandleJumpInput()
    {
        if (!isGrounded) return;

        // Hold SPACE
        if (Input.GetKey(KeyCode.Space))
        {
            spaceHoldTimer += Time.deltaTime;
        }

        // Release SPACE
        if (Input.GetKeyUp(KeyCode.Space) && !jumpExecuted)
        {
            PerformJump();
            jumpExecuted = true;
        }
    }

    // ---------------- JUMP LOGIC ----------------
    void PerformJump()
    {
        float jumpForce = normalJumpForce;

        // Charged Jump
        if (spaceHoldTimer >= chargeTimeRequired && staminaSource != null)
        {
            float staminaCost = staminaSource.maxStamina * staminaCostPercent;

            if (staminaSource.GetStaminaPercent() > staminaCostPercent)
            {
                jumpForce *= chargedJumpMultiplier;
                DrainStamina(staminaCost);
            }
        }

        verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
    }

    void DrainStamina(float amount)
    {
        staminaSource.SendMessage(
            "ReduceStamina",
            amount,
            SendMessageOptions.DontRequireReceiver
        );
    }

    // ---------------- GRAVITY ----------------
    void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}