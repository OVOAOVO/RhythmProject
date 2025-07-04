using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine.SceneManagement;
using System;
using System.Linq;

public class EnemySpawner : MonoBehaviour
{
    public GameObject target;
    public float moveSpeed = 6f;

    [Header("生成控制")]
    public int spawnSteps = 10;
    public float radius = 5f;
    public float startAngle = 45f;
    public float endAngle = 135f;

    [Header("反馈与UI")]
    public MMFeedbacks healthBarDecreaseFeedBacks;
    public MMProgressBar progressBar;

    [Header("敌人预制体")]
    public List<GameObject> EnemyPrefab;
    public List<GameObject> JumpingEnemyPrefab;
    public List<GameObject> DashEnemyPrefab; // ✅ 新增 dash 敌人列表

    private BeatScheduler beatScheduler;
    private EnemySpawnPattern spawnPattern;
    void Start()
    {
        beatScheduler = new BeatScheduler();
        beatScheduler.Init(Conductor.Instance);

        var activeBehaviors = new List<IEnemySpawnBehavior>();

        if (EnemyPrefab != null && EnemyPrefab.Count > 0)
        {
            activeBehaviors.Add(new BasicMonsterSpawner(EnemyPrefab, this));
        }
        if (JumpingEnemyPrefab != null && JumpingEnemyPrefab.Count > 0)
        {
            activeBehaviors.Add(new JumpMonsterSpawner(JumpingEnemyPrefab, this));
        }
        if (DashEnemyPrefab != null && DashEnemyPrefab.Count > 0)
        {
            activeBehaviors.Add(new DashMonsterSpawner(DashEnemyPrefab, this));
        }

        spawnPattern = new EnemySpawnPattern(spawnSteps, radius, startAngle, endAngle, target, moveSpeed, progressBar, healthBarDecreaseFeedBacks);
        spawnPattern.RegisterPatterns(activeBehaviors);
    }


    void Update()
    {
        if (beatScheduler.IsEnemySpawnBeat(Conductor.Instance.hit))
        {
            spawnPattern.SpawnRandom();
        }

        if (spawnPattern.CheckShouldEndGame(Conductor.Instance))
        {
            ResultDataManager.LastPlayedSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("ResultMenu");
        }
    }
#if UNITY_EDITOR
private void OnDrawGizmosSelected()
{
    if (spawnSteps <= 1) return;

    // 场景中的中心点（对应 SpawnPattern 里生成位置的原点）
    Vector3 center = transform.position;

    // 每一步的角度增量
    float step = (endAngle - startAngle) / (spawnSteps - 1);

    // 用于画线时连接上一步的位置
    Vector3? prevPos = null;

    for (int i = 0; i < spawnSteps; i++)
    {
        // 计算当前角度
        float angleRad = (startAngle + step * i) * Mathf.Deg2Rad;
        // 原始极坐标到笛卡尔(x,y)：
        float x = radius * Mathf.Cos(angleRad);
        float y = radius * Mathf.Sin(angleRad);
        // 和你的 SpawnPattern 一致的 z 偏移
        float zOffset = i * 0.05f;

        // 对应 SpawnPattern 中 return 的 new Vector3(y, -x, zOffset)
    Vector3 worldSpawnPos = new Vector3(y, -x, zOffset);  // 不加 center


        // 画一个小球标记
        Gizmos.color = Color.red;
        Gizmos.DrawSphere(worldSpawnPos, 0.15f);

        // （可选）用线把点和中心连起来
        Gizmos.color = Color.red;
        Gizmos.DrawLine(center, worldSpawnPos);

        // （可选）把相邻两点连成折线，便于看出扇形轮廓
        if (prevPos.HasValue)
        {
            Gizmos.DrawLine(prevPos.Value, worldSpawnPos);
        }
        prevPos = worldSpawnPos;
    }
}
#endif


}
