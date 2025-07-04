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
    }
}
