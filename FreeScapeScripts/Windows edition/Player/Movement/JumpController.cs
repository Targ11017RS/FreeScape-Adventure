using UnityEngine;
// empty child gameobject at groundcheckpoint is required!
[RequireComponent(typeof(CharacterController))]
public class JumpController : MonoBehaviour
{
    [Header("Jump Settings")]
    public float jumpForce = 1.6f;
    public float gravity = -9.81f;

    [Header("Ground Check")]
    public Transform groundCheckPoint;
    public float groundCheckRadius = 0.3f;
    public LayerMask groundMask;

    CharacterController controller;
    float verticalVelocity;
    bool isGrounded;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        GroundCheck();
        HandleJump();
        ApplyGravity();
    }

    void GroundCheck()
    {
        isGrounded = Physics.CheckSphere(
            groundCheckPoint.position,
            groundCheckRadius,
            groundMask
        );

        if (isGrounded && verticalVelocity < 0f)
            verticalVelocity = -2f;
    }

    void HandleJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && isGrounded)
        {
            verticalVelocity = Mathf.Sqrt(jumpForce * -2f * gravity);
        }
    }

    void ApplyGravity()
    {
        verticalVelocity += gravity * Time.deltaTime;
        controller.Move(Vector3.up * verticalVelocity * Time.deltaTime);
    }
}