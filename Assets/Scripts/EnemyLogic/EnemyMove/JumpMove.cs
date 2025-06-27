using System;
using System.Collections;
using UnityEngine;

public class JumpMove : IMoveBehavior
{
    private Vector3 mid;
    private Vector3 jumpTarget;

    public JumpMove(Vector3 mid, Vector3 jumpTarget)
    {
        this.mid = mid;
        this.jumpTarget = jumpTarget;
    }

    public IEnumerator Move(Enemy enemy, Transform target, float speed, Action<Enemy> onComplete)
    {
        // 1. Move to mid point
        while ((enemy.transform.position - mid).sqrMagnitude > 0.01f)
        {
            Vector3 direction = (mid - enemy.transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRot, Time.deltaTime * 10f);
            }

            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, mid, speed * Time.deltaTime);
            yield return null;
        }

        // 2. Jump arc
        Vector3 start = enemy.transform.position;
        float jumpTime = 0f;
        while (jumpTime < 1f)
        {
            jumpTime += Time.deltaTime / 0.5f;

            Vector3 flat = Vector3.Lerp(start, jumpTarget, jumpTime);
            float height = Mathf.Sin(jumpTime * Mathf.PI) * 1.5f;
            Vector3 nextPos = flat + Vector3.up * height;

            // 朝向跳跃方向
            Vector3 direction = (nextPos - enemy.transform.position).normalized;
            if (direction != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(direction, Vector3.up);
                enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRot, Time.deltaTime * 10f);
            }

            enemy.transform.position = nextPos;
            yield return null;
        }

        // 3. Final chase to target
        yield return new DirectMove().Move(enemy, target, speed, onComplete);
    }
}
