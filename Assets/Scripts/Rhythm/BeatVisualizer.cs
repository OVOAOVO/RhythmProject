using UnityEngine;

public class BeatVisualizer : MonoBehaviour
{
    [Header("Material Control")]
    public Material targetMaterial;
    public string shaderPropertyName = "_BeatStrength";
    public float strengthMultiplier = 0.1f;
    public float beatDecay = 0.9f;

    private float beatStrength = 0f;
    private int lastBeat = -1;

    void LateUpdate()
    {
        if (Conductor.Instance == null || Conductor.Instance.CurrentState != Conductor.MusicState.Playing)
            return;

        float songPosition = Conductor.Instance.songPosition;
        float secPerBeat = Conductor.Instance.secPerBeat;

        int currentBeat = Mathf.FloorToInt(songPosition / secPerBeat);

        // 当进入新的一拍时，触发一次脉冲
        if (currentBeat != lastBeat)
        {
            lastBeat = currentBeat;
            beatStrength = 1.0f;
        }
        else
        {
            // 否则逐渐衰减
            beatStrength *= beatDecay;
        }

        if (targetMaterial != null)
        {
            targetMaterial.SetFloat(shaderPropertyName, beatStrength * strengthMultiplier);
        }
    }
}
