using UnityEngine;

public class BeatVisualizer : MonoBehaviour
{
    public Material targetMaterial;           // 原始材质，仅用于复制
    public string shaderPropertyName = "_BeatStrength";
    public float strengthMultiplier = 0.1f;
    public float beatDecay = 0.9f;

    private Material runtimeMaterial;         // 运行时克隆版本
    private float beatStrength = 0f;
    private int lastBeat = -1;

    void Start()
    {
        if (targetMaterial != null)
        {
            runtimeMaterial = new Material(targetMaterial);  // 克隆一份
            GetComponent<Renderer>().material = runtimeMaterial; // 应用给当前对象（根据需要替换）
        }
    }

    void LateUpdate()
    {
        if (Conductor.Instance == null || Conductor.Instance.CurrentState != Conductor.MusicState.Playing)
            return;

        float songPosition = Conductor.Instance.songPosition;
        float secPerBeat = Conductor.Instance.secPerBeat;

        int currentBeat = Mathf.FloorToInt(songPosition / secPerBeat);

        if (currentBeat != lastBeat)
        {
            lastBeat = currentBeat;
            beatStrength = 1.0f;
        }
        else
        {
            beatStrength *= beatDecay;
        }

        if (runtimeMaterial != null)
        {
            runtimeMaterial.SetFloat(shaderPropertyName, beatStrength * strengthMultiplier);
        }
    }
}

