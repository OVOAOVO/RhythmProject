using UnityEngine;
using DG.Tweening;

public class EnemyIdle : MonoBehaviour
{
    [Header("呼吸缩放")]
    public float scaleFactor = 0.1f;       // 缩放幅度（越大呼吸越明显）
    public float scaleDuration = 0.6f;      // 每次呼吸时长（越小越快）

    private Vector3 baseScale;

    private void Start()
    {
        baseScale = transform.localScale;

        // 呼吸缩放动画（无限循环，平滑往复）
        transform.DOScale(baseScale * (1 + scaleFactor), scaleDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetId("enemy_breathing")
            .SetTarget(transform);
    }

    private void OnDisable()
    {
        DOTween.Kill(transform); // 清除当前物体上的所有 Tween，防止泄露或冲突
    }
}
