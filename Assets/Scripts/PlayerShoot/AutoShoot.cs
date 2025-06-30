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
