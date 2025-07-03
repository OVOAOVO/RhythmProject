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

        // 使用你的对象池系统
        GameObject boss = ObjectPool.Instance.GetGameObject(bossPrefab);
        if (boss == null)
        {
            Debug.LogError("对象池中获取 Boss 实例失败！");
            return;
        }

        // boss.transform.LookAt(target); // 可以设置朝向
        boss.SetActive(true);                     // 激活
        
        // ✅ Boss 到位后，再调用 UI 显示
        BossUIManager.Instance?.ShowBossUI();

        // 获取 Enemy 脚本
        BossEnemy enemy = boss.GetComponent<BossEnemy>();
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
