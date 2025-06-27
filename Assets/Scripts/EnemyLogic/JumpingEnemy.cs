using System;
using UnityEngine;

public class JumpingEnemy : Enemy
{
    public void InitializeJump(Vector3 mid, Vector3 jumpTo, Transform target, float speed, Action<Enemy> onReached)
    {
        var jumpMove = new JumpMove(mid, jumpTo);
        base.Initialize(target, speed, onReached, jumpMove);
    }
}
