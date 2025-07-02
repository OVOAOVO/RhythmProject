using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class JumpMonsterSpawner : IEnemySpawnBehavior
{
    private List<GameObject> prefabs;
    private MonoBehaviour coroutineHost;

    public JumpMonsterSpawner(List<GameObject> prefabList, MonoBehaviour coroutineHost)
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

        JumpingEnemy enemy = obj.GetComponent<JumpingEnemy>();
        Vector3 midPoint = position + new Vector3(-2f, 0f, 0f);
        Vector3 jumpTarget = position + new Vector3(-4f, 0f, 0f);
        enemy.InitializeJump(midPoint, jumpTarget, target.transform, moveSpeed, onReached);
        // //播放生成特效
        // enemy.PlaySpawnEffect(position);
        Conductor.Instance.aliveEnemies++;
    }

    private IEnumerator EnableNextFrame(Collider col)
    {
        yield return null;
        if (col != null) col.enabled = true;
    }
}