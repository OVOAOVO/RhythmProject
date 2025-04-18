using UnityEditor;
using UnityEngine;


public class Pistol : Gun
{
    protected override void Fire()
    {
        RaycastHit2D hit2D = Physics2D.Raycast(muzzlePos.position, shootDir,30);
    
        GameObject bullet = ObjectPool.Instance.GetGameObject(bulletPrefab);
        LineRenderer tracer = bullet.GetComponent<LineRenderer>();
        tracer.SetPosition(0, muzzlePos.position);
        if (hit2D.collider != null)
        {
            tracer.SetPosition(1, hit2D.point);
        }
        else
        {
            tracer.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 0.0f));
        }
    }
}