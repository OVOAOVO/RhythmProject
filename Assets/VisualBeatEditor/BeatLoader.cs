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
        string filePath;

    #if UNITY_EDITOR
        // 编辑器运行时：直接用 Assets 目录
        filePath = Path.Combine(Application.dataPath, "BeatBook/beat_data.json");
    #else
        // 构建后：从 .exe 同目录读取
        filePath = Path.Combine(Application.dataPath, "../beat_data.json");
    #endif

        Debug.Log($"🧾 Attempting to load beat data from: {filePath}");

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
