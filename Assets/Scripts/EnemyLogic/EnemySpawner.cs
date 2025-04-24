using UnityEngine;
using System.Collections.Generic;
using System.Collections;
public class EnemySpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public GameObject target;
    public float radius = 5f;
    public float startAngle = 0f;
    public float endAngle = 180f;
    public float moveSpeed = 20f;

    private float previousHit = 0f;

    // 协程原因
    //同一帧里“生成”一个敌人，再马上做一次 Physics.Raycast，新的 Collider 已经被注册到场景里了；
    // 如果它恰巧落在你的射线方向上，就会被立刻击中，造成一种穿透并且击中了刚生成敌人的bug
    void Update()
    {
        if (Conductor.Instance.hit > previousHit)
        {
            previousHit = Conductor.Instance.hit;
            StartCoroutine(SpawnAtEndOfFrame());
        }
    }

    private IEnumerator SpawnAtEndOfFrame()
    {
        yield return null; // 等待一帧
        SpawnMonsterAtRandomAngle();
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
