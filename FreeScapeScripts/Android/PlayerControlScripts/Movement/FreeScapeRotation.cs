using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class FreeScapeRotation : MonoBehaviour
{
    [Header("Rotation Settings")]
    public float rotationSpeed = 100f;       // Max rotation speed
    public float rotationSmoothTime = 0.1f;  // Smoothing time

    private float currentVelocity = 0f;
    private float currentRotationInput = 0f;
    private float targetRotationInput = 0f;

    private bool rotateLeft = false;
    private bool rotateRight = false;

    [Header("UI Buttons (With EventTrigger)")]
    public Button leftButton;
    public Button rightButton;

    public static FreeScapeRotation Instance;

    private void Awake()
    {
        // Singleton Setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Add hold functionality using EventTrigger
        if (leftButton != null)
            AddHoldEvents(leftButton, StartLookLeft, StopLooking);

        if (rightButton != null)
            AddHoldEvents(rightButton, StartLookRight, StopLooking);
    }

    private void Update()
    {
        // Determine target input
        if (rotateLeft)
            targetRotationInput = -1f;
        else if (rotateRight)
            targetRotationInput = 1f;
        else
            targetRotationInput = 0f;

        // Smooth the input using SmoothDamp
        currentRotationInput = Mathf.SmoothDamp(currentRotationInput, targetRotationInput, ref currentVelocity, rotationSmoothTime);

        // Apply rotation
        transform.Rotate(Vector3.up, currentRotationInput * rotationSpeed * Time.deltaTime);
    }

    // HOLD START/STOP Functions
    public void StartLookLeft() => rotateLeft = true;
    public void StartLookRight() => rotateRight = true;
    public void StopLooking()
    {
        rotateLeft = false;
        rotateRight = false;
    }

    // Helper to add press-hold via EventTrigger
    void AddHoldEvents(Button btn, UnityEngine.Events.UnityAction onPress, UnityEngine.Events.UnityAction onRelease)
    {
        EventTrigger trigger = btn.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = btn.gameObject.AddComponent<EventTrigger>();

        trigger.triggers.Clear(); // Clear existing triggers to avoid duplicates

        // On Press
        EventTrigger.Entry pressEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerDown };
        pressEntry.callback.AddListener((data) => onPress());
        trigger.triggers.Add(pressEntry);

        // On Release
        EventTrigger.Entry releaseEntry = new EventTrigger.Entry { eventID = EventTriggerType.PointerUp };
        releaseEntry.callback.AddListener((data) => onRelease());
        trigger.triggers.Add(releaseEntry);
    }
}
