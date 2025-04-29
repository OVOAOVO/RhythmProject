using UnityEngine;
using System.IO;

public class BeatLoader : MonoBehaviour
{
    public static BeatRecord LoadedData { get; private set; }

    void Awake()
    {
        LoadFromJson();
    }

    void LoadFromJson()
    {
        string filePath = Path.Combine(Application.dataPath, "BeatBook/beat_data.json");
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LoadedData = JsonUtility.FromJson<BeatRecord>(json);
            Debug.Log($"✅ Loaded {LoadedData.beatHits.Count} beats.");
        }
        else
        {
            Debug.LogWarning("⚠️ Beat JSON file not found.");
            LoadedData = new BeatRecord();
        }
    }
}
