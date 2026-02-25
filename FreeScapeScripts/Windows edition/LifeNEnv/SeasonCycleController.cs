using UnityEngine;
using System.Collections;

public class SeasonCycleController : MonoBehaviour
{
    public enum Season { Spring, Summer, Autumn, Winter, Rainy }
    public Season currentSeason { get; private set; }

    [Header("Reference")]
    public SkyBoxManipulator sky;

    [Header("Season Duration (Minutes)")]
    public float seasonDurationInMinutes = 3f;

    [Header("Transition Settings")]
    public float transitionDuration = 4f;

    [Header("Rain System")]
    public ParticleSystem rainParticles;

    [Header("Spring Skyboxes")]
    public Material springMorning, springDay, springEvening, springNight;

    [Header("Summer Skyboxes")]
    public Material summerMorning, summerDay, summerEvening, summerNight;

    [Header("Autumn Skyboxes")]
    public Material autumnMorning, autumnDay, autumnEvening, autumnNight;

    [Header("Winter Skyboxes")]
    public Material winterMorning, winterDay, winterEvening, winterNight;

    [Header("Rainy Skyboxes")]
    public Material rainyMorning, rainyDay, rainyEvening, rainyNight;

    float seasonTimer;
    float seasonDuration;
    bool isTransitioning = false;

    void Start()
    {
        seasonDuration = seasonDurationInMinutes * 60f;
        currentSeason = Season.Spring;
        ApplySeasonInstant();
    }

    void Update()
    {
        if (sky == null || isTransitioning) return;

        seasonTimer += Time.deltaTime;

        if (seasonTimer >= seasonDuration)
        {
            seasonTimer = 0f;
            StartCoroutine(TransitionSeason());
        }
    }

    IEnumerator TransitionSeason()
    {
        isTransitioning = true;

        Season nextSeason = (Season)(((int)currentSeason + 1) % 5);

        float t = 0f;

        // Fade lighting darker before swap
        while (t < 1f)
        {
            t += Time.deltaTime / transitionDuration;
            if (sky.sunLight != null)
                sky.sunLight.intensity = Mathf.Lerp(1f, 0.2f, t);

            yield return null;
        }

        currentSeason = nextSeason;
        ApplySeasonInstant();

        t = 0f;

        // Fade lighting back in
        while (t < 1f)
        {
            t += Time.deltaTime / transitionDuration;
            if (sky.sunLight != null)
                sky.sunLight.intensity = Mathf.Lerp(0.2f, 1f, t);

            yield return null;
        }

        isTransitioning = false;
    }

    void ApplySeasonInstant()
    {
        switch (currentSeason)
        {
            case Season.Spring:
                ApplySkyboxSet(springMorning, springDay, springEvening, springNight);
                SetRain(false);
                break;

            case Season.Summer:
                ApplySkyboxSet(summerMorning, summerDay, summerEvening, summerNight);
                SetRain(false);
                break;

            case Season.Autumn:
                ApplySkyboxSet(autumnMorning, autumnDay, autumnEvening, autumnNight);
                SetRain(false);
                break;

            case Season.Winter:
                ApplySkyboxSet(winterMorning, winterDay, winterEvening, winterNight);
                SetRain(false);
                break;

            case Season.Rainy:
                ApplySkyboxSet(rainyMorning, rainyDay, rainyEvening, rainyNight);
                SetRain(true);
                break;
        }
    }

    void ApplySkyboxSet(Material morning, Material day, Material evening, Material night)
    {
        sky.morningSkybox = morning;
        sky.daySkybox = day;
        sky.eveningSkybox = evening;
        sky.nightSkybox = night;
    }

    void SetRain(bool state)
    {
        if (rainParticles == null) return;

        if (state)
        {
            rainParticles.gameObject.SetActive(true);
            rainParticles.Play();
        }
        else
        {
            rainParticles.Stop();
            rainParticles.gameObject.SetActive(false);
        }
    }
}
