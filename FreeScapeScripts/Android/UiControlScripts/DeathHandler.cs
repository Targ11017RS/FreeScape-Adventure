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
        // Defensive checks & helpful logs
        if (deathScreen == null) Debug.LogWarning("DeathHandler: deathScreen not assigned in Inspector.");
        if (youDiedGroup == null) Debug.LogWarning("DeathHandler: youDiedGroup not assigned in Inspector.");
        if (buttonsGroup == null) Debug.LogWarning("DeathHandler: buttonsGroup not assigned in Inspector.");
        if (respawnButton == null) Debug.LogWarning("DeathHandler: respawnButton not assigned in Inspector.");
        if (LeftAndroPanel == null) Debug.LogWarning("DeathHandler: LeftAndroPanel not assigned in Inspector.");
        if (RightAndroPanel == null) Debug.LogWarning("DeathHandler: RightAndroPanel not assigned in Inspector.");
        if (TopAndroPanel == null) Debug.LogWarning("DeathHandler: TopAndroPanel not assigned in Inspector.");
        if (BottomAndroPanel == null) Debug.LogWarning("DeathHandler: BottomAndroPanel not assigned in Inspector.");

        // Ensure UI starts hidden
        if (deathScreen != null)
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
        if (deathScreen != null)
            deathScreen.SetActive(true);

        PlaySFX(deathSFX);

        // Hide gameplay UI panels (null-safe)
        if (LeftAndroPanel != null) LeftAndroPanel.SetActive(false);
        if (RightAndroPanel != null) RightAndroPanel.SetActive(false);
        if (TopAndroPanel != null) TopAndroPanel.SetActive(false);
        if (BottomAndroPanel != null) BottomAndroPanel.SetActive(false);

        // Fade in "You Died" and buttons
        if (youDiedGroup != null)
            StartCoroutine(FadeInCanvasGroup(youDiedGroup, fadeDuration));
        if (buttonsGroup != null)
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

        // Make sure the game is unpaused immediately so subsequent calls don't get blocked
        Time.timeScale = 1f;

        isDead = false;

        // Restore UI (null-safe)
        if (LeftAndroPanel != null) LeftAndroPanel.SetActive(true);
        if (RightAndroPanel != null) RightAndroPanel.SetActive(true);
        if (TopAndroPanel != null) TopAndroPanel.SetActive(true);
        if (BottomAndroPanel != null) BottomAndroPanel.SetActive(true);

        // Hide death screen
        if (deathScreen != null)
            deathScreen.SetActive(false);

        // Reset UI fade states
        SetCanvasGroup(youDiedGroup, 0f, false);
        SetCanvasGroup(buttonsGroup, 0f, false);

        // Resume background music
        if (audioSource != null)
        {
            // If we paused before, resume playback. If you want to restart the clip, call Play() after assigning clip.
            if (BGM != null)
                audioSource.clip = BGM;
            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        Debug.Log("Respawn executed.");
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

        if (group == null) yield break;

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
        // Keep group active only if visible
        group.gameObject.SetActive(alpha > 0f);
    }
}
