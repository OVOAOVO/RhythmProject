using System;
using UnityEngine;
using DG.Tweening;
using System.Collections;
public class DashMove : IMoveBehavior
{
    private int dashCount;
    private float dashDistance;
    private float dashSpeed;
    private float pauseBetweenDashes;

    public DashMove(int dashCount = 3, float dashDistance = 3f, float dashSpeed = 3f, float pause = 0.5f)
    {
        this.dashCount = dashCount;
        this.dashDistance = dashDistance;
        this.dashSpeed = dashSpeed;
        this.pauseBetweenDashes = pause;
    }

    public IEnumerator Move(Enemy enemy, Transform target, float speed, Action<Enemy> onComplete)
    {
        for (int i = 0; i < dashCount; i++)
        {
            Vector3 enemyPos = enemy.transform.position;
            Vector3 targetPos = target.position;

            Vector3 toTarget = (new Vector3(targetPos.x, targetPos.y, enemyPos.z) - enemyPos).normalized;

            // 限制偏转角度，避免往回冲刺
            float maxAngleOffset = 30f;
            float angleToTarget = Mathf.Atan2(toTarget.y, toTarget.x) * Mathf.Rad2Deg;
            float randomAngle = UnityEngine.Random.Range(-maxAngleOffset, maxAngleOffset);
            float finalAngle = angleToTarget + randomAngle;
            float rad = finalAngle * Mathf.Deg2Rad;

            Vector3 dashDir = new Vector3(Mathf.Cos(rad), Mathf.Sin(rad), 0).normalized;

            if (dashDir != Vector3.zero)
            {
                float angle = Mathf.Atan2(dashDir.y, dashDir.x) * Mathf.Rad2Deg;
                enemy.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            }

            Vector3 dashEnd = enemyPos + dashDir * dashDistance;

            float duration = dashDistance / dashSpeed;

            Tween tween = enemy.transform.DOMove(new Vector3(dashEnd.x, dashEnd.y, enemyPos.z), duration)
                .SetEase(Ease.OutQuad)
                .SetLink(enemy.gameObject);

            yield return tween.WaitForCompletion();
            yield return new WaitForSeconds(pauseBetweenDashes);
        }
        // 最后用普通 DirectMove 继续追击
        yield return new DirectMove().Move(enemy, target, speed, onComplete);
    }
}
