using UnityEngine;
using DG.Tweening;

public class EnemyIdle : MonoBehaviour
{
    [Header("呼吸缩放")]
    public float scaleFactor = 0.1f;       // 缩放幅度（越大呼吸越明显）
    public float scaleDuration = 0.6f;     // 每次呼吸时长（越小越快）

    private Vector3 baseScale;

    private void Awake()
    {
        baseScale = transform.localScale;
    }

    private void OnEnable()
    {
        // 每次激活都重新创建 Tween
        transform.DOScale(baseScale * (1 + scaleFactor), scaleDuration)
            .SetLoops(-1, LoopType.Yoyo)
            .SetEase(Ease.InOutSine)
            .SetId("enemy_breathing")
            .SetTarget(transform);
    }

    private void OnDisable()
    {
        // Kill 掉以 transform 为 target 的所有 Tween
        DOTween.Kill(transform);
        transform.localScale = baseScale; // 防止残留缩放
    }
}
