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

            Vector3 toTarget = (new Vector3(targetPos.x, targetPos.y, 0) - new Vector3(enemyPos.x, enemyPos.y, 0)).normalized;
            Vector2 randomOffset = UnityEngine.Random.insideUnitCircle;
            Vector3 dashDir = (toTarget + new Vector3(randomOffset.x, randomOffset.y, 0) * 0.5f).normalized;
            Vector3 dashEnd = enemyPos + dashDir * dashDistance;

            if (dashDir != Vector3.zero)
            {
                float angle = Mathf.Atan2(dashDir.y, dashDir.x) * Mathf.Rad2Deg;
                enemy.transform.rotation = Quaternion.Euler(0, 0, angle - 90f);
            }

            float duration = dashDistance / dashSpeed;

            Tween tween = enemy.transform.DOMove(new Vector3(dashEnd.x, dashEnd.y, enemyPos.z), duration)
                .SetEase(Ease.OutQuad)
                .SetLink(enemy.gameObject);  // <== 关键代码

            yield return tween.WaitForCompletion(); // 等待 tween 完成
            yield return new WaitForSeconds(pauseBetweenDashes);
        }

        // 最后用普通 DirectMove 继续追击
        yield return new DirectMove().Move(enemy, target, speed, onComplete);
    }
}
