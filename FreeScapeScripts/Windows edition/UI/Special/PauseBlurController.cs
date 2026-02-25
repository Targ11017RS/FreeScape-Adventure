using UnityEngine;
//weight 0(imp)
using UnityEngine.Rendering;

public class PauseBlurController : MonoBehaviour
{
    public float smoothSpeed = 6f;

    Volume volume;
    float targetWeight;

    void Start()
    {
        volume = GetComponent<Volume>();
        volume.weight = 0f;
    }

    void Update()
    {
        volume.weight = Mathf.Lerp(
            volume.weight,
            targetWeight,
            Time.unscaledDeltaTime * smoothSpeed
        );
    }

    public void EnableBlur()
    {
        targetWeight = 1f;
    }

    public void DisableBlur()
    {
        targetWeight = 0f;
    }
}