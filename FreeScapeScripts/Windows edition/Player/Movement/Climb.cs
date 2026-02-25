using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class Climb : MonoBehaviour
{
    public float climbSpeed = 3f;
    public float climbCheckDistance = 1f;
    public float maxClimbAngle = 85f;

    private CharacterController controller;
    private bool isClimbing;

    void Start()
    {
        controller = GetComponent<CharacterController>();
    }

    void Update()
    {
        CheckClimbSurface();

        if (isClimbing)
        {
            float vertical = Input.GetAxis("Vertical"); // W/S input
            Vector3 climbDirection = transform.up * vertical;
            controller.Move(climbDirection * climbSpeed * Time.deltaTime);
        }
    }

    void CheckClimbSurface()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 1f;

        if (Physics.Raycast(origin, transform.forward, out hit, climbCheckDistance))
        {
            float angle = Vector3.Angle(hit.normal, Vector3.up);
            isClimbing = angle > maxClimbAngle; // e.g., steep surfaces
        }
        else
        {
            isClimbing = false;
        }
    }
}
