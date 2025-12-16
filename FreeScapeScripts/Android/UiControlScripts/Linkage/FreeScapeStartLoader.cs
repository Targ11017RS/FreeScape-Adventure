using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FreeScapeStartLoader : MonoBehaviour
{
    public Button FreeScapeStartLoaderButton;

    private void Start()
    {
        if (FreeScapeStartLoaderButton != null)
        {
            FreeScapeStartLoaderButton.onClick.AddListener(FreeScapeStartSceneLoader);
        }
        else
        {
            Debug.LogError("FreeScapeStartLoaderButton is not assigned in the Inspector!");
        }
    }

    void FreeScapeStartSceneLoader()
    {
        Debug.Log("Trying to load scene: FreeScapeStart");

        try
        {
            SceneManager.LoadScene("FreeScapeGame", LoadSceneMode.Single);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Scene loading failed. Check if the scene 'FreeScapeStart' is added to Build Settings.");
            Debug.LogException(ex);
        }
    }
}
