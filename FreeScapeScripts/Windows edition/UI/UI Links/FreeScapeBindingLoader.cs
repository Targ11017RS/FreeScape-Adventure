using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class FreeScapeBindingLoader : MonoBehaviour
{
   public GameObject FreeScapeBindingLoaderButtonParent;
    public Button FreeScapeBindingLoaderButton;
    public void Start() {
        FreeScapeBindingLoaderButtonParent.SetActive(true);
        if (Application.platform == RuntimePlatform.Android)
        {
        FreeScapeBindingLoaderButtonParent.SetActive(false);
        
        FreeScapeBindingLoaderButton.onClick.AddListener(FreeScapeBindingSceneLoader);
        }
    }
    void FreeScapeBindingSceneLoader(){
        try
        {
            SceneManager.LoadScene("FreeScapeBindings",LoadSceneMode.Single);
        }
        catch (System.Exception)
        {
            Debug.Log("Check FreeScapeBindingLoader.cs.");
            
        }
    }
    
}