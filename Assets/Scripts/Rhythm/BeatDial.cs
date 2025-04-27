using UnityEngine;
using UnityEngine.UI;

public class BeatDial : MonoBehaviour
{
    private Image img;
    private RectTransform rectT;
    private int currentHit;

    // 每次节拍要旋转的角度
    public float anglePerHit = 45f;
    // 平滑时间，越小越“硬”
    public float smoothTime = 0.1f;

    // 内部追踪：目标角度、当前插值速度
    private float targetZ;
    private float zVelocity = 0f;

    void Start()
    {
        img = GetComponent<Image>();
        rectT = img.rectTransform;
        // 记录初始的 hit 值
        currentHit = Conductor.Instance.hit;
        // 初始化目标角度为当前角度
        targetZ = rectT.eulerAngles.z;
    }

    void Update()
    {
        // 检测到新的 hit，就累加目标角度
        if (Conductor.Instance.hit > currentHit)
        {
            int delta = Conductor.Instance.hit - currentHit;
            currentHit = Conductor.Instance.hit;
            targetZ += anglePerHit * delta;
        }

        // 平滑地把当前角度向目标角度推进
        float currentZ = rectT.eulerAngles.z;
        float smoothZ = Mathf.SmoothDampAngle(currentZ, targetZ, ref zVelocity, smoothTime);
        rectT.rotation = Quaternion.Euler(0f, 0f, smoothZ);
    }
}
