using UnityEngine;
using System.Collections.Generic;
using System.IO;

public class BeatRecorder : MonoBehaviour
{
    public KeyCode recordKey = KeyCode.Space;
    public KeyCode saveKey = KeyCode.S;

    private BeatRecord beatRecord = new BeatRecord();

    void Update()
    {
        if (Input.GetKeyDown(recordKey))
        {
            int currentBeat = Conductor.Instance.hit;
            beatRecord.beatHits.Add(currentBeat);
            Debug.Log($"Recorded beat: {currentBeat}");
        }

        if (Input.GetKeyDown(saveKey))
        {
            SaveToJson();
        }
    }

    void SaveToJson()
    {
        string folderPath = Path.Combine(Application.dataPath, "BeatBook");
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string filePath = Path.Combine(folderPath, "beat_data.json");
        string json = JsonUtility.ToJson(beatRecord, true);
        File.WriteAllText(filePath, json);
        Debug.Log($"✅ Saved beat data to: {filePath}");
    }
}
