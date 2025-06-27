using System;
using System.Collections;
using UnityEngine;

public interface IEnemySpawnBehavior
{
    void Spawn(Vector3 position, GameObject target, float moveSpeed, Action<Enemy> onReached);
}