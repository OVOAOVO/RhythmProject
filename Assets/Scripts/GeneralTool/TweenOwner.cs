using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// 可挂载在任意 GameObject 上，用于集中管理 DOTween 生命周期，适配对象池模式（OnDisable 清理）
/// </summary>
[DisallowMultipleComponent]
public class TweenOwner : MonoBehaviour
{
    private readonly List<Tween> activeTweens = new();

    /// <summary>
    /// 注册一个 Tween 到当前对象 Tween 列表中
    /// </summary>
    public void RegisterTween(Tween tween)
    {
        if (tween != null && tween.IsActive())
        {
            activeTweens.Add(tween);
        }
    }

    /// <summary>
    /// 杀死所有当前拥有的 Tween（通常在对象池回收时调用）
    /// </summary>
    public void KillAllTweens()
    {
        for (int i = 0; i < activeTweens.Count; i++)
        {
            var t = activeTweens[i];
            if (t != null && t.IsActive())
            {
                t.Kill();
            }
        }
        activeTweens.Clear();
    }

    /// <summary>
    /// 避免 Tween 遗留在对象池中：在对象被禁用（SetActive(false)）时自动清理
    /// </summary>
    private void OnDisable()
    {
        KillAllTweens();
    }
}
