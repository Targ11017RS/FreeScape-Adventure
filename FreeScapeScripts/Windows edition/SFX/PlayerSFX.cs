using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerSFX : MonoBehaviour
{
    [Header("References")]
    public AudioClip jumpSFX;
    public AudioClip landSFX;
    public AudioClip groundSFX;
    public AudioClip grassSFX;
    public AudioClip woodSFX;

    [Header("Surface Tags")]
    public string groundTag = "Ground";
    public string grassTag = "Grass";
    public string woodTag = "Wood";

    private CharacterController controller;
    private Vector3 lastPosition;
    private bool wasGroundedLastFrame;

    void Start()
    {
        controller = GetComponent<CharacterController>();
        lastPosition = transform.position;
    }

    void Update()
    {
        bool isGrounded = controller.isGrounded;

        // Detect jump
        if (isGrounded && Input.GetKeyDown(KeyCode.Space))
        {
            PlayOneShot(jumpSFX);
        }

        // Detect landing
        if (!wasGroundedLastFrame && isGrounded)
        {
            PlayOneShot(landSFX);
            PlaySurfaceContactSFX();
        }

        wasGroundedLastFrame = isGrounded;
        lastPosition = transform.position;
    }

    void PlayOneShot(AudioClip clip)
    {
        if (clip != null && vol.Instance != null)
            vol.Instance.GetComponent<AudioSource>().PlayOneShot(clip);
    }

    void PlaySurfaceContactSFX()
    {
        RaycastHit hit;
        Vector3 origin = transform.position + Vector3.up * 0.1f;

        if (Physics.Raycast(origin, Vector3.down, out hit, 2f))
        {
            string tag = hit.collider.tag;

            if (tag == grassTag)
                PlayOneShot(grassSFX);
            else if (tag == woodTag)
                PlayOneShot(woodSFX);
            else if (tag == groundTag)
                PlayOneShot(groundSFX);
        }
    }
}
