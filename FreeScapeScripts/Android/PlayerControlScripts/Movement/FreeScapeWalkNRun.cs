using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

[RequireComponent(typeof(CharacterController))]
public class FreeScapeWalkNRun : MonoBehaviour
{
    public float walkSpeed = 2f;
    public float runSpeed = 5f;
    public float smoothTime = 0.15f; // Time to reach target speed

    private CharacterController controller;

    [Header("UI Buttons (with EventTrigger)")]
    public Button walkButton;
    public Button runButton;
    public Button backButton;

    [Header("Stamina Settings")]
    public Slider staminaSlider;
    public float maxStamina = 100f;
    public float staminaDrainRate = 15f;   // per second when running
    public float staminaRecoveryRate = 25f; // per second when not running

    private float currentStamina;

    private bool isWalking = false;
    private bool isRunning = false;
    private bool isMovingBackward = false;

    private Vector3 currentVelocity = Vector3.zero;
    private Vector3 smoothVelocity = Vector3.zero;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        currentStamina = maxStamina;

        if (staminaSlider != null)
        {
            staminaSlider.maxValue = maxStamina;
            staminaSlider.value = currentStamina;
        }

        if (walkButton != null) AddHoldEvents(walkButton, StartWalk, StopMove);
        if (runButton != null) AddHoldEvents(runButton, StartRun, StopMove);
        if (backButton != null) AddHoldEvents(backButton, StartBack, StopMove);
    }

    void Update()
    {
        Vector3 targetVelocity = Vector3.zero;

        if (isRunning && currentStamina > 0f)
        {
            targetVelocity = transform.forward * runSpeed;
        }
        else if (isWalking)
        {
            targetVelocity = transform.forward * walkSpeed;
        }
        else if (isMovingBackward)
        {
            targetVelocity = -transform.forward * walkSpeed; // walkSpeed for back
        }

        // Smooth movement using SmoothDamp
        currentVelocity = Vector3.SmoothDamp(currentVelocity, targetVelocity, ref smoothVelocity, smoothTime);
        controller.Move(currentVelocity * Time.deltaTime);

        HandleStamina();
    }

    void HandleStamina()
    {
        if (isRunning && currentStamina > 0f)
        {
            currentStamina -= staminaDrainRate * Time.deltaTime;
            if (currentStamina <= 0f)
            {
                currentStamina = 0f;
                StopMove(); // stop when stamina runs out
            }
        }
        else if (!isRunning && currentStamina < maxStamina)
        {
            currentStamina += staminaRecoveryRate * Time.deltaTime;
            if (currentStamina > maxStamina) currentStamina = maxStamina;
        }

        if (staminaSlider != null)
            staminaSlider.value = currentStamina;
    }

    // Movement states
    public void StartWalk()
    {
        isWalking = true;
        isRunning = false;
        isMovingBackward = false;
    }

    public void StartRun()
    {
        if (currentStamina > 0f)
        {
            isRunning = true;
            isWalking = false;
            isMovingBackward = false;
        }
    }

    public void StartBack()
    {
        isMovingBackward = true;
        isWalking = false;
        isRunning = false;
    }

    public void StopMove()
    {
        isWalking = false;
        isRunning = false;
        isMovingBackward = false;
    }

    // Button event binding
    void AddHoldEvents(Button btn, UnityEngine.Events.UnityAction onPress, UnityEngine.Events.UnityAction onRelease)
    {
        EventTrigger trigger = btn.GetComponent<EventTrigger>();
        if (trigger == null) trigger = btn.gameObject.AddComponent<EventTrigger>();

        EventTrigger.Entry press = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        press.callback.AddListener((data) => onPress());
        trigger.triggers.Add(press);

        EventTrigger.Entry release = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        release.callback.AddListener((data) => onRelease());
        trigger.triggers.Add(release);
    }
}
