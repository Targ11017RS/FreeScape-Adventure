using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FreeScapeStartLoader : MonoBehaviour
{
    public Button FreeScapeStartLoaderButton;
    public void Start() {
        FreeScapeStartLoaderButton.onClick.AddListener(FreeScapeStartSceneLoader);
    }
    void FreeScapeStartSceneLoader(){
        try
        {
            SceneManager.LoadScene("FreeScapeStart",LoadSceneMode.Single);
        }
        catch (System.Exception)
        {
            Debug.Log("Check FreeScapeStartLoader.cs.");
           
        }
    }
}