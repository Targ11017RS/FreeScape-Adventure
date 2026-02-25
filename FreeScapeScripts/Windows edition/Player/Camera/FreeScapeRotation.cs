using UnityEngine;

public class FreeScapeRotation : MonoBehaviour
{
    [Header("References")]
    public Transform cameraTransform;

    [Header("Rotation Settings")]
    public float rotationSpeed = 120f;
    public float smoothTime = 0.1f;
    public float minPitch = -80f;
    public float maxPitch = 80f;

    [Header("180 Turn Settings")]
    public float turn180Duration = 0.25f;

    bool isTurning180 = false;
    float targetYaw180;
    float turn180Velocity;


    float yaw;
    float pitch;

    float yawVelocity;
    float pitchVelocity;

    float targetYawInput;
    float targetPitchInput;

    float currentYawInput;
    float currentPitchInput;

    void Start()
    {
        


        yaw = transform.eulerAngles.y;
        pitch = cameraTransform.localEulerAngles.x;
        
        if (pitch > 180f) pitch -= 360f;


    }

    void Update()
    {
        HandleInput();
        SmoothInput();
        ApplyRotation();
    }

    void HandleInput()
    {
        // 🔁 180 DEGREE TURN
        if (Input.GetKeyDown(KeyCode.Q) && !isTurning180)
    {
        targetYaw180 = yaw + 180f;
        isTurning180 = true;
        return;
    }


        // LEFT / RIGHT
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            targetYawInput = -1f;
        else if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
            targetYawInput = 1f;
        else
            targetYawInput = 0f;

        // UP / DOWN
        if (Input.GetKey(KeyCode.UpArrow))
            targetPitchInput = -1f;
        else if (Input.GetKey(KeyCode.DownArrow))
            targetPitchInput = 1f;
        else
            targetPitchInput = 0f;
    }

    void SmoothInput()
    {
        currentYawInput = Mathf.SmoothDamp(
            currentYawInput,
            targetYawInput,
            ref yawVelocity,
            smoothTime
        );

        currentPitchInput = Mathf.SmoothDamp(
            currentPitchInput,
            targetPitchInput,
            ref pitchVelocity,
            smoothTime
        );
    }

    void ApplyRotation()
{
    // ───── Smooth 180 Turn ─────
    if (isTurning180)
    {
        yaw = Mathf.SmoothDampAngle(
            yaw,
            targetYaw180,
            ref turn180Velocity,
            turn180Duration
        );

        transform.rotation = Quaternion.Euler(0f, yaw, 0f);

        if (Mathf.Abs(Mathf.DeltaAngle(yaw, targetYaw180)) < 0.1f)
        {
            yaw = targetYaw180;
            isTurning180 = false;
        }
        return;
    }

    // ───── Normal rotation ─────
    yaw += currentYawInput * rotationSpeed * Time.deltaTime;
    transform.rotation = Quaternion.Euler(0f, yaw, 0f);

    pitch += currentPitchInput * rotationSpeed * Time.deltaTime;
    pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    cameraTransform.localRotation = Quaternion.Euler(pitch, 0f, 0f);
}

}