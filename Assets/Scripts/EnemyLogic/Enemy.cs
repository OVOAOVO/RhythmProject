using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;
using DG.Tweening;
public class Enemy : MonoBehaviour
{
    protected Transform target;
    protected float moveSpeed;
    protected Action<Enemy> onReachedTarget;

    private Coroutine moveCoroutine;
    private IMoveBehavior moveBehavior = new DirectMove(); // 默认行为

    private List<Tween> activeTweens = new List<Tween>();
    public virtual void Initialize(Transform target, float speed, Action<Enemy> onReached = null, IMoveBehavior customBehavior = null)
    {
        this.target = target;
        this.moveSpeed = speed;
        this.onReachedTarget = onReached;
        this.moveBehavior = customBehavior ?? new DirectMove();

        StopAllCoroutines();
        moveCoroutine = StartCoroutine(moveBehavior.Move(this, target, speed, onReachedTarget));
    }

    private void OnEnable() => AutoShoot.RegisterEnemy(this);
    private void OnDisable()
    {
        AutoShoot.UnregisterEnemy(this);
        KillAllTweens(); // 保证回收到对象池时清理     
    }
    public void RegisterTween(Tween tween)
    {
        activeTweens.Add(tween);
    }

    public void KillAllTweens()
    {
        foreach (var t in activeTweens)
        {
            if (t.IsActive()) t.Kill();
        }
        activeTweens.Clear();
    }

}
