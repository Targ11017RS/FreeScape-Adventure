
using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class UISFXManipulator : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler
{
    public AudioSource audioSource;

    public AudioClip hoverClip;
    public AudioClip clickClip;

    private Dictionary<string, AudioClip> sfxMap;

    void Start()
    {
        // Initialize and assign sound types
        sfxMap = new Dictionary<string, AudioClip>
        {
            { "hover", hoverClip },
            { "click", clickClip }
        };
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        PlaySFX("hover");
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        PlaySFX("click");
    }

    void PlaySFX(string type)
    {
        if (sfxMap.ContainsKey(type) && sfxMap[type] != null)
        {
            audioSource.PlayOneShot(sfxMap[type]);
        }
        else
        {
            Debug.LogWarning("SFX not found or not assigned for: " + type);
        }
    }
}