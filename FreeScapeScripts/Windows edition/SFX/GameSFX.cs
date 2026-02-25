using UnityEngine;

public class GameSFX : MonoBehaviour
{
    public static GameSFX Instance { get; private set; }

    [Header("References")]
    public SkyBoxManipulator skyBoxManipulator;

    [Header("Looping Day Cycle SFX")]
    public AudioClip morningSFX;
    public AudioClip daySFX;
    public AudioClip eveningSFX;
    public AudioClip nightSFX;

    private SkyBoxManipulator.DayPhase lastPhase;

    private void Awake()
    {
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
        if (skyBoxManipulator == null)
            skyBoxManipulator = FindObjectOfType<SkyBoxManipulator>();
    }

    private void Update()
    {
        if (skyBoxManipulator == null || vol.Instance == null) return;

        SkyBoxManipulator.DayPhase currentPhase = skyBoxManipulator.CurrentPhase;

        if (currentPhase != lastPhase)
        {
            lastPhase = currentPhase;
            PlayPhaseSFX(currentPhase);
        }
    }

   void PlayPhaseSFX(SkyBoxManipulator.DayPhase phase)
{
    AudioClip clip = null;

    switch (phase)
    {
        case SkyBoxManipulator.DayPhase.Morning:
            clip = morningSFX;
            break;
        case SkyBoxManipulator.DayPhase.Day:
            clip = daySFX;
            break;
        case SkyBoxManipulator.DayPhase.Evening:
            clip = eveningSFX;
            break;
        case SkyBoxManipulator.DayPhase.Night:
            clip = nightSFX;
            break;
    }

    if (clip != null)
    {
        vol.Instance.Play(clip, fadeDuration: 1.5f); // Fade between clips
    }
}

       
    
}
