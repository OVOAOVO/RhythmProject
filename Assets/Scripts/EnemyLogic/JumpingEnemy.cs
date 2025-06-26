using System.Collections;
using UnityEngine;

public class JumpingEnemy : Enemy
{
    private enum Phase { MoveLeft, Jump, Chase }

    private Vector3 midPos;
    private Vector3 jumpTarget;
    private Vector3 jumpStart;
    private float jumpTime;
    private void OnEnable()
    {
        AutoShoot.RegisterEnemy(this);
    }

    private void OnDisable()
    {
        AutoShoot.UnregisterEnemy(this);
    }

    public void InitializeJump(Vector3 mid, Vector3 jumpTo, Transform finalTarget, float speed, System.Action<Enemy> onReached)
    {     
        // 调用基类初始化
        // 手动给基类字段赋值，但不要启动协程
        this.target = finalTarget;
        this.moveSpeed = speed;
        this.onReachedTarget = onReached;
        midPos = mid;
        jumpTarget = jumpTo;
        jumpTime = 0f;

        StopAllCoroutines();
        // 开始协程来处理整个过程
        StartCoroutine(JumpSequence());
    }

    private IEnumerator JumpSequence()
    {
        // MoveLeft阶段
        yield return MoveToMidPos();

        // Jump阶段
        yield return JumpToTarget();

        // 跳跃结束后，进入基类的追击逻辑
        base.Initialize(target, moveSpeed, onReachedTarget);
    }

    private IEnumerator MoveToMidPos()
    {
        while ((transform.position - midPos).sqrMagnitude > 0.01f)
        {
            Vector3 dir = (midPos - transform.position).normalized;
            if (dir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
            }

            transform.position = Vector3.MoveTowards(transform.position, midPos, moveSpeed * Time.deltaTime);
            yield return null;
        }

        jumpStart = transform.position;
        jumpTime = 0f;
    }

    private IEnumerator JumpToTarget()
    {
        while (jumpTime < 1f)
        {
            jumpTime += Time.deltaTime / 0.5f;

            Vector3 flatPos = Vector3.Lerp(jumpStart, jumpTarget, jumpTime);
            float height = Mathf.Sin(Mathf.PI * jumpTime) * 1.5f;
            Vector3 nextPos = flatPos + new Vector3(0, height, 0);

            Vector3 dir = (nextPos - transform.position).normalized;
            if (dir != Vector3.zero)
            {
                Quaternion targetRot = Quaternion.LookRotation(dir, Vector3.up);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * 10f);
            }

            transform.position = nextPos;
            yield return null;
        }
    }
}
