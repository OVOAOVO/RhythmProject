using System;
using UnityEngine;

public class BossSpawner : MonoBehaviour
{
    [Header("Boss 设置")]
    public GameObject bossPrefab;
    public Transform target;
    public float moveSpeed = 6f;

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

        // 实例化 Boss（不要手动设置位置，交给 BossEnemy.Initialize 控制）
        GameObject boss = Instantiate(bossPrefab);

        boss.transform.LookAt(target); // 可以设置朝向

        // 启用 Boss UI
        BossUIManager.Instance?.ShowBossUI();

        // 获取 Enemy 脚本
        Enemy enemy = boss.GetComponent<Enemy>();
        if (enemy != null)
        {
            if (!enemy.IsBoss)
            {
                enemy.IsBoss = true;
                Debug.Log("标记 Boss 为 Boss 类型");
            }

            // 直接调用 Initialize，让 BossEnemy 自己控制入场动画和位置
            enemy.Initialize(target, moveSpeed, e =>
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
            Debug.LogWarning("Boss GameObject 上没有 Enemy 组件！");
        }
    }
}
