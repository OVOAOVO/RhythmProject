using System;
using UnityEngine;
using DG.Tweening;

public class BossEnemy : Enemy
{
    public override void Initialize(Transform target, float speed, Action<Enemy> onReached = null, IMoveBehavior customBehavior = null)
    {
        this.target = target;
        this.moveSpeed = speed;
        this.onReachedTarget = onReached;
        // 这里不使用自定义的moveBehavior，且不启动基类协程，注释掉：
        // this.moveBehavior = customBehavior ?? new DirectMove();

        // 直接将Boss位置设置为远处起点，比如target.x + 100
        transform.position = new Vector3(target.position.x + 30f, target.position.y, target.position.z);

        // 禁用碰撞，动画结束再启用
        var col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 入场动画：移动到目标附近的入场点，比如 target.x + 5
        Vector3 enterPos = new Vector3(target.position.x + 8f, target.position.y, target.position.z);
        // ✅ 使用 TweenOwner 来注册 tween
        var tween = transform.DOMove(enterPos, 1.5f).SetEase(Ease.OutBack).OnComplete(() =>
        {
            if (col != null) col.enabled = true;
            onReached?.Invoke(this);
        });

        // ✅ 确保 TweenOwner 存在
        var owner = GetComponent<TweenOwner>();
        if (owner == null) owner = gameObject.AddComponent<TweenOwner>();

        owner.RegisterTween(tween);
    }
}
