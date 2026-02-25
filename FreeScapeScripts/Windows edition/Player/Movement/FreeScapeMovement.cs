using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CharacterController))]
public class FreeScapeMovement : MonoBehaviour
{
    [Header("References")]
    public LifeCycleController lifeCycle;   // ← assign in Inspector

    [Header("Movement")]
    public float walkSpeed = 2f;
    public float baseRunSpeed = 5f;
    public float smoothTime = 0.15f;

    [Header("Gravity & Ground")]
    public float gravity = -9.81f;
    public float groundCheckDistance = 0.3f;
    public LayerMask groundMask;
    public Transform groundCheckPoint;

    [Header("Stamina")]
    public Slider staminaSlider;
    public float maxStamina = 100f;
    public float baseDrainRate = 20f;
    public float baseRecoveryRate = 15f;
    public float runThresholdPercent = 0.75f;

    CharacterController controller;

    Vector3 currentVelocity;
    Vector3 smoothVelocity;
    float verticalVelocity;

    float currentStamina;
    float staminaVelocity;

    bool isGrounded;
    bool isRunning;
    bool isMovingForward;

    float currentRunSpeed;
    float currentDrainRate;
    float currentRecoveryRate;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }
    }

    void Update()
    {
        ApplyAgingEffects();

        GroundCheck();
        HandleMovement();
        HandleGravity();
        HandleStamina();
        UpdateSlider();
    }

    void ApplyAgingEffects()
    {
        if (lifeCycle == null) return;

        float age = lifeCycle.age;

        if (age < 60f)
        {
            currentRunSpeed = baseRunSpeed;
            currentDrainRate = baseDrainRate;
            currentRecoveryRate = baseRecoveryRate;
            return;
        }

        // Normalize age from 60 → 90
        float agingFactor = Mathf.InverseLerp(60f, 90f, age);

        // At 90:
        // Run speed reduced by 40%
        // Drain increased by 50%
        // Recovery reduced by 50%

        currentRunSpeed = Mathf.Lerp(baseRunSpeed, baseRunSpeed * 0.6f, agingFactor);
        currentDrainRate = Mathf.Lerp(baseDrainRate, baseDrainRate * 1.5f, agingFactor);
        currentRecoveryRate = Mathf.Lerp(baseRecoveryRate, baseRecoveryRate * 0.5f, agingFactor);
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheckPoint.position,
            groundCheckDistance,
            groundMask
        );

        if (isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;
    }

    void HandleMovement()
    {
        Vector3 targetVelocity = Vector3.zero;

        isMovingForward = Input.GetKey(KeyCode.W);
        bool backward = Input.GetKey(KeyCode.S);
        bool shiftPressed = Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift);

        bool hasEnoughStamina = currentStamina >= maxStamina * runThresholdPercent;

        isRunning = shiftPressed && isMovingForward && hasEnoughStamina;

        if (isRunning)
            targetVelocity = transform.forward * currentRunSpeed;
        else if (isMovingForward)
            targetVelocity = transform.forward * walkSpeed;
        else if (backward)
            targetVelocity = -transform.forward * walkSpeed;

        currentVelocity = Vector3.SmoothDamp(
            currentVelocity,
            targetVelocity,
            ref smoothVelocity,
            smoothTime
        );
    }

    void HandleGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;

        Vector3 move = currentVelocity;
        move.y = verticalVelocity;

        controller.Move(move * Time.deltaTime);
    }

    void HandleStamina()
    {
        float targetStamina = currentStamina;

        if (isRunning)
            targetStamina -= currentDrainRate * Time.deltaTime;
        else
            targetStamina += currentRecoveryRate * Time.deltaTime;

        targetStamina = Mathf.Clamp(targetStamina, 0f, maxStamina);

        currentStamina = Mathf.SmoothDamp(
            currentStamina,
            targetStamina,
            ref staminaVelocity,
            0.2f
        );
    }

    void UpdateSlider()
    {
        if (staminaSlider == null) return;

        staminaSlider.maxValue = maxStamina;
        staminaSlider.value = currentStamina;
    }

    public float GetStaminaPercent()
    {
        return currentStamina / maxStamina;
    }

    public void ReduceStamina(float amount)
    {
        currentStamina -= amount;
        currentStamina = Mathf.Clamp(currentStamina, 0f, maxStamina);
    }
}
