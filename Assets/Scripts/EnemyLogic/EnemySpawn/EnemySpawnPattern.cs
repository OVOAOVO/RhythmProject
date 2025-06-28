// EnemySpawnPattern.cs
using UnityEngine;
using System;
using System.Collections.Generic;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;
using UnityEngine.SceneManagement;
public class EnemySpawnPattern
{
    private int spawnSteps;
    private float radius, startAngle, endAngle;
    private GameObject target;
    private float moveSpeed;
    private int currentSpawnIndex = 0;
    private bool goingDown = true;

    private List<IEnemySpawnBehavior> behaviors = new List<IEnemySpawnBehavior>();
    private MMProgressBar progressBar;
    private MMFeedbacks feedbacks;

    public EnemySpawnPattern(int steps, float rad, float start, float end, GameObject tgt, float speed, MMProgressBar bar, MMFeedbacks fb)
    {
        spawnSteps = steps;
        radius = rad;
        startAngle = start;
        endAngle = end;
        target = tgt;
        moveSpeed = speed;
        progressBar = bar;
        feedbacks = fb;
    }

    public void RegisterPatterns(List<IEnemySpawnBehavior> list)
    {
        behaviors = list;
    }

    public void SpawnRandom()
    {
        int index = UnityEngine.Random.Range(0, behaviors.Count);
        Vector3 pos = GetNextSpawnPosition();
        behaviors[index].Spawn(pos, target, moveSpeed, OnEnemyReached);
    }

    private Vector3 GetNextSpawnPosition()
    {
        float step = (endAngle - startAngle) / (spawnSteps - 1);
        float angle = (startAngle + step * currentSpawnIndex) * Mathf.Deg2Rad;
        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);

        UpdateSpawnIndex();
        return new Vector3(y, -x, 0f);
    }

    private void UpdateSpawnIndex()
    {
        if (goingDown)
        {
            currentSpawnIndex++;
            if (currentSpawnIndex >= spawnSteps)
            {
                currentSpawnIndex = spawnSteps - 2;
                goingDown = false;
            }
        }
        else
        {
            currentSpawnIndex--;
            if (currentSpawnIndex < 0)
            {
                currentSpawnIndex = 1;
                goingDown = true;
            }
        }
    }

    private void OnEnemyReached(Enemy enemy)
    {
        ObjectPool.Instance.PushObject(enemy.gameObject);
        progressBar.Minus10Percent();
        feedbacks.PlayFeedbacks();
        Conductor.Instance.aliveEnemies--;
    }

    public bool CheckShouldEndGame(Conductor conductor)
    {
        return (progressBar.BarTarget <= 0f || 
            (Conductor.Instance.aliveEnemies <= 0 && Conductor.Instance.CurrentState == Conductor.MusicState.Finished));
    }
}