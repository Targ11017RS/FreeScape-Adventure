using UnityEngine;
using UnityEngine.SceneManagement;
public class GameFocusCheck : MonoBehaviour
{
    private bool isGameFocused = true;
    void OnApplicationFocus(bool focusStatus) {
        if (focusStatus != isGameFocused)
        {
            isGameFocused = focusStatus;
            if (!focusStatus)
            {
                RestartGame();
            }
        }
    }
    void RestartGame(){
        Scene startScene = SceneManager.GetSceneByName("start");
        SceneManager.LoadScene(startScene.name);
    }
}