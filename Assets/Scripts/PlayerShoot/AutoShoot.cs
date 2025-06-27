using UnityEngine;
using System.Linq;
using System.Collections.Generic;
using System.Collections;
public class AutoShoot : Gun
{
    private int lastShotBeat = -1;
    private List<int> scheduledBeats = new List<int>();
    private Coroutine currentTracerRoutine;
    private Vector3[] basePositions = new Vector3[8]; //segmentCount = 8; // 越多越平滑
    private static readonly List<Enemy> activeEnemies = new List<Enemy>();

    public static void RegisterEnemy(Enemy enemy)
    {
        if (!activeEnemies.Contains(enemy))
            activeEnemies.Add(enemy);
    }

    public static void UnregisterEnemy(Enemy enemy)
    {
        activeEnemies.Remove(enemy);
    }

    protected override void Start()
    {
        base.Start();

        if (BeatLoader.LoadedData != null)
        {
            scheduledBeats = new List<int>(BeatLoader.LoadedData.beatHits);
        }
        else
        {
            Debug.LogWarning("⚠️ Beat data not loaded in AutoShoot.");
        }
    }

    protected override void Update()
    {
        if (Time.timeScale == 0f) return; // 如果游戏暂停了，不执行枪械逻辑

        int currentBeat = Conductor.Instance?.hit ?? -1;
        if (currentBeat != -1 && currentBeat != lastShotBeat)
        {
            lastShotBeat = currentBeat;
            Shooting();
        }
    }


    protected override void Shooting()
    {
        if (Conductor.Instance == null || Conductor.Instance.CurrentState != Conductor.MusicState.Playing)
            return;

        int currentBeat = Conductor.Instance.hit;

        // 每个节拍都射击最近敌人
        Transform target = FindClosestEnemy();
        if (target != null)
        {
            // Vector3 targetCenter = target.position;
            // shootDir = (targetCenter - muzzlePos.position).normalized;
            SetTracer(target);
        }
    }

    // // 设置子弹轨迹
    // // 这里是直接 LineRenderer 没有扰动
    // private void SetTracer(Vector3 endPosition)
    // {
    //     GameObject bullet = ObjectPool.Instance.GetGameObject(bulletPrefab);
    //     LineRenderer tracer = bullet.GetComponent<LineRenderer>();
    //     tracer.SetPosition(0, muzzlePos.position);
    //     tracer.SetPosition(1, endPosition);
    // }

    private void SetTracer(Transform targetTransform)
    {
        GameObject bullet = ObjectPool.Instance.GetGameObject(bulletPrefab);
        LineRenderer tracer = bullet.GetComponent<LineRenderer>();

        int segmentCount = 8;
        tracer.positionCount = segmentCount;

        Vector3 start = muzzlePos.position;
        Vector3 dir = (targetTransform.position - start); // 初始方向

        if (currentTracerRoutine != null)
            StopCoroutine(currentTracerRoutine);

        currentTracerRoutine = StartCoroutine(AnimateTracerFollow(tracer, start, dir, segmentCount, targetTransform));
    }


    private IEnumerator AnimateTracerFollow(LineRenderer tracer, Vector3 start, Vector3 initialDir, int segmentCount, Transform target)
    {
        float duration = 0.15f;
        float timer = 0f;

        for (int i = 0; i < segmentCount; i++)
        {
            float t = i / (segmentCount - 1f);
            basePositions[i] = start + initialDir * t;
        }

        while (timer < duration && target != null)
        {
            timer += Time.deltaTime;
            float progress = timer / duration;
            float noiseAmount = Mathf.Lerp(0.4f, 0f, progress);

            Vector3 dynamicDir = (target.position - start); // 实时方向

            for (int i = 0; i < segmentCount; i++)
            {
                float t = i / (segmentCount - 1f);
                Vector3 basePos = start + dynamicDir * t; // 跟随敌人移动更新位置
                Vector3 offset = Vector3.Cross(dynamicDir.normalized, Vector3.forward) * UnityEngine.Random.Range(-1f, 1f) * noiseAmount;
                tracer.SetPosition(i, basePos + offset);
            }

            yield return null;
        }

        // 最终归为敌人当前方向直线
        if (target != null)
        {
            Vector3 finalDir = (target.position - start);
            for (int i = 0; i < segmentCount; i++)
            {
                float t = i / (segmentCount - 1f);
                tracer.SetPosition(i, start + finalDir * t);
            }
        }
    }
    /*
     NOTE: 效果描述
    抖动不再是完全随机，而是基于连续的函数（Perlin/Sin），有流动感 ,跟尿尿一样
    幅度（maxAmplitude）和频率（noiseScale）都可调
    时间越近，线条越抖；时间越远，越接近直线
    可以叠加更多频率细节（如高频 noise）做闪电感
*/

//     private IEnumerator AnimateTracer(LineRenderer tracer, Vector3 start, Vector3 dir, int segmentCount)
    // {
    //     float duration = 0.15f;
    //     float timer = 0f;

    //     Vector3[] basePositions = new Vector3[segmentCount];
    //     for (int i = 0; i < segmentCount; i++)
    //     {
    //         float t = i / (segmentCount - 1f);
    //         basePositions[i] = start + dir * t;
    //     }

    //     float noiseScale = 2f;   // 控制噪声频率（空间频率）
    //     float maxAmplitude = 0.6f; // 最大抖动幅度（值越大越激烈）

    //     while (timer < duration)
    //     {
    //         timer += Time.deltaTime;
    //         float progress = timer / duration;
    //         float amplitude = Mathf.Lerp(maxAmplitude, 0f, progress); // 抖动慢慢衰减

    //         for (int i = 0; i < segmentCount; i++)
    //         {
    //             float t = i / (segmentCount - 1f);
    //             Vector3 basePos = basePositions[i];

    //             // 使用 Perlin Noise + Sin 模拟平滑扰动
    //             float noise = Mathf.PerlinNoise(Time.time * 10f + t * noiseScale, 0f);
    //             float sinWave = Mathf.Sin((Time.time + t * noiseScale) * 20f);
    //             float offsetAmount = (noise + sinWave) * 0.5f * amplitude;

    //             Vector3 offsetDir = Vector3.Cross(dir.normalized, Vector3.forward); // 垂直于射线方向
    //             tracer.SetPosition(i, basePos + offsetDir * offsetAmount);
    //         }

    //         yield return null;
    //     }

    //     // 回归直线
    //     for (int i = 0; i < segmentCount; i++)
    //     {
    //         tracer.SetPosition(i, basePositions[i]);
    //     }
    // }


    private Transform FindClosestEnemy()
    {
        float minDistance = float.MaxValue;
        Transform closest = null;

        foreach (var enemy in activeEnemies)
        {
            if (enemy == null || !enemy.gameObject.activeInHierarchy)
            continue;

            float dist = Vector3.Distance(muzzlePos.position, enemy.transform.position);
            if (dist < minDistance)
            {
                minDistance = dist;
                closest = enemy.transform;
            }
        }
        return closest;
    }
}
