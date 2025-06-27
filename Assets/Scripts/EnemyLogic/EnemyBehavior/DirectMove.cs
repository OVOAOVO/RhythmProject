using System;
using System.Collections;
using UnityEngine;

public class DirectMove : IMoveBehavior
{
    public IEnumerator Move(Enemy enemy, Transform target, float speed, Action<Enemy> onComplete)
    {
        while ((enemy.transform.position - target.position).sqrMagnitude > 0.01f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, target.position, speed * Time.deltaTime);
            yield return null;
        }

        enemy.transform.position = target.position;
        onComplete?.Invoke(enemy);
    }
}
