using System;
using UnityEngine;
using DG.Tweening;

public class BossEnemy : Enemy
{
    public IAttackBehavior attackBehavior;

    [Header("入场偏移（相对于目标）")]
    [Tooltip("Boss 初始生成偏移量")] public Vector3 startOffset = new Vector3(30f, 0f, 0f);
    [Tooltip("Boss 入场点偏移量")] public Vector3 enterOffset = new Vector3(8f, 0f, 0f);

    public override void Initialize(Transform target, float speed, Action<Enemy> onReached = null, IMoveBehavior customBehavior = null)
    {
        this.target = target;
        this.moveSpeed = speed;
        this.onReachedTarget = onReached;

        // 直接将Boss位置设置为远处起点
        transform.position = target.position + startOffset;

        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Vector3 enterPos = target.position + enterOffset;
        var tween = transform.DOMove(enterPos, 1.5f)
                           .SetEase(Ease.OutBack)
                           .OnComplete(() =>
        {
            if (col != null) col.enabled = true;
            onReached?.Invoke(this);
            StartCoroutine(attackBehavior?.Attack(this, target));
        });

        var owner = GetComponent<TweenOwner>() ?? gameObject.AddComponent<TweenOwner>();
        owner.RegisterTween(tween);
    }

    public void Exit()
    {
        StopAllCoroutines(); // 停止攻击行为

        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        Vector3 exitPos = target.position + startOffset; // 原路径反向退出
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(exitPos, 1.2f).SetEase(Ease.InBack));
        seq.Join(transform.DOScale(Vector3.zero, 1.2f).SetEase(Ease.InBack));
        seq.OnComplete(() =>
        {
            // 回收对象（你用的是对象池）
            gameObject.SetActive(false);
            transform.localScale = Vector3.one; // 恢复缩放
        });

        var owner = GetComponent<TweenOwner>() ?? gameObject.AddComponent<TweenOwner>();
        owner.RegisterTween(seq);
    }

}