using UnityEngine;

public class Gun : MonoBehaviour
{
    public float interval = 0.2f;

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
        if (Time.timeScale == 0f) return; // 如果游戏暂停了，不执行枪械逻辑
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

        if (IsFireKeyPressed() && timer == 0)
        {
            timer = interval;
            Fire();
        }
    }

    protected virtual bool IsFireKeyPressed()
    {
        // 遍历所有 KeyCode，排除不想要的控制键
        foreach (KeyCode key in System.Enum.GetValues(typeof(KeyCode)))
        {
            if (Input.GetKey(key))
            {
                // 排除 Escape、控制键、功能键等
                if (key == KeyCode.Escape ||
                    key == KeyCode.LeftAlt || key == KeyCode.RightAlt ||
                    key == KeyCode.LeftControl || key == KeyCode.RightControl ||
                    key == KeyCode.LeftShift || key == KeyCode.RightShift ||
                    key == KeyCode.Tab || key == KeyCode.CapsLock)
                {
                    continue;
                }

                return true; // 有按下的有效按键
            }
        }
        return false;
    }

    
    protected virtual void Fire()
    {
        // GameObject bullet = ObjectPool.Instance.GetGameObject(bulletPrefab);
        // bullet.transform.position = muzzlePos.position;

        //bullet.GetComponent<Bullet>().SetSpeed(shootDir);
    }
}
