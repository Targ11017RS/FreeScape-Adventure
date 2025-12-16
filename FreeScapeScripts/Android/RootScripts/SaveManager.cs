using UnityEngine;
using System.IO;
[System.Serializable]
public class SaveData
{
  public float sensitivity;
}


public class SaveManager : MonoBehaviour
{
    public static SaveManager Instance;

    private string savePath;

    public SaveData CurrentData = new SaveData();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            savePath = Application.persistentDataPath + "/settings.json";
            Load();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void Save()
    {
        string json = JsonUtility.ToJson(CurrentData, true);
        File.WriteAllText(savePath, json);
        Debug.Log("Saved to " + savePath);

    }

    public void Load()
    {
        if (File.Exists(savePath))
        {
            
            string json = File.ReadAllText(savePath);
            CurrentData = JsonUtility.FromJson<SaveData>(json);
            Debug.Log("Loaded settings.");
        }
        else
        {
            Debug.Log("No save file found, using defaults.");
            
            CurrentData.sensitivity = 5f;
        }
    }
}
