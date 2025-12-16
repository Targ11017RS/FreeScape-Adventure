using UnityEngine;

public class AndroLandscape : MonoBehaviour
{
    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        #if UNITY_ANDROID && !UNITY_EDITOR 
        Screen.orientation = ScreenOrientation.Landscape;
        Screen.autorotateToPotrait = false;
        Screen.autorotateToPotraitUpsideDown = false;
        Screen.autorotateToLandscapeLeft = true;
        Screen.autorotateToLandscapeRight = true;
        Screen.orientation = ScreenOrientation.AutoRotation;
    }
    try
    {
        AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3D.Player.UnityPlayer");
        AndroidJavaObject activity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
        const int SCREEN_ORIENTATION_LANDSCAPE = 0;
        activity.Call("setRequestedOrientation" , SCREEN_ORIENTATION_LANDSCAPE);
        AndroidJavaObject window = activity.Call<AndroidJavaObject>("getWindow");
        int FLAG_FULLSCREEN = 1024;
        int FLAG_LAYOUT_NO_LIMITS = 512;
        window.Call("addFlags" , FLAG_FULLSCREEN | FLAG_LAYOUT_NO_LIMITS);
        AndroidJavaObject decorView = activity.Call<AndroidJavaObject>("getDecorView");
        int SYSTEM_UI_FLAG_FULLSCREEN = 4;
        decorView.Call("setSystemUiVisibility" , SYSTEM_UI_FLAG_FULLSCREEN);
    }
    catch (System.Exception e)
    {
      Debug.Log("failed to lock" + e.Message);
    }
    #else
    Debug.Log("AndroLandscape.cs - failed/ skipped - no android device found");
    #endif
}}
