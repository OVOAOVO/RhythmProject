using UnityEngine;
using System.Collections;
public class HitTextController : MonoBehaviour
{
    public float displayDuration = 0.2f;  // 默认显示时间

    private void OnEnable()
    {
        // 启动协程，显示一段时间后销毁或回收到对象池
        StartCoroutine(DestroyAfterTime(displayDuration));
    }

    private IEnumerator DestroyAfterTime(float time)
    {
        // 等待指定时间
        yield return new WaitForSeconds(time);

        // 回收对象到对象池
        ObjectPool.Instance.PushObject(this.gameObject);
    }
}
