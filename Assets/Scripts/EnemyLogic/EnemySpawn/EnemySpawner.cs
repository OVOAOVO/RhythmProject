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
}