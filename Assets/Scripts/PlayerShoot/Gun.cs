using UnityEngine;

public class Gun : MonoBehaviour
{
    public float interval;

    public GameObject bulletPrefab;
    protected Transform muzzlePos;
    protected Vector2 mousePos;
    protected Vector2 shootDir;

    protected float timer;
    
    protected virtual void Start()
    {
        muzzlePos = transform.Find("MuzzlePos");
    }

    protected virtual void Update()
    {
        Vector3 mp = Input.mousePosition;
        mp.z = -Camera.main.transform.position.z;       // 摄像机到 z=0 平面的距离
        mousePos = Camera.main.ScreenToWorldPoint(mp);

        shootDir = (mousePos - (Vector2)muzzlePos.position).normalized;
        Shooting();
    }

    protected virtual void Shooting()
    {
        if(timer != 0)
        {
            timer -= Time.deltaTime;
            if(timer <= 0)
                timer = 0;
        }

        if(Input.GetMouseButton(0) && timer == 0)
        {
            timer = interval;
            Fire();
        }
    }

    protected virtual void Fire()
    {
        // GameObject bullet = ObjectPool.Instance.GetGameObject(bulletPrefab);
        // bullet.transform.position = muzzlePos.position;

        //bullet.GetComponent<Bullet>().SetSpeed(shootDir);
    }
}
