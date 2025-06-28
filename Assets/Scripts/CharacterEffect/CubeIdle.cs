using UnityEngine;
using DG.Tweening;

public class CubeIdle : MonoBehaviour
{
    [Header("呼吸缩放")]
    public float scaleFactor = 0.05f;
    public float scaleDuration = 0.6f;

    [Header("上下浮动")]
    public float floatHeight = 0.5f;
    public float floatDuration = 0.4f;

    [Header("节拍旋转倾斜")]
    public float swayAmount = 15f;
    public float swayDuration = 0.15f;

    private Vector3 basePosition;
    private Vector3 baseScale;
    private Quaternion baseRotation;

    private int lastHit = -1;
    private bool swayDirection = false;

    void Start()
    {
        basePosition = transform.localPosition;
        baseScale = transform.localScale;
        baseRotation = transform.localRotation;

        // 呼吸缩放
        transform.DOScale(baseScale * (1 + scaleFactor), scaleDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.OutBack)
            .SetId("scale")
            .SetTarget(transform);

        // 上下浮动
        transform.DOLocalMoveY(basePosition.y + floatHeight, floatDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InQuad)
            .SetId("float")
            .SetTarget(transform);
    }

    void Update()
    {
        var conductor = Conductor.Instance;
        if (conductor == null || conductor.CurrentState != Conductor.MusicState.Playing) return;

        if (conductor.hit != lastHit)
        {
            lastHit = conductor.hit;
            OnBeat();
        }
    }

    void OnBeat()
    {
        float angle = swayDirection ? swayAmount : -swayAmount;
        swayDirection = !swayDirection;

        // 倾斜目标角度（基于初始旋转）
        Quaternion tiltRotation = Quaternion.Euler(baseRotation.eulerAngles + new Vector3(0, 0, angle));

        // 倾斜过去，再自动回正，无 Kill
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOLocalRotateQuaternion(tiltRotation, swayDuration).SetEase(Ease.OutQuad));
        seq.Append(transform.DOLocalRotateQuaternion(baseRotation, swayDuration).SetEase(Ease.InQuad));
        seq.SetId("rotate").SetTarget(transform);
    }

    void OnDestroy()
    {
        DOTween.Kill(transform); // 清理当前对象上的所有 Tween
    }
}
