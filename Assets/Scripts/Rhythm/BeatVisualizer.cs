using UnityEngine;

public class BeatVisualizer : MonoBehaviour
{
    [Header("Material Control")]
    public Material targetMaterial;
    public string shaderPropertyName = "_BeatStrength";
    public float strengthMultiplier = 0.1f;
    public float beatDecay = 0.95f;

    private float beatStrength = 0f;

    void LateUpdate()
    {
        if (Conductor.Instance == null || Conductor.Instance.CurrentState != Conductor.MusicState.Playing)
            return;

        float songPosition = Conductor.Instance.songPosition;
        float secPerBeat = Conductor.Instance.secPerBeat;

        float currentBeat = songPosition / secPerBeat;
        float fractional = currentBeat - Mathf.Floor(currentBeat);

        // 越接近整数节拍，值越大（简单脉冲效果）
        float proximity = Mathf.Cos(fractional * Mathf.PI * 2); // 范围 [-1, 1]
        beatStrength = Mathf.Max(beatStrength * beatDecay, Mathf.Abs(proximity));

        if (targetMaterial != null)
        {
            targetMaterial.SetFloat(shaderPropertyName, beatStrength * strengthMultiplier);
        }
    }
}
