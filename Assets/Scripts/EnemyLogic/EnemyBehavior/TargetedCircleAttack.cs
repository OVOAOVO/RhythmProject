using System.Collections;
using UnityEngine;
using MoreMountains.Tools;
using System.Collections.Generic;

public class TargetedCircleAttack : IAttackBehavior
{
    private GameObject circleEffectPrefab;
    private MMProgressBar progressBar;
    private BeatRecord beatRecord; // ✅ 加入记录引用

    public TargetedCircleAttack(GameObject effectPrefab, MMProgressBar bar, BeatRecord record)
    {
        this.circleEffectPrefab = effectPrefab;
        this.progressBar = bar;
        this.beatRecord = record;
    }

    public IEnumerator Attack(Enemy enemy, Transform target)
    {
        if (circleEffectPrefab == null || target == null || beatRecord == null)
        {
            yield break;
        }

        int lastBeat = -1;

        while (!Conductor.Instance.IsMusicFinished())
        {
            int currentBeat = Conductor.Instance.hit;

            if (currentBeat != lastBeat)
            {
                lastBeat = currentBeat;

                // ✅ 检查是否是攻击节拍
                if (beatRecord.attackBeats.Contains(currentBeat))
                {
                    // ✅ 播放攻击粒子特效
                    GameObject effect = GameObject.Instantiate(circleEffectPrefab, target.position, Quaternion.identity);
                    GameObject.Destroy(effect, 2f); // 避免残留

                    // ✅ 可选：更新 UI 或播放反馈
                    if (progressBar != null)
                    {
                        // progressBar.SetValue(...);
                    }

                    // ✅ 可加：打击反馈 / 声音 / Shake Camera 等
                }
            }

            yield return null;
        }
    }
}
