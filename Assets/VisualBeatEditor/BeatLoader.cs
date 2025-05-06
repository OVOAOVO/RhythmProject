using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;

public class BeatLoader : MonoBehaviour
{
    public static BeatRecord LoadedData { get; private set; }

    [Tooltip("若非空，优先加载此谱面名；为空则根据场景名加载")]
    public string beatMapName = ""; // 可手动设置，或留空以自动使用场景名

    void Awake()
    {
        string mapNameToLoad = string.IsNullOrEmpty(beatMapName)
            ? SceneManager.GetActiveScene().name
            : beatMapName;

        LoadFromJson(mapNameToLoad);
    }

    public void LoadFromJson(string mapName)
    {
        string filePath;

        filePath = Path.Combine(Application.dataPath, $"BeatBook/{mapName}.json");

        Debug.Log($"🧾 Attempting to load beat data: {filePath}");

        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            LoadedData = JsonUtility.FromJson<BeatRecord>(json);
            Debug.Log($"✅ Loaded {LoadedData.beatHits.Count} beats from '{mapName}'.");
        }
        else
        {
            Debug.LogWarning($"⚠️ Beat JSON file for '{mapName}' not found.");
            LoadedData = new BeatRecord();
        }
    }
}
