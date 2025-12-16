using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections;
public class PauseMenuController : MonoBehaviour
{
    [Header("Root Panels")]
    public GameObject MenuPanel;
    public GameObject SurityMenuPanel;
    public GameObject LeftAndroPanel;
    public GameObject RightAndroPanel;
    public GameObject BottomAndroPanel;
    public GameObject TopAndroPanel;

    [Header("Action Objects")]
    public Button ResumeButton;
    public Button MainMenuButton;
    public Button no;
    public Button PauseButton;

    [Header("Audio")]
    public AudioSource bgm;

    [Header("Fade Settings")]
    public float FadeDuration = 0.4f;

    private CanvasGroup pauseCanvasgroup;
    private bool isPaused = false;

    void Start()
    {
        pauseCanvasgroup = MenuPanel.GetComponent<CanvasGroup>();
        if (pauseCanvasgroup == null)
        {
            pauseCanvasgroup = TopAndroPanel.AddComponent<CanvasGroup>();
        }
        if (ResumeButton == null || MainMenuButton == null || no == null || PauseButton == null)
        {
            Debug.LogError("PauseMenuController: One or more Button references are not assigned in the Inspector!");
            enabled = false;
            return;
        }
        //default all menus must be disabled
        MenuPanel.SetActive(false);
        SurityMenuPanel.SetActive(false);
        TopAndroPanel.SetActive(true);
        LeftAndroPanel.SetActive(true);
        RightAndroPanel.SetActive(true);
        BottomAndroPanel.SetActive(true);

        ResumeButton.onClick.AddListener(Back);
        MainMenuButton.onClick.AddListener(OpenSurity);
        PauseButton.onClick.AddListener(DisplayPause);
        no.onClick.AddListener(DisableSurity);

        pauseCanvasgroup.alpha = 0f;

        // Make sure pause button is active at start
        if (TopAndroPanel != null)
            TopAndroPanel.SetActive(true);
    }
    
    void DisplayPause()
    {
        if (isPaused) return;
        StartCoroutine(FadeCanvasGroup(pauseCanvasgroup, 0f, 1f));
        isPaused = true;
        Time.timeScale = 0f;
        if (bgm != null)
            bgm.Pause();

        if (TopAndroPanel != null)
           TopAndroPanel.SetActive(false);

        LeftAndroPanel.SetActive(false);
        RightAndroPanel.SetActive(false);
        BottomAndroPanel.SetActive(false);
        MenuPanel.SetActive(true);
    }
    void Back()
    {
        if (!isPaused) return;

        StartCoroutine(FadeOutAndDeactivate());
        Time.timeScale = 1f;

        if (bgm != null)
            bgm.UnPause();

         if (TopAndroPanel != null)
            TopAndroPanel.SetActive(true);
       
        isPaused = false;
        
        LeftAndroPanel.SetActive(true);
        RightAndroPanel.SetActive(true);
        BottomAndroPanel.SetActive(true);
        MenuPanel.SetActive(false);
    }
    void OpenSurity()
    {
        SurityMenuPanel.SetActive(true);
    }
    void DisableSurity()
    {
        SurityMenuPanel.SetActive(false);
    }
    private IEnumerator FadeOutAndDeactivate()
    {
        yield return FadeCanvasGroup(pauseCanvasgroup, 1f, 0f);
        MenuPanel.SetActive(false);
        SurityMenuPanel.SetActive(false);
    }

    private IEnumerator FadeCanvasGroup(CanvasGroup cg, float begin, float end)
    {
        float elapsed = 0f;
        cg.alpha = begin;

        while (elapsed < FadeDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(begin, end, elapsed / FadeDuration);
            yield return null;
        }

        cg.alpha = end;
    }

}