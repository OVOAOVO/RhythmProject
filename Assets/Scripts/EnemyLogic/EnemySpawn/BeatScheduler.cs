// BeatScheduler.cs
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BeatScheduler
{
    private List<int> scheduledBeats;
    private Dictionary<int, int> spawnedBeatCounts = new Dictionary<int, int>();
    private int spawnAdvanceBeats = 6;

    public void Init(Conductor conductor)
    {
        if (BeatLoader.LoadedData != null)
            scheduledBeats = new List<int>(BeatLoader.LoadedData.spawnBeats);
        else
            scheduledBeats = new List<int>();

        if (conductor != null && conductor.songBPM > 0)
        {
            float beatsPerSecond = conductor.songBPM / 60f;
            float travelTime = 2.5f / 20f;
            spawnAdvanceBeats = Mathf.CeilToInt(travelTime * beatsPerSecond);
        }
    }

    public bool IsEnemySpawnBeat(int currentHit)
    {
        foreach (int beat in scheduledBeats)
        {
            int fireBeat = beat - spawnAdvanceBeats;
            if (currentHit == fireBeat)
            {
                if (!spawnedBeatCounts.ContainsKey(beat))
                    spawnedBeatCounts[beat] = 0;

                int total = scheduledBeats.Count(b => b == beat);
                if (spawnedBeatCounts[beat] < total)
                {
                    spawnedBeatCounts[beat]++;
                    return true;
                }
            }
        }
        return false;
    }
}