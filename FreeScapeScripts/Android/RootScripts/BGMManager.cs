using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class BGMManager : MonoBehaviour
{
    public string SceneName;
    private static BGMManager CurrentBGM;
    void Awake() {
      if (CurrentBGM != null && CurrentBGM.SceneName == SceneName)
      {
        Destroy(gameObject);
        return;
      }  
      if (CurrentBGM != null&& CurrentBGM != this)
      {
        Destroy(CurrentBGM.gameObject);
      }
      CurrentBGM = this;
      DontDestroyOnLoad(gameObject);
    }
    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;

    }
    /// <summary>
    /// This function is called when the behaviour becomes disabled or inactive.
    /// </summary>
    void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }
    void OnSceneLoaded(Scene scene,LoadSceneMode mode)
    {
        Destroy(gameObject);
    }
}