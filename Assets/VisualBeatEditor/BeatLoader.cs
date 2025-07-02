using UnityEngine;
using UnityEngine.SceneManagement;
using System.IO;
using UnityEngine.Networking;
using System.Collections;

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

        StartCoroutine(LoadBeatData(mapNameToLoad));
    }

    IEnumerator LoadBeatData(string mapName)
    {
        string persistentPath = Path.Combine(Application.persistentDataPath, $"BeatBook/{mapName}.json");
        if (File.Exists(persistentPath))
        {
            string json = File.ReadAllText(persistentPath);
            LoadedData = JsonUtility.FromJson<BeatRecord>(json);
            Debug.Log($"✅ Loaded {LoadedData.beatHits.Count} beats from persistent storage.");
            yield break;
        }

        string streamingPath = Path.Combine(Application.streamingAssetsPath, $"BeatBook/{mapName}.json");

#if UNITY_ANDROID && !UNITY_EDITOR
        UnityWebRequest request = UnityWebRequest.Get(streamingPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string json = request.downloadHandler.text;
            LoadedData = JsonUtility.FromJson<BeatRecord>(json);
            Debug.Log($"✅ Loaded {LoadedData.beatHits.Count} beats from StreamingAssets.");
        }
        else
        {
            Debug.LogWarning($"❌ Failed to load from StreamingAssets: {request.error}");
            LoadedData = new BeatRecord();
        }
#else
        if (File.Exists(streamingPath))
        {
            string json = File.ReadAllText(streamingPath);
            LoadedData = JsonUtility.FromJson<BeatRecord>(json);
            Debug.Log($"✅ Loaded {LoadedData.beatHits.Count} beats from StreamingAssets.");
        }
        else
        {
            Debug.LogWarning($"⚠️ Beat JSON file for '{mapName}' not found in any location.");
            LoadedData = new BeatRecord();
        }
#endif
    }
}
