using System;
using System.Collections;
using UnityEngine;
public interface IMoveBehavior
{
    IEnumerator Move(Enemy enemy, Transform target, float speed, Action<Enemy> onComplete);
}
