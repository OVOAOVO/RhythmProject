using UnityEditor;
using UnityEngine;


public class EnemyPistol : Gun
{
    // int lastHit = -1;
    // protected override void Shooting()
    // {
    //     if(timer != 0)
    //     {
    //         timer -= Time.deltaTime;
    //         if(timer <= 0)
    //             timer = 0;
    //     }

    //     int currentHit = Conductor.Instance.hit;
    //     if(currentHit > lastHit && timer == 0)
    //     {
    //         timer = interval;
    //         Fire();
    //         lastHit = currentHit;
    //     }
    // }

    // protected override void Fire()
    // {
    //     RaycastHit2D hit2D = Physics2D.Raycast(muzzlePos.position, shootDir,30);
    
    //     GameObject bullet = ObjectPool.Instance.GetGameObject(bulletPrefab);
    //     LineRenderer tracer = bullet.GetComponent<LineRenderer>();
    //     tracer.SetPosition(0, muzzlePos.position);
    //     if (hit2D.collider != null)
    //     {
    //         tracer.SetPosition(1, hit2D.point);
    //     }
    //     else
    //     {
    //         tracer.SetPosition(1, new Vector3(mousePos.x, mousePos.y, 0.0f));
    //     }
    // }
}