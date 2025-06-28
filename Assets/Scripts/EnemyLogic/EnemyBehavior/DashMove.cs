using System;
using System.Collections;
using UnityEngine;

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

            // 计算 XY 平面上的方向（忽略 Z 轴差异）
            Vector3 toTarget = (new Vector3(targetPos.x, targetPos.y, 0) - new Vector3(enemyPos.x, enemyPos.y, 0)).normalized;
            Vector3 randomOffset = UnityEngine.Random.insideUnitCircle; // XY平面随机偏移
            Vector3 dashDir = (toTarget + new Vector3(randomOffset.x, randomOffset.y, 0) * 0.5f).normalized;

            // 计算冲刺终点，Z轴保持不变
            Vector3 dashEnd = new Vector3(enemyPos.x, enemyPos.y, enemyPos.z) + dashDir * dashDistance;

            // 朝向冲刺方向（只考虑XY平面旋转，保持Z轴不变）
            if (dashDir != Vector3.zero)
            {
                float angle = Mathf.Atan2(dashDir.y, dashDir.x) * Mathf.Rad2Deg;
                enemy.transform.rotation = Quaternion.Euler(0, 0, angle - 90f); // -90度根据模型朝向调整
            }

            // 冲刺移动，位置更新只影响X和Y，Z不变
            while ((new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0) - new Vector3(dashEnd.x, dashEnd.y, 0)).sqrMagnitude > 0.01f)
            {
                Vector3 newPos = Vector3.MoveTowards(new Vector3(enemy.transform.position.x, enemy.transform.position.y, 0), new Vector3(dashEnd.x, dashEnd.y, 0), dashSpeed * Time.deltaTime);
                enemy.transform.position = new Vector3(newPos.x, newPos.y, enemy.transform.position.z);
                yield return null;
            }

            yield return new WaitForSeconds(pauseBetweenDashes);
        }

        // 最后用普通移动追击目标（需要保证DirectMove的Move也是在XY平面）
        yield return new DirectMove().Move(enemy, target, speed, onComplete);
    }
}

