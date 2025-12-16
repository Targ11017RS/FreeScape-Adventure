using UnityEngine;
using UnityEngine.UI;

public class FreeScapeQuit : MonoBehaviour
{
    public Button FreeScapeQuitButton;
    public void Start() {
        FreeScapeQuitButton.onClick.AddListener(FreeScapeGameQuit);
    }
    void FreeScapeGameQuit(){
        try
        {
           Application.Quit();
        }
        catch (System.Exception)
        {
            Debug.Log("Check FreeScapeQuit.cs.");
          
        }
    }
}