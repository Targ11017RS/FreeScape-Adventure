using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerCrouch : MonoBehaviour
{
    [Header("Crouch Settings")]
    public float crouchMultiplier = 0.6f; // % of current height
    public float transitionSpeed = 8f;
    public KeyCode crouchKey = KeyCode.J;

    CharacterController controller;

    float standingHeight;
    float targetHeight;
    float currentHeight;

    bool isCrouching;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        standingHeight = controller.height;
        currentHeight = standingHeight;
    }

    void Update()
    {
        // Detect crouch input (J key)
        isCrouching = Input.GetKey(crouchKey);

        // IMPORTANT: Always read latest standing height (for aging system)
        standingHeight = Mathf.Max(standingHeight, controller.height);

        targetHeight = isCrouching 
            ? standingHeight * crouchMultiplier 
            : standingHeight;

        currentHeight = Mathf.Lerp(
            controller.height,
            targetHeight,
            Time.deltaTime * transitionSpeed
        );

        controller.height = currentHeight;
        controller.center = new Vector3(0, currentHeight / 2f, 0);
    }
}
