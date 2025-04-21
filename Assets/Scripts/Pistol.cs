using UnityEditor;
using UnityEngine;


public class Pistol : Gun
{
    protected override void Fire()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(muzzlePos.position, shootDir, out hit, 30);  // 使用 3D 射线

        // 设置LineRenderer的终点
        SetTracer(isHit ? hit.point : new Vector3(mousePos.x, mousePos.y, 0.0f));
        if (isHit)
        {
            ObjectPool.Instance.PushObject(hit.collider.gameObject);  // 将射中物体放入对象池
            Debug.Log("Hit and object destroyed!");
        }
        else
        {
            Debug.Log("No hit!");
        }
    }

    private void SetTracer(Vector3 endPosition)
    {
        GameObject bullet = ObjectPool.Instance.GetGameObject(bulletPrefab);
        LineRenderer tracer = bullet.GetComponent<LineRenderer>();
        tracer.SetPosition(0, muzzlePos.position);  // 设置起始点
        tracer.SetPosition(1, endPosition);  // 设置终点
    }
}