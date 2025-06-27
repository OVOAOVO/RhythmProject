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
    public float moveSpeed = 20f;

    [Header("生成控制")]
    public int spawnSteps = 10;
    public float radius = 5f;
    public float startAngle = 0f;
    public float endAngle = 180f;

    [Header("反馈与UI")]
    public MMFeedbacks healthBarDecreaseFeedBacks;
    public MMProgressBar progressBar;

    private BeatScheduler beatScheduler;
    private EnemySpawnPattern spawnPattern;

    void Start()
    {
        beatScheduler = new BeatScheduler();
        beatScheduler.Init(Conductor.Instance);

        spawnPattern = new EnemySpawnPattern(spawnSteps, radius, startAngle, endAngle, target, moveSpeed, progressBar, healthBarDecreaseFeedBacks);
        spawnPattern.RegisterPatterns(new List<IEnemySpawnBehavior>
        {
            new BasicMonsterSpawner(EnemyPrefab, this),
            new JumpMonsterSpawner(JumpingEnemyPrefab, this)
        });
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

    public List<GameObject> EnemyPrefab;
    public List<GameObject> JumpingEnemyPrefab;
}