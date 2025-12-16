using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class FastSceneLoader : MonoBehaviour
{
   public static FastSceneLoader Instance;
   private void Awake()
    {
        // Singleton Setup
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);  // Keep across scenes if necessary
        }
        else
        {
            Destroy(gameObject);  // Destroy duplicate
        }
    }

    // Call this method with the name of the scene to load
    public void LoadSceneFast(string sceneName)
    {
        if (string.IsNullOrEmpty(sceneName))
        {
            Debug.LogError("Scene name is invalid or empty.");
            return;
        }
        else
        {
            SceneManager.LoadScene(sceneName,LoadSceneMode.Single);
        }
      
    }

  
}
