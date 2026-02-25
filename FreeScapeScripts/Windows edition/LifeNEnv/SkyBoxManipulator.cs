using UnityEngine;

public class SkyBoxManipulator : MonoBehaviour
{
    public enum DayPhase { Morning, Day, Evening, Night }
    public DayPhase CurrentPhase { get; private set; }

    [Header("Skyboxes")]
    public Material morningSkybox;
    public Material daySkybox;
    public Material eveningSkybox;
    public Material nightSkybox;

    [Header("Lights")]
    public Light sunLight;
    public Light moonLight;

    [Header("Cycle")]
    public float fullDayDurationInMinutes = 2f; // faster for gameplay
    public Gradient sunColorOverTime;
    public AnimationCurve sunTemperatureCurve;
    public float maxSunTemperature = 6500f;

    [Header("Life Sync")]
    public int currentAge = 0;

    [Header("Ending")]
    public bool lockFinalSunset = false;
    public bool fadeSunToBlack = false;
    public float sunsetLockSpeed = 0.2f;
    public float sunFadeSpeed = 0.2f;

    float cycleTime = 0f;
    float cycleDuration;
    float timeMultiplier = 1f;

    void Start()
    {
        cycleDuration = fullDayDurationInMinutes * 60f;
    }

    void Update()
    {
        if (lockFinalSunset)
        {
            LockToFinalSunset();

            if (fadeSunToBlack)
                FadeSunOut();

            return;
        }

        UpdateTimeMultiplier();

        cycleTime += Time.deltaTime * timeMultiplier;
        float percent = (cycleTime % cycleDuration) / cycleDuration;

        UpdateLighting(percent);
        UpdateSkybox(percent);
    }

    void UpdateTimeMultiplier()
    {
        float normalizedAge = Mathf.Clamp01(currentAge / 90f);

        // Time feels faster as life progresses
        float targetSpeed = Mathf.Lerp(0.6f, 2.8f, normalizedAge);

        timeMultiplier = Mathf.Lerp(timeMultiplier, targetSpeed, Time.deltaTime * 0.4f);
    }

    void LockToFinalSunset()
    {
        float sunsetPercent = 0.65f;

        UpdateLighting(sunsetPercent);
        RenderSettings.skybox = eveningSkybox;
        CurrentPhase = DayPhase.Evening;

        sunLight.transform.rotation = Quaternion.Lerp(
            sunLight.transform.rotation,
            Quaternion.Euler(8f, 0f, 0f),
            Time.deltaTime * sunsetLockSpeed
        );

        sunLight.intensity = Mathf.Lerp(
            sunLight.intensity,
            0.3f,
            Time.deltaTime * sunsetLockSpeed
        );

        moonLight.intensity = 0f;
    }

    void FadeSunOut()
    {
        sunLight.intensity = Mathf.Lerp(
            sunLight.intensity,
            0f,
            Time.deltaTime * sunFadeSpeed
        );

        RenderSettings.ambientIntensity = Mathf.Lerp(
            RenderSettings.ambientIntensity,
            0f,
            Time.deltaTime * sunFadeSpeed
        );
    }

    void UpdateLighting(float p)
    {
        float angle = Mathf.Lerp(-90, 90, Mathf.Sin(p * Mathf.PI));
        sunLight.transform.rotation = Quaternion.Euler(angle, 0, 0);
        moonLight.transform.rotation = Quaternion.Euler(-angle, 180, 0);

        sunLight.intensity = Mathf.Clamp01(Mathf.Cos(p * Mathf.PI) * 1.5f);
        moonLight.intensity = 1f - sunLight.intensity;

        sunLight.color = sunColorOverTime.Evaluate(p);
        sunLight.colorTemperature =
            Mathf.Max(sunTemperatureCurve.Evaluate(p) * maxSunTemperature, 2000f);
    }

    void UpdateSkybox(float p)
    {
        if (p < 0.25f)
        {
            RenderSettings.skybox = morningSkybox;
            CurrentPhase = DayPhase.Morning;
        }
        else if (p < 0.5f)
        {
            RenderSettings.skybox = daySkybox;
            CurrentPhase = DayPhase.Day;
        }
        else if (p < 0.75f)
        {
            RenderSettings.skybox = eveningSkybox;
            CurrentPhase = DayPhase.Evening;
        }
        else
        {
            RenderSettings.skybox = nightSkybox;
            CurrentPhase = DayPhase.Night;
        }
    }
}
