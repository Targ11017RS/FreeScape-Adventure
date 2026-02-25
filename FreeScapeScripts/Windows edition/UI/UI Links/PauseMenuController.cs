using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    [Header("Root Panels")]
    public GameObject MenuPanel;
    public GameObject SurityMenuPanel;
    public GameObject BottomAndroPanel;
    public GameObject TopAndroPanel;

    [Header("Action Objects")]
    public Button ResumeButton;
    public Button YesButton;// YES button
    public Button no;               // NO button
    public Button PauseButton;
    public Button MainMenuButton;

    [Header("Audio")]
    public AudioSource bgm;

    [Header("Fade Settings")]
    public float FadeDuration = 0.4f;

    private CanvasGroup pauseCanvasgroup;
    private bool isPaused = false;
    private bool isTransitioning = false;

    [Header("Loading")]
    public LoadingManager loadingManager;

    [Header("Cursor")]
    public CursorManager CursorManagerGO;



    public PauseBlurController pauseBlur;

    void Start()
    {
        pauseCanvasgroup = MenuPanel.GetComponent<CanvasGroup>();
        if (pauseCanvasgroup == null)
            pauseCanvasgroup = MenuPanel.AddComponent<CanvasGroup>();

        pauseCanvasgroup.interactable = true;
        pauseCanvasgroup.blocksRaycasts = true;
        pauseCanvasgroup.alpha = 0f;

        MenuPanel.SetActive(false);
        SurityMenuPanel.SetActive(false);

        ResumeButton.onClick.AddListener(Resume);
        
        PauseButton.onClick.AddListener(Pause);
        MainMenuButton.onClick.AddListener(OpenSurity);  // THIS OPENS SURITY
        // YES & NO inside Surity
        YesButton.onClick.AddListener(LoadMainMenu);
        no.onClick.AddListener(DisableSurity);
    }

    void Update()
    {
        if (isTransitioning) return;

        // ───── Surity menu controls ─────
        if (SurityMenuPanel.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                LoadMainMenu(); // YES
            }
            else if (Input.GetKeyDown(KeyCode.Escape))
            {
                DisableSurity(); // NO
            }
            return;
        }

        // ───── Pause / Resume ─────
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (SurityMenuPanel.activeSelf)
            {
                DisableSurity();
            }
            else if (MenuPanel.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }


       if (MenuPanel.activeSelf && !SurityMenuPanel.activeSelf && Input.GetKeyDown(KeyCode.E))
        {
            OpenSurity();
        }

    }

    void Pause()
    {
        if (isPaused) return;

        isPaused = true;
        isTransitioning = true;

        MenuPanel.SetActive(true);
        StartCoroutine(PauseRoutine());
    }

    void Resume()
    {
        if (!isPaused) return;

        isPaused = false;
        isTransitioning = true;
        StartCoroutine(ResumeRoutine());
    }

    IEnumerator PauseRoutine()
 {
    StartCoroutine(FadeCanvasGroup(0f, 1f));

    Time.timeScale = 0f;

    if (bgm) bgm.Pause();
    if (pauseBlur) pauseBlur.EnableBlur();

    TopAndroPanel?.SetActive(false);
    BottomAndroPanel?.SetActive(false);

    CursorManagerGO?.ShowCursor(); // SHOW cursor

    yield return new WaitForSecondsRealtime(FadeDuration);
    isTransitioning = false;
 }


    IEnumerator ResumeRoutine()
{
    yield return FadeCanvasGroup(1f, 0f);

    MenuPanel.SetActive(false);
    SurityMenuPanel.SetActive(false);

    Time.timeScale = 1f;

    if (bgm) bgm.UnPause();
    if (pauseBlur) pauseBlur.DisableBlur();

    TopAndroPanel?.SetActive(true);
    BottomAndroPanel?.SetActive(true);

    CursorManagerGO?.HideCursor(); // HIDE cursor

    isTransitioning = false;
}


    void OpenSurity()
{
    SurityMenuPanel.SetActive(true);
    CursorManagerGO?.ShowCursor();
}

    void DisableSurity()
    {
        SurityMenuPanel.SetActive(false);
    }
    void LoadMainMenu()
{
    Time.timeScale = 1f;
    loadingManager.LoadScene();
}


    IEnumerator FadeCanvasGroup(float from, float to)
    {
        float t = 0f;
        while (t < FadeDuration)
        {
            t += Time.unscaledDeltaTime;
            pauseCanvasgroup.alpha = Mathf.Lerp(from, to, t / FadeDuration);
            yield return null;
        }
        pauseCanvasgroup.alpha = to;
    }
}
