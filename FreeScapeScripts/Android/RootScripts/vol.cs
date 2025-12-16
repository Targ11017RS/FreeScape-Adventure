using System.Collections;
using UnityEngine;

public class vol : MonoBehaviour
{
    public static vol Instance;
    private AudioSource Music;

    public float minVolume = 0.0f;
    public float maxVolume = 1.0f;
    private float currentVol;
    private Coroutine fadeCoroutine;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        Music = GetComponent<AudioSource>();
        if (Music == null)
        {
            Debug.LogError("AudioSource component is missing.");
        }

        currentVol = PlayerPrefs.GetFloat("Music", maxVolume);
        SetMusicVolume(currentVol);
    }

    public void SetMusicVolume(float volume)
    {
        currentVol = Mathf.Clamp(volume, minVolume, maxVolume);
        if (Music != null)
        {
            Music.volume = currentVol;
        }
        PlayerPrefs.SetFloat("Music", currentVol);
        PlayerPrefs.Save();
    }

    public float GetMusicVolume() => currentVol;

    public void ToggleMusic(bool isMute)
    {
        if (Music != null)
        {
            Music.mute = isMute;
        }
    }

    public void Play(AudioClip music, float fadeDuration = 1f)
    {
        if (Music.clip == music && Music.isPlaying)
            return;

        if (fadeCoroutine != null)
            StopCoroutine(fadeCoroutine);

        fadeCoroutine = StartCoroutine(FadeInNewClip(music, fadeDuration));
    }

    public void Stop() => Music?.Stop();

    private IEnumerator FadeInNewClip(AudioClip newClip, float duration)
    {
        float startVol = Music.volume;

        // Fade out
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            Music.volume = Mathf.Lerp(startVol, 0f, t / duration);
            yield return null;
        }

        Music.Stop();
        Music.clip = newClip;
        Music.Play();

        // Fade in
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            Music.volume = Mathf.Lerp(0f, currentVol, t / duration);
            yield return null;
        }

        Music.volume = currentVol;
    }
}
