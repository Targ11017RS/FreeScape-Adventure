using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class FreeScapeSens : MonoBehaviour
{
    public Slider SensSlider;

    float minVal = 0f;
    float maxVal = 100f;

    void Start()
    {
        SensSlider.maxValue = maxVal;
        SensSlider.minValue = minVal;
        SensSlider.wholeNumbers = true;
      
        SensSlider.onValueChanged.AddListener(SensitivityHandler);

        if (SceneManager.GetActiveScene().name == "Game")
        {
            SaveManager.Instance.Load();
            float savedSensitivity = SaveManager.Instance.CurrentData.sensitivity;

            if (FreeScapeRotation.Instance != null)
            {
                FreeScapeRotation.Instance.rotationSpeed = savedSensitivity;
            }

            SensSlider.value = savedSensitivity;
        }
        else if (FreeScapeRotation.Instance != null)
        {
            SensSlider.value = FreeScapeRotation.Instance.rotationSpeed;
        }
    }

    void SensitivityHandler(float val)
    {
        if (SaveManager.Instance != null)
        {
            SaveManager.Instance.CurrentData.sensitivity = val;
        }

        if (FreeScapeRotation.Instance != null)
        {
            FreeScapeRotation.Instance.rotationSpeed = val;
        }
    }
}
