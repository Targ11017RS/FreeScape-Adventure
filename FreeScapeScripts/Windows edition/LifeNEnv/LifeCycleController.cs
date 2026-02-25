using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class LifeCycleController : MonoBehaviour
{
    [Header("Age System")]
    public TMP_Text ageText;
    public float age = 0f;
    public float yearsPerMinute = 4f;

    int naturalDeathAge;
    bool deathUnlocked = false;

    [Header("Player")]
    public FreeScapeMovement playerMovement;
    public CapsuleCollider playerCapsule;
    public Transform playerCamera;
    public float baseMaxStamina = 100f;

    [Header("Height")]
    public float childHeight = 1.0f;
    public float adultHeight = 2.0f;
    public float cameraHeightMultiplier = 0.9f;

    [Header("Sky")]
    public SkyBoxManipulator sky;

    [Header("UI")]
    public TMP_Text endLifeText;
    public Button endLifeButton;
    public Button liveMoreButton;

    [Header("Credits")]
    public CreditsVideoController creditsController;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioClip gameplayMusic;

    bool isEnding = false;
    bool uiVisible = false;

    void Start()
    {
        naturalDeathAge = Random.Range(75, 91);

        if (musicSource != null && gameplayMusic != null)
        {
            musicSource.clip = gameplayMusic;
            musicSource.Play();
        }

        // Hide UI at start
        if (endLifeText != null)
            endLifeText.gameObject.SetActive(false);

        if (endLifeButton != null)
        {
            endLifeButton.gameObject.SetActive(false);
            endLifeButton.onClick.AddListener(ManualEndLife);
        }

        if (liveMoreButton != null)
        {
            liveMoreButton.gameObject.SetActive(false);
            liveMoreButton.onClick.AddListener(LiveMore);
        }
    }

    void Update()
    {
        if (isEnding) return;

        UpdateAge();
        UpdateStamina();
        UpdateHeightAndCamera();
        SyncSky();

        HandleRandomDeath();
        HandleEndLifeUI();
    }

    void UpdateAge()
    {
        age += (Time.deltaTime / 60f) * yearsPerMinute;

        if (ageText != null)
            ageText.text = "Age: " + Mathf.FloorToInt(age);
    }

    void UpdateStamina()
    {
        if (playerMovement == null) return;

        float normalizedAge = Mathf.Clamp01(age / 90f);
        float staminaCurve = Mathf.Sin(normalizedAge * Mathf.PI);
        float targetMax = Mathf.Lerp(35f, baseMaxStamina, staminaCurve);

        playerMovement.maxStamina =
            Mathf.Lerp(playerMovement.maxStamina, targetMax, Time.deltaTime * 1.5f);
    }

    void UpdateHeightAndCamera()
    {
        if (playerCapsule == null || playerCamera == null) return;

        float growthPhase = Mathf.Clamp01(age / 18f);
        float targetHeight = Mathf.Lerp(childHeight, adultHeight, growthPhase);

        float newHeight = Mathf.Lerp(playerCapsule.height, targetHeight, Time.deltaTime * 0.8f);

        playerCapsule.height = newHeight;
        playerCapsule.center = new Vector3(0, newHeight / 2f, 0);

        Vector3 camPos = playerCamera.localPosition;
        camPos.y = newHeight * cameraHeightMultiplier;
        playerCamera.localPosition = camPos;
    }

    void SyncSky()
    {
        if (sky != null)
            sky.currentAge = Mathf.FloorToInt(age);
    }

    void HandleRandomDeath()
    {
        if (age >= 75f && !deathUnlocked)
        {
            deathUnlocked = true;
        }

        if (deathUnlocked && age >= naturalDeathAge)
        {
            StartCoroutine(EndLifeSequence());
        }
    }

    void HandleEndLifeUI()
    {
        if (Input.GetKeyDown(KeyCode.F5))
        {
            uiVisible = !uiVisible;

            if (endLifeText != null)
                endLifeText.gameObject.SetActive(uiVisible);

            if (endLifeButton != null)
                endLifeButton.gameObject.SetActive(uiVisible);

            if (liveMoreButton != null)
                liveMoreButton.gameObject.SetActive(uiVisible);
        }
    }

    public void ManualEndLife()
    {
        if (!isEnding)
            StartCoroutine(EndLifeSequence());
    }

    public void LiveMore()
    {
        uiVisible = false;

        if (endLifeText != null)
            endLifeText.gameObject.SetActive(false);

        if (endLifeButton != null)
            endLifeButton.gameObject.SetActive(false);

        if (liveMoreButton != null)
            liveMoreButton.gameObject.SetActive(false);
    }

    IEnumerator EndLifeSequence()
    {
        isEnding = true;

        if (playerMovement != null)
            playerMovement.enabled = false;

        if (sky != null)
        {
            sky.lockFinalSunset = true;
            sky.fadeSunToBlack = true;
        }

        yield return new WaitForSeconds(6f);

        if (creditsController != null)
            creditsController.StartCredits();
    }
}
