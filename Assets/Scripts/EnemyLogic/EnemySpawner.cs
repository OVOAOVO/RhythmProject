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
    public List<GameObject> EnemyPrefab;
    public List<GameObject> jumpingEnemyPrefab; // 跳跃怪物预制体
    private List<Action> spawnFunctions;
    public GameObject target;
    public float radius = 5f;
    public float startAngle = 0f;
    public float endAngle = 180f;
    public float moveSpeed = 20f;

    public MMFeedbacks healthBarDecreaseFeedBacks; // 反馈系统
    public MMProgressBar progressBar;

    private List<int> scheduledBeats = new List<int>();
   
    private Dictionary<int, int> spawnedBeatCounts = new Dictionary<int, int>(); // 记录每个 beat 已触发次数
    private int spawnAdvanceBeats = 6;
    
    void Start()
    {
        if (BeatLoader.LoadedData != null)
        {
            scheduledBeats = new List<int>(BeatLoader.LoadedData.beatHits);
        }
        else
        {
            Debug.LogWarning("⚠️ No beat data loaded.");
        }

        if (Conductor.Instance != null && Conductor.Instance.songBPM > 0)
        {
            float beatsPerSecond = Conductor.Instance.songBPM / 60f;
            float timeToTravel = (radius/2) / moveSpeed; // 假设从半径5米远处移动
            spawnAdvanceBeats = Mathf.CeilToInt(timeToTravel * beatsPerSecond);//这个计算的是根据BPM要提前多少拍发射怪物刚好到中心
        }

            // 初始化生成函数列表
        spawnFunctions = new List<Action>
        {
            SpawnMonsterAtRandomAngle,
            SpawnJumpMonster,
            // 你可以继续加更多的生成函数，比如：
            // SpawnFlyMonster,
            // SpawnBoss
        };
    }
    // 每拍发射怪物
    // void Update()
    // {
    //     if (Conductor.Instance.hit > previousHit)
    //     {
    //         previousHit = Conductor.Instance.hit;
    //         SpawnMonsterAtRandomAngle();
    //         //SpawnJumpMonster();
    //     }

    //     if (Conductor.Instance.CurrentState == Conductor.MusicState.Finished)
    //     {
    //         // 并且场上已经没有怪物了，直接跳转
    //         if (Conductor.Instance.aliveEnemies <= 0)
    //         {
    //             Debug.Log("update跳转到结果界面");
    //             ResultDataManager.LastPlayedSceneName = SceneManager.GetActiveScene().name;
    //             SceneManager.LoadScene("ResultMenu");
    //         }
    //     }
    // }
    void Update()
    {
        int currentHit = Conductor.Instance.hit;

        foreach (int beat in scheduledBeats)
        {
            int fireBeat = beat - spawnAdvanceBeats;

            // ✅ 精准匹配节拍，并且只触发一次
            //Unity 的 Update() 是 每秒调用多次（一般是每秒 60 帧或以上），而 hit 只在进入下一拍时才加 1
            if (currentHit == fireBeat)
            {
                // 检查这个 beat 之前触发了多少次
                if (!spawnedBeatCounts.ContainsKey(beat))
                    spawnedBeatCounts[beat] = 0;

                // 统计 beat 的总出现次数
                int totalOccurrences = scheduledBeats.Count(b => b == beat);

                if (spawnedBeatCounts[beat] < totalOccurrences)
                {
                    spawnedBeatCounts[beat]++;
                    int index = UnityEngine.Random.Range(0, spawnFunctions.Count);
                    spawnFunctions[index].Invoke(); // 生成怪物
                }
            }
        }

        // 结算逻辑保持不变
        if (Conductor.Instance.CurrentState == Conductor.MusicState.Finished &&
            Conductor.Instance.aliveEnemies <= 0)
        {
            ResultDataManager.LastPlayedSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("ResultMenu");
        }
    }


    void SpawnMonsterAtRandomAngle()
    {
        float angle = UnityEngine.Random.Range(startAngle, endAngle) * Mathf.Deg2Rad;
        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);
        Vector3 spawnPos = new Vector3(y, -x, 0f); // 旋转90度

        // 随机选择一个预制体
        GameObject prefab = EnemyPrefab[UnityEngine.Random.Range(0, EnemyPrefab.Count)];
        GameObject monsterObj = ObjectPool.Instance.GetGameObject(prefab);
        monsterObj.transform.position = spawnPos;
        monsterObj.transform.LookAt(target.transform);

        // ✅ 设置随机缩放（大小 80% - 120%）
        float scale = UnityEngine.Random.Range(0.5f, 1.2f);
        monsterObj.transform.localScale = Vector3.one * scale;

        // 先关闭 Collider
        Collider col = monsterObj.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 下一帧再启用 Collider
        StartCoroutine(EnableColliderNextFrame(col));

        Enemy enemy = monsterObj.GetComponent<Enemy>();
        enemy.Initialize(target.transform, moveSpeed, OnEnemyReached);

        Conductor.Instance.aliveEnemies++; // 生成时数量+1
    }

    void SpawnJumpMonster()
    {
        float angle = UnityEngine.Random.Range(startAngle, endAngle) * Mathf.Deg2Rad;
        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);
        Vector3 spawnPos = new Vector3(y, -x, 0f); // 旋转90度

        // 计算跳跃中点和落地点
        Vector3 midPoint = spawnPos + new Vector3(-2.0f, 0f, 0f); // 向左移动一定距离作为中点
        Vector3 jumpTarget = spawnPos + new Vector3(-4f, 0f, 0f); // 向左更远处作为跳跃目标

        GameObject prefab = jumpingEnemyPrefab[UnityEngine.Random.Range(0, jumpingEnemyPrefab.Count)];
        GameObject monsterObj = ObjectPool.Instance.GetGameObject(prefab);

        monsterObj.transform.position = spawnPos;   
        // ✅ 设置随机缩放（大小 80% - 120%）
        float scale = UnityEngine.Random.Range(0.5f, 1.2f);
        monsterObj.transform.localScale = Vector3.one * scale;

        // 先关闭 Collider
        Collider col = monsterObj.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 下一帧再启用 Collider
        StartCoroutine(EnableColliderNextFrame(col));

        JumpingEnemy enemy = monsterObj.GetComponent<JumpingEnemy>();
        enemy.InitializeJump(midPoint, jumpTarget, target.transform, moveSpeed, OnEnemyReached);

        Conductor.Instance.aliveEnemies++; // 生成时数量+1
    }
    // NOTE:
    // 同一帧里“生成”一个敌人，再马上做一次 Physics.Raycast，新的 Collider 已经被注册到场景里了；
    // 不管在任何方向上？在当前方向上？，就会被立刻击中，造成一种穿透并且击中了刚生成敌人的bug
    // “前面生成的敌人被穿透”，但可能你认为“先生成的”就是“视觉上靠近摄像机的”。
    // 但其实 Unity 并不会自动把“先生成的对象”摆在“前面”。
    // 如果你在一个半圆范围内随机生成敌人，他们的位置是随机的，哪怕一个敌人在逻辑上“先生成”，但空间位置上可能在队尾，Raycast 命中它也合理。
    IEnumerator EnableColliderNextFrame(Collider col)
    {
        yield return null; // 等待一帧
        if (col != null) col.enabled = true;
    }

    void OnEnemyReached(Enemy enemy)
    {
        ObjectPool.Instance.PushObject(enemy.gameObject);
       
        progressBar.Minus10Percent(); // 减少进度条
        healthBarDecreaseFeedBacks.PlayFeedbacks(); // 播放反馈
        
        Conductor.Instance.aliveEnemies--; // 被击中/到终点回收时数量-1
        
        //怪物到达终点，或者生命值为0，直接跳转到结果界面
        if ((Conductor.Instance.aliveEnemies <= 0 && Conductor.Instance.CurrentState == Conductor.MusicState.Finished )|| progressBar.BarTarget <= 0f)
        {
            ResultDataManager.LastPlayedSceneName = SceneManager.GetActiveScene().name;
            SceneManager.LoadScene("ResultMenu");
        }
    }
}
