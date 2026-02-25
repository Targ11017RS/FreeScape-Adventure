using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingManager : MonoBehaviour
{
    public Button StartBtn;
    public string nextSceneName;

    public static string sceneToLoad; // ✅ Static variable to pass data between scenes

    void Start()
    {
        StartBtn.onClick.AddListener(LoadScene);
    }

    public void LoadScene()
    {
        sceneToLoad = nextSceneName; // ✅ Set static variable
        SceneManager.LoadScene("Loading Scene",LoadSceneMode.Single); // Load the intermediate loading scene
    }
}
