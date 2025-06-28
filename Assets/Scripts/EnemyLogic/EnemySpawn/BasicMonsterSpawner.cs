using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class BasicMonsterSpawner : IEnemySpawnBehavior
{
    private List<GameObject> prefabs;
    private MonoBehaviour coroutineHost;

    public BasicMonsterSpawner(List<GameObject> prefabList, MonoBehaviour coroutineHost)
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

        Collider col = obj.GetComponent<Collider>();
        if (col != null) col.enabled = false;
        coroutineHost.StartCoroutine(EnableNextFrame(col));

        Enemy enemy = obj.GetComponent<Enemy>();
        enemy.Initialize(target.transform, moveSpeed, onReached);
        Conductor.Instance.aliveEnemies++;
    }

    private IEnumerator EnableNextFrame(Collider col)
    {
        yield return null;
        if (col != null) col.enabled = true;
    }
}