using UnityEngine;
using System.Collections;

[RequireComponent(typeof(AudioSource))]
public class EnvironmentalAudioController : MonoBehaviour
{
    [Header("Terrain Layers")]
    public LayerMask terrainLayer; // e.g. grass, gravel

    [Header("Audio Clips")]
    public AudioClip windAmbient;   // always low-volume
    public AudioClip terrainMusic;  // grass/gravel music
    public AudioClip waterMusic;    // water music

    [Header("Settings")]
    public float fadeDuration = 2f;      // crossfade duration
    public float maxDistanceVolume = 1f; // max volume near water
    public float minDistanceVolume = 0.2f; // min volume far from water
    public float waterDetectionRadius = 10f; // radius to detect water planes

    private AudioSource audioSource;
    private AudioClip currentClip;

    private Transform player;
    private GameObject nearestWater = null;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.loop = true;
        audioSource.volume = minDistanceVolume;

        player = Camera.main.transform; // assuming main camera represents player

        // Start with wind ambient
        PlayClip(windAmbient, true);
    }

    void Update()
    {
        UpdateEnvironmentMusic();
        UpdateDistanceVolume();
    }

    void UpdateEnvironmentMusic()
    {
        Ray ray = new Ray(player.position + Vector3.up * 0.5f, Vector3.down);
        if (Physics.Raycast(ray, out RaycastHit hit, 5f))
        {
            AudioClip targetClip = windAmbient;
            nearestWater = null;

            // Check terrain layer
            if (((1 << hit.collider.gameObject.layer) & terrainLayer) != 0)
            {
                targetClip = terrainMusic;
            }

            // Detect water planes nearby (tag = "Water")
            Collider[] waterHits = Physics.OverlapSphere(player.position, waterDetectionRadius);
            foreach (var col in waterHits)
            {
                if (col.CompareTag("Water"))
                {
                    targetClip = waterMusic;
                    nearestWater = col.gameObject;
                    break;
                }
            }

            // If clip changed, crossfade
            if (currentClip != targetClip)
            {
                StartCoroutine(FadeToClip(targetClip));
            }
        }
    }

    void UpdateDistanceVolume()
    {
        if (currentClip == waterMusic && nearestWater != null)
        {
            float distance = Vector3.Distance(player.position, nearestWater.transform.position);
            float volume = Mathf.Lerp(maxDistanceVolume, minDistanceVolume, distance / waterDetectionRadius);
            audioSource.volume = Mathf.Clamp(volume, minDistanceVolume, maxDistanceVolume);
        }
        else
        {
            // Terrain or wind, normal smooth volume
            audioSource.volume = Mathf.Lerp(audioSource.volume, maxDistanceVolume * 0.8f, Time.deltaTime);
        }
    }

    void PlayClip(AudioClip clip, bool immediate = false)
    {
        currentClip = clip;
        if (immediate)
        {
            audioSource.clip = clip;
            audioSource.volume = maxDistanceVolume;
            audioSource.Play();
        }
        else
        {
            StartCoroutine(FadeToClip(clip));
        }
    }

    IEnumerator FadeToClip(AudioClip newClip)
    {
        float t = 0f;
        float startVolume = audioSource.volume;

        // Fade out current clip
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            audioSource.volume = Mathf.Lerp(startVolume, 0f, t);
            yield return null;
        }

        // Switch clip
        audioSource.clip = newClip;
        audioSource.Play();
        currentClip = newClip;

        // Fade in new clip
        t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;
            audioSource.volume = Mathf.Lerp(0f, maxDistanceVolume, t);
            yield return null;
        }
    }
}
