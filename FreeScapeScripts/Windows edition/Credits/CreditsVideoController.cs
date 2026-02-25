using UnityEngine;
using UnityEngine.Video;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class CreditsVideoController : MonoBehaviour
{
    [Header("Video & UI")]
    public VideoPlayer videoPlayer;
    public CanvasGroup videoCanvasGroup;
    public TMP_Text finalText;

    [Header("Sky")]
    public SkyBoxManipulator sky;

    [Header("Audio")]
    public AudioSource musicSource;
    public AudioClip creditsMusic;

    [Header("Fade Settings")]
    public float fadeDuration = 2f;
    public float finalTextDelay = 1.5f;

    void Start()
    {
        // FULLY disable at start
        if (videoCanvasGroup != null)
        {
            videoCanvasGroup.alpha = 0f;
            videoCanvasGroup.gameObject.SetActive(false);
        }

        if (finalText != null)
            finalText.gameObject.SetActive(false);
    }

    public void StartCredits()
    {
        if (videoCanvasGroup != null)
        {
            videoCanvasGroup.gameObject.SetActive(true);
            videoCanvasGroup.alpha = 1f;
        }

        if (finalText != null)
            finalText.gameObject.SetActive(false);

        if (videoPlayer != null)
        {
            videoPlayer.loopPointReached += OnVideoEnd;
            videoPlayer.Play();
        }

        if (musicSource != null && creditsMusic != null)
        {
            musicSource.Stop();
            musicSource.clip = creditsMusic;
            musicSource.volume = 1f;
            musicSource.Play();
        }
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        StartCoroutine(FadeOutVideo());
    }

    IEnumerator FadeOutVideo()
    {
        if (sky != null)
            sky.fadeSunToBlack = true;

        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / fadeDuration;

            if (videoCanvasGroup != null)
                videoCanvasGroup.alpha = Mathf.Lerp(1f, 0f, t);

            if (musicSource != null)
                musicSource.volume = Mathf.Lerp(1f, 0f, t);

            yield return null;
        }

        if (videoCanvasGroup != null)
            videoCanvasGroup.gameObject.SetActive(false);

        yield return new WaitForSeconds(finalTextDelay);

        if (finalText != null)
        {
            finalText.text = "Thank you for staying.";
            finalText.gameObject.SetActive(true);
        }
    }
}
