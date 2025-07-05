using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class BeatRecorder : MonoBehaviour
{
    public KeyCode attackKey = KeyCode.Space;
    public KeyCode spawnKey = KeyCode.K; // 新增，用于记录“发射类”节拍
    public KeyCode saveKey = KeyCode.S;

    [Tooltip("保存谱面用的名字，留空则使用当前场景名")]
    public string beatMapName = "";

    private BeatRecord beatRecord = new BeatRecord();

    void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            int currentBeat = Conductor.Instance.hit;
            beatRecord.attackBeats.Add(currentBeat);
            Debug.Log($"🗡️ Recorded ATTACK beat: {currentBeat}");
        }

        if (Input.GetKeyDown(spawnKey))
        {
            int currentBeat = Conductor.Instance.hit;
            beatRecord.spawnBeats.Add(currentBeat);
            Debug.Log($"🔫 Recorded SPAWN beat: {currentBeat}");
        }

        if (Input.GetKeyDown(saveKey))
        {
            string nameToUse = string.IsNullOrEmpty(beatMapName)
                ? UnityEngine.SceneManagement.SceneManager.GetActiveScene().name
                : beatMapName;

            SaveToJson(nameToUse);
        }
    }

    void SaveToJson(string mapName)
    {
        string folderPath = Path.Combine(Application.streamingAssetsPath, "BeatBook");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = Path.Combine(folderPath, $"{mapName}.json");
        string json = JsonUtility.ToJson(beatRecord, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"✅ Saved beat data to: {filePath}");
    }
}
