using System;
using UnityEngine;

public class DashEnemy : Enemy
{
    public void InitializeDash(Transform target, float speed, Action<Enemy> onReached, int dashCount = 3, float dashDistance = 3f, float dashSpeed = 10f, float pauseBetweenDashes = 0.2f)
    {
        var dashMove = new DashMove(dashCount, dashDistance, dashSpeed, pauseBetweenDashes);
        base.Initialize(target, speed, onReached, dashMove);
    }
}
