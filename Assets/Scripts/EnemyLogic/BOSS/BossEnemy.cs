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
}