using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class EnemySpawner : MonoBehaviour
{
    public GameObject monsterPrefab;
    public GameObject target;
    public float radius = 5f;
    public float startAngle = 0f;
    public float endAngle = 180f;
    public float moveSpeed = 20f;

    public MMFeedbacks healthBarFeedBacks; // 反馈系统
    public MMProgressBar progressBar;
    private float previousHit = 0f;

    void Update()
    {
        if (Conductor.Instance.hit > previousHit)
        {
            previousHit = Conductor.Instance.hit;
            SpawnMonsterAtRandomAngle();
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

        // 先关闭 Collider
        Collider col = monsterObj.GetComponent<Collider>();
        if (col != null) col.enabled = false;

        // 下一帧再启用 Collider
        StartCoroutine(EnableColliderNextFrame(col));

        Enemy enemy = monsterObj.GetComponent<Enemy>();
        enemy.Initialize(target.transform, moveSpeed, OnEnemyReached);
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
        healthBarFeedBacks.PlayFeedbacks(); // 播放反馈
    }
}
