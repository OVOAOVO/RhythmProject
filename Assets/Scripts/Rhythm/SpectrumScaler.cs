using UnityEngine;

public class SpectrumScaler : MonoBehaviour
{
    public AudioSource source;              // 音乐播放源
    public Material targetMaterial;         // 需要控制的材质
    public int frequencyBand = 2;           // 控制哪个频段（0 是低频）
    public float sensitivity = 10f;         // 敏感度：调节缩放幅度
    public float decay = 0.95f;             // 衰减：控制跳动的平滑度

    private float[] spectrum = new float[64];
    private float beatStrength = 0f;

    void Update()
    {
        if (!source || !targetMaterial) return;

        // 获取频率数据（64 个频段）
        source.GetSpectrumData(spectrum, 0, FFTWindow.BlackmanHarris);

        // 取目标频段值，乘以敏感度
        float energy = spectrum[frequencyBand] * sensitivity;

        // 加上衰减处理，避免突兀
        beatStrength = Mathf.Max(beatStrength * decay, energy);

        // 传值给 Shader
        targetMaterial.SetFloat("_BeatStrength", beatStrength);
    }
}
