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

    [Header("Cycle Settings")]
    public float fullDayDurationInMinutes = 24f;
    public Gradient sunColorOverTime;
    public AnimationCurve sunTemperatureCurve;
    public float maxSunTemperature = 6500f;

    [Header("Player")]
    public Transform player;
    public float sunDamageInterval = 1f;
    public int sunDamageAmount = 5;

    private float cycleTime = 0f;
    private float damageTimer = 0f;
    private bool isPermanentNight = false;
    private float cycleDurationSeconds;
    private float halfCycle;

    private void Start()
    {
        cycleDurationSeconds = fullDayDurationInMinutes * 60f;
        halfCycle = cycleDurationSeconds / 2f;

        if (!player)
            player = GameObject.FindGameObjectWithTag("Player")?.transform;
    }

    private void Update()
    {
        if (isPermanentNight)
        {
            RenderSettings.skybox = nightSkybox;
            sunLight.enabled = false;
            moonLight.enabled = true;
            RotateLight(moonLight, Time.deltaTime * 5f);
            return;
        }

        cycleTime += Time.deltaTime;
        float dayPercent = cycleTime / cycleDurationSeconds;

        UpdateLighting(dayPercent);
        UpdateSkybox(dayPercent);
        CheckSunDamage(dayPercent);

        if (cycleTime >= cycleDurationSeconds)
            isPermanentNight = true;
    }

    void UpdateLighting(float percent)
    {
        float sunAngle = Mathf.Lerp(-90, 90, Mathf.Sin(percent * Mathf.PI));
        sunLight.transform.rotation = Quaternion.Euler(sunAngle, 0f, 0f);
        moonLight.transform.rotation = Quaternion.Euler(-sunAngle, 180f, 0f);

        sunLight.intensity = Mathf.Clamp01(Mathf.Cos(percent * Mathf.PI) * 1.5f);
        moonLight.intensity = 1f - sunLight.intensity;

        sunLight.color = sunColorOverTime.Evaluate(percent);
        float temp = sunTemperatureCurve.Evaluate(percent) * maxSunTemperature;
        sunLight.colorTemperature = Mathf.Max(temp, 2000f);
    }

    void UpdateSkybox(float percent)
    {
        if (percent < 0.25f)
        {
            RenderSettings.skybox = morningSkybox;
            CurrentPhase = DayPhase.Morning;
        }
        else if (percent < 0.5f)
        {
            RenderSettings.skybox = daySkybox;
            CurrentPhase = DayPhase.Day;
        }
        else if (percent < 0.75f)
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

    void CheckSunDamage(float percent)
    {
        if (percent > 0.5f && percent < 0.75f)
        {
            if (IsPlayerUnderSun())
            {
                damageTimer += Time.deltaTime;
                if (damageTimer >= sunDamageInterval)
                {
                    damageTimer = 0f;
                    player.GetComponent<FreeScapeHealthScript>()?.TakeDamage(sunDamageAmount);
                }
            }
        }
    }

    bool IsPlayerUnderSun()
    {
        Vector3 direction = sunLight.transform.forward;
        Ray ray = new Ray(player.position + Vector3.up, -direction);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            return hit.collider.gameObject == player.gameObject;
        }
        return true;
    }

    void RotateLight(Light lightSource, float rotationSpeed)
    {
        lightSource.transform.Rotate(Vector3.right * rotationSpeed);
    }
}
