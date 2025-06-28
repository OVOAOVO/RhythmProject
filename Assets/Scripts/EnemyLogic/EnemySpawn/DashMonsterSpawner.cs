using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class DashMonsterSpawner : IEnemySpawnBehavior
{
    private List<GameObject> prefabs;
    private MonoBehaviour coroutineHost;

    public DashMonsterSpawner(List<GameObject> prefabList, MonoBehaviour coroutineHost)
    {
        prefabs = prefabList;
        this.coroutineHost = coroutineHost;
    }

    public void Spawn(Vector3 position, GameObject target, float moveSpeed, Action<Enemy> onReached)
    {
        GameObject prefab = prefabs[UnityEngine.Random.Range(0, prefabs.Count)];
        GameObject obj = ObjectPool.Instance.GetGameObject(prefab);
        obj.transform.position = position;
        obj.transform.LookAt(target.transform);

        // 延迟开启碰撞体，避免生成瞬间触发
        Collider col = obj.GetComponent<Collider>();
        if (col != null) col.enabled = false;
        coroutineHost.StartCoroutine(EnableNextFrame(col));

        Enemy enemy = obj.GetComponent<Enemy>();

        // 正确调用 Initialize 方法，传入 DashMove 行为
        enemy.Initialize(target.transform, moveSpeed, onReached, new DashMove());

        Conductor.Instance.aliveEnemies++;
    }

    private IEnumerator EnableNextFrame(Collider col)
    {
        yield return null;
        if (col != null) col.enabled = true;
    }
}
