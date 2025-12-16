using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SimpleJumpHoldCrouchFPP : MonoBehaviour
{
    [Header("Player Settings")]
    public float jumpForce = 5f;
    public float gravity = -9.81f;
    public float standingHeight = 2f;
    public float crouchHeight = 1f;

    [Header("References")]
    public CharacterController controller;
    public Transform cameraTransform; // optional
    public Button jumpButton;
    public GameObject crouchButtonObject;

    private Vector3 velocity;
    private bool isGrounded;
    private bool crouchHeld = false;

    void Start()
    {
        if (controller == null) controller = GetComponent<CharacterController>();

        jumpButton.onClick.AddListener(HandleJump);
        AddCrouchButtonEvents();
    }

    void Update()
    {
        HandleGravity();
        HandleCrouchState();
    }

    void HandleGravity()
    {
        isGrounded = controller.isGrounded;

        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;

        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);
    }

    void HandleJump()
    {
        if (isGrounded)
            velocity.y = Mathf.Sqrt(jumpForce * -2f * gravity);
    }

    void HandleCrouchState()
    {
        if (crouchHeld)
        {
            controller.height = crouchHeight;

            if (cameraTransform != null)
                cameraTransform.localPosition = new Vector3(0f, 0.5f, 0f);
        }
        else
        {
            controller.height = standingHeight;

            if (cameraTransform != null)
                cameraTransform.localPosition = new Vector3(0f, 0.8f, 0f);
        }
    }

    void AddCrouchButtonEvents()
    {
        EventTrigger trigger = crouchButtonObject.GetComponent<EventTrigger>();
        if (trigger == null)
            trigger = crouchButtonObject.AddComponent<EventTrigger>();

        // On Pointer Down
        EventTrigger.Entry pressEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerDown
        };
        pressEntry.callback.AddListener((eventData) => { crouchHeld = true; });
        trigger.triggers.Add(pressEntry);

        // On Pointer Up
        EventTrigger.Entry releaseEntry = new EventTrigger.Entry
        {
            eventID = EventTriggerType.PointerUp
        };
        releaseEntry.callback.AddListener((eventData) => { crouchHeld = false; });
        trigger.triggers.Add(releaseEntry);
    }
}
