using UnityEngine;
using DG.Tweening;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class BossSpawner : MonoBehaviour
{
    [Header("Boss 设置")]
    public GameObject bossPrefab;
    public Transform target;
    public float moveSpeed = 6f;

    [Header("Boss 位置偏移（相对于目标）")]
    [Tooltip("Boss 初始生成偏移量")] public Vector3 bossStartOffset = new Vector3(30f, 0f, 0f);
    [Tooltip("Boss 入场点偏移量")] public Vector3 bossEnterOffset = new Vector3(8f, 0f, 0f);

    [Header("攻击设置")]
    public GameObject circleEffectPrefab;
    public float beatDuration = 0.5f;
    public MMProgressBar Cube_NotEnemy_progressBar;

    private void Start()
    {
        SpawnBoss();
    }

    public void SpawnBoss()
    {
        if (bossPrefab == null || target == null)
        {
            Debug.LogWarning("BossPrefab 或 Target 未设置");
            return;
        }

        GameObject boss = ObjectPool.Instance.GetGameObject(bossPrefab);
        if (boss == null)
        {
            Debug.LogError("对象池中获取 Boss 实例失败！");
            return;
        }

        boss.SetActive(true);

        BossUIManager.Instance?.ShowBossUI();

        BossEnemy bossEnemy = boss.GetComponent<BossEnemy>();
        if (bossEnemy != null)
        {
            bossEnemy.IsBoss = true;
            bossEnemy.startOffset = bossStartOffset;
            bossEnemy.enterOffset = bossEnterOffset;

            bossEnemy.attackBehavior = new TargetedCircleAttack(circleEffectPrefab, beatDuration, Cube_NotEnemy_progressBar);

            bossEnemy.Initialize(target, moveSpeed, e =>
            {
                Debug.Log("Boss 入场完成: " + e.name);
                if (Conductor.Instance != null)
                {
                    Conductor.Instance.aliveEnemies++;
                }
            });
        }
        else
        {
            Debug.LogWarning("Boss GameObject 上没有 BossEnemy 组件！");
        }
    }

#if UNITY_EDITOR
    private void OnDrawGizmosSelected()
    {
        if (target == null) return;

        Vector3 startPos = target.position + bossStartOffset;
        Vector3 enterPos = target.position + bossEnterOffset;

        Gizmos.color = Color.red;
        Gizmos.DrawSphere(startPos, 0.3f);

        Gizmos.color = Color.green;
        Gizmos.DrawSphere(enterPos, 0.3f);

        Gizmos.color = Color.cyan;
        Gizmos.DrawLine(startPos, enterPos);
    }
#endif
}