using System.Collections;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class TargetedCircleAttack : IAttackBehavior
{
    private GameObject circleEffectPrefab;
    private MMProgressBar progressBar;

    public TargetedCircleAttack(GameObject effectPrefab, MMProgressBar bar)
    {
        this.circleEffectPrefab = effectPrefab;
        this.progressBar = bar;
    }

    public IEnumerator Attack(Enemy enemy, Transform target)
    {
        if (circleEffectPrefab == null || target == null)
        {
            yield break;
        }

        // 获取当前节拍时长（自动）
        float beatDuration = Conductor.Instance != null ? Conductor.Instance.secPerBeat : 0.5f;

        // 1. 实例化圆圈特效
        GameObject circleEffect = GameObject.Instantiate(circleEffectPrefab, target.position, Quaternion.identity);
        circleEffect.transform.localScale = Vector3.one * 3f; // 初始大圈

        // 2. 缩圈动画：持续 3 个节拍
        float duration = beatDuration * 3f;

        // 动画序列
        Sequence seq = DOTween.Sequence();
        seq.Append(circleEffect.transform.DOScale(0.5f, duration).SetEase(Ease.InOutSine));

        // 透明度淡出（如果有 SpriteRenderer）
        var sr = circleEffect.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            seq.Join(sr.DOFade(0f, duration));
        }

        // 播放并等待结束
        seq.Play();
        yield return seq.WaitForCompletion();

        // 3. 扣血（可选）
        if (progressBar != null)
        {
            progressBar.MinusPercent(0.01f);
        }

        // 4. 清理
        GameObject.Destroy(circleEffect);
    }
}
