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
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
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
