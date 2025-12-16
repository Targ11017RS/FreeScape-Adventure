using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DeathHandler : MonoBehaviour
{
    [Header("UI References")]
    public Button dieButton;            // Button to trigger death (for testing)
    public GameObject deathScreen;      // Death screen container

    public CanvasGroup youDiedGroup;    // "You Died" text group
    public CanvasGroup buttonsGroup;    // Buttons (Respawn, etc.)

    public Button respawnButton;        // Respawn button

    [Header("Fade Settings")]
    public float fadeDuration = 1f;

    [Header("Audio")]
    public AudioSource audioSource;     // Audio source for SFX/BGM
    public AudioClip deathSFX;          // Death sound effect
    public AudioClip BGM;               // Background music clip

    [Header("Extra UI")]
    public GameObject LeftAndroPanel;
    public GameObject RightAndroPanel;
    public GameObject TopAndroPanel;
    public GameObject BottomAndroPanel;

    private bool isDead = false;

    private void Start()
    {
        // Ensure UI starts hidden
        deathScreen.SetActive(false);
        SetCanvasGroup(youDiedGroup, 0f, false);
        SetCanvasGroup(buttonsGroup, 0f, false);

        // Button event hookups
        if (dieButton != null)
            dieButton.onClick.AddListener(HandleDeath);

        if (respawnButton != null)
            respawnButton.onClick.AddListener(() =>
            {
                PlayClick();
                Respawn();
            });

        // Start playing background music if not already
        if (audioSource != null && BGM != null)
        {
            audioSource.clip = BGM;
            audioSource.loop = true;
            audioSource.Play();
        }
    }

    void HandleDeath()
    {
        if (isDead) return;

        isDead = true;
        deathScreen.SetActive(true);

        PlaySFX(deathSFX);

        // Hide gameplay UI panels
        LeftAndroPanel.SetActive(false);
        RightAndroPanel.SetActive(false);
        TopAndroPanel.SetActive(false);
        BottomAndroPanel.SetActive(false);

        // Fade in "You Died" and buttons
        StartCoroutine(FadeInCanvasGroup(youDiedGroup, fadeDuration));
        StartCoroutine(FadeInCanvasGroup(buttonsGroup, fadeDuration, fadeDuration * 0.5f));

        // Pause background music
        if (audioSource != null)
            audioSource.Pause();

        // Freeze time if needed (optional)
        Time.timeScale = 0f;
    }

    void Respawn()
    {
        if (!isDead) return;

        isDead = false;

        // Unfreeze time
        Time.timeScale = 1f;

        // Restore UI
        LeftAndroPanel.SetActive(true);
        RightAndroPanel.SetActive(true);
        TopAndroPanel.SetActive(true);
        BottomAndroPanel.SetActive(true);

        // Hide death screen
        deathScreen.SetActive(false);

        // Reset UI fade states
        SetCanvasGroup(youDiedGroup, 0f, false);
        SetCanvasGroup(buttonsGroup, 0f, false);

        // Resume background music
        if (audioSource != null)
        {
            audioSource.clip = BGM;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }
    }

    void PlayClick()
    {
        // Optional: add a click SFX here if you have one
    }

    void PlaySFX(AudioClip clip)
    {
        if (audioSource != null && clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }

    IEnumerator FadeInCanvasGroup(CanvasGroup group, float duration, float delay = 0f)
    {
        yield return new WaitForSecondsRealtime(delay);

        group.gameObject.SetActive(true);
        group.interactable = false;
        group.blocksRaycasts = false;

        float time = 0f;
        while (time < duration)
        {
            group.alpha = Mathf.Lerp(0f, 1f, time / duration);
            time += Time.unscaledDeltaTime;
            yield return null;
        }

        group.alpha = 1f;
        group.interactable = true;
        group.blocksRaycasts = true;
    }

    void SetCanvasGroup(CanvasGroup group, float alpha, bool interactable)
    {
        if (group == null) return;
        group.alpha = alpha;
        group.interactable = interactable;
        group.blocksRaycasts = interactable;
        group.gameObject.SetActive(alpha > 0f);
    }
}
