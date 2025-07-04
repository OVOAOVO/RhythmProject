using System.Collections;
using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class TargetedCircleAttack : IAttackBehavior
{
    private GameObject circleEffectPrefab;
    private float beatDuration;  // 一个节拍的时长，需外部传入或获取
    private MMProgressBar progressBar;

    public TargetedCircleAttack(GameObject effectPrefab, float beatDuration, MMProgressBar bar)
    {
        this.circleEffectPrefab = effectPrefab;
        this.beatDuration = beatDuration;
        this.progressBar = bar;
    }

    public IEnumerator Attack(Enemy enemy, Transform target)
    {
        if (circleEffectPrefab == null || target == null)
        {
            yield break;
        }

        // 1. 实例化圆圈特效
        GameObject circleEffect = GameObject.Instantiate(circleEffectPrefab, target.position, Quaternion.identity);
        circleEffect.transform.localScale = Vector3.one * 3f; // 初始大圈

        // 2. 缩圈动画：3秒内缩小到接近玩家（假设3个节拍长度）
        float duration = beatDuration * 3;

        // 动画序列
        Sequence seq = DOTween.Sequence();

        // 缩小到0.5倍
        seq.Append(circleEffect.transform.DOScale(0.5f, duration).SetEase(Ease.InOutSine));
        // 透明度淡出（假设circleEffect有SpriteRenderer）
        var sr = circleEffect.GetComponent<SpriteRenderer>();
        if (sr != null)
        {
            seq.Join(sr.DOFade(0f, duration));
        }

        // 播放序列
        seq.Play();

        // 等待动画结束
        yield return seq.WaitForCompletion();

        // 3. 扣血
        if (progressBar != null)
        {
            progressBar.MinusPercent(0.01f);
        }

        // 4. 清理特效
        GameObject.Destroy(circleEffect);
    }
}
