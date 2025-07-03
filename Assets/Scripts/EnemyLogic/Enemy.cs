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

    public GameObject hitEffectPrefab;  // 被击中粒子特效预制体

    public GameObject spawnEffectPrefab;  // 生成粒子特效预制体

    public bool IsBoss = false;

    private TweenOwner tweenOwner;

    public virtual void Initialize(Transform target, float speed, Action<Enemy> onReached = null, IMoveBehavior customBehavior = null)
    {
        this.target = target;
        this.moveSpeed = speed;
        this.onReachedTarget = onReached;
        this.moveBehavior = customBehavior ?? new DirectMove();
        
        EnsureTweenOwner(); // 确保有 TweenOwner

        StopAllCoroutines();
        moveCoroutine = StartCoroutine(moveBehavior.Move(this, target, speed, onReachedTarget));
    }

    private void OnEnable() => AutoShoot.RegisterEnemy(this);
    private void OnDisable() => AutoShoot.UnregisterEnemy(this);

    public void RegisterTween(Tween tween)
    {
        tweenOwner?.RegisterTween(tween);
    }

    public void EnsureTweenOwner()
    {
        if (tweenOwner == null)
        {
            tweenOwner = GetComponent<TweenOwner>();
            if (tweenOwner == null)
            {
                tweenOwner = gameObject.AddComponent<TweenOwner>();
            }
        }
    }

    // 因为使用了对象池维护了敌人对象，所以不能在这边维护粒子的生命周期    
    public void PlayHitEffect(Vector3 position)
    {
        ParticleEffectManager.Instance.PlayEnemyHittedEffect(hitEffectPrefab, position);
    }

    public void PlaySpawnEffect(Vector3 position)
    {
        ParticleEffectManager.Instance.PlayEnemySpawnEffect(spawnEffectPrefab, position);
    }
}
