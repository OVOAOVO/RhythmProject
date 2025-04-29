using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Transform target;
    protected float moveSpeed;
    protected System.Action<Enemy> onReachedTarget;

    public virtual void Initialize(Transform target, float speed, System.Action<Enemy> onReached = null)
    {
        this.target = target;
        this.moveSpeed = speed;
        this.onReachedTarget = onReached;
        StopAllCoroutines(); // 防止重复启动协程
        StartCoroutine(MoveToTarget());
    }

    protected  IEnumerator MoveToTarget()
    {
        while ((transform.position - target.position).sqrMagnitude > 0.01f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, moveSpeed * Time.deltaTime);
            yield return null;
        }

        transform.position = target.position;
        onReachedTarget?.Invoke(this);
    }
}
