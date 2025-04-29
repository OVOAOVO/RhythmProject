using UnityEngine;
using System.IO;

public class BeatLoader : MonoBehaviour
{
    public BeatRecord loadedData;

    void Start()
    {
        LoadFromJson();
    }

    void LoadFromJson()
    {
        string filePath = Path.Combine(Application.dataPath, "BeatBook/beat_data.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            loadedData = JsonUtility.FromJson<BeatRecord>(json);
            Debug.Log($"✅ Loaded {loadedData.beatHits.Count} beats.");
        }
        else
        {
            Debug.LogWarning("⚠️ Beat JSON file not found.");
        }
    }
}
