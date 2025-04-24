using UnityEngine;
using System.Collections.Generic;

public class EnemySpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public GameObject target;
    public float radius = 5f;
    public float startAngle = 0f;
    public float endAngle = 180f;
    public float moveSpeed = 20f;

    private float previousHit = 0f;

    void Update()
    {
        if (Conductor.Instance.hit > previousHit)
        {
            SpawnMonsterAtRandomAngle();
            previousHit = Conductor.Instance.hit;
        }
    }

    void SpawnMonsterAtRandomAngle()
    {
        float angle = Random.Range(startAngle, endAngle) * Mathf.Deg2Rad;
        float x = radius * Mathf.Cos(angle);
        float y = radius * Mathf.Sin(angle);
        Vector3 spawnPos = new Vector3(y, -x, 0f); // 旋转90度

        GameObject monsterObj = ObjectPool.Instance.GetGameObject(monsterPrefab);
        monsterObj.transform.position = spawnPos;
        monsterObj.transform.LookAt(target.transform);

        Enemy enemy = monsterObj.GetComponent<Enemy>();
        enemy.Initialize(target.transform, moveSpeed, OnEnemyReached);
    }

    void OnEnemyReached(Enemy enemy)
    {
        ObjectPool.Instance.PushObject(enemy.gameObject);
    }
}
