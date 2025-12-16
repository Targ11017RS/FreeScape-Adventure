using UnityEngine;
using UnityEngine.UI;

public class CameraAssigner : MonoBehaviour
{
    public Camera MinimisedImage;
    public Camera ZoomedMap;
    public RawImage Minimap;

    private bool isZoomed = false;

    void Start()
    {
        // Start with minimap camera enabled
        MinimisedImage.enabled = true;
        ZoomedMap.enabled = false;

        // Add click listener to minimap UI
        Button minMapButton = Minimap.gameObject.AddComponent<Button>();
        minMapButton.onClick.AddListener(SwitcherCam);
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            SwitcherCam();
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            GetBack();
        }
    }

    void SwitcherCam()
    {
        isZoomed = true;
        MinimisedImage.enabled = false;
        ZoomedMap.enabled = true;
    }

    void GetBack()
    {
        if (!isZoomed) return;

        ZoomedMap.enabled = false;
        MinimisedImage.enabled = true;
        isZoomed = false;
    }
}
