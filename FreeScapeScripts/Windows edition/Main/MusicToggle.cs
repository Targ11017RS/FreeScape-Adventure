using UnityEngine;
using UnityEngine.UI;

public class MusicToggle : MonoBehaviour
{
    public AudioSource Audio;
    public Toggle MusicHandler;
    bool isMusicPlaying = true;
    void Start() {
        MusicHandler.onValueChanged.AddListener(ToggleMusic);
    }

     public void ToggleMusic(bool value)
    {
        isMusicPlaying = value;
        if (isMusicPlaying)
        {
            Audio.Play();
        }else
        {
            Audio.Stop();
        }
    }





}