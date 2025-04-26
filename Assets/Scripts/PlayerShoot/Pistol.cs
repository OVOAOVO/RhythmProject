using UnityEditor;
using UnityEngine;
using UnityEngine.UI;  // 引入UI命名空间（如果需要使用UI元素）
using MoreMountains.Feedbacks;

public class Pistol : Gun
{
    public GameObject HitBad;  // 用于击中判断的Prefab（包含Canvas和Text）
    public GameObject HitGood; 
    public GameObject HitPerfect;  
    
    [Header("Feedback to Play on Click")]
    public MMFeedbacks feedbacks;// 反馈系统

    protected override void Start()
    {
        base.Start();  // 先执行父类逻辑（比如找到 muzzle）       
    }

    protected override void Fire()
    {
        RaycastHit hit;
        bool isHit = Physics.Raycast(muzzlePos.position, shootDir, out hit, 30);  // 使用 3D 射线
        // 设置LineRenderer的终点
        SetTracer(isHit ? hit.point : new Vector3(mousePos.x, mousePos.y, 0.0f));
        if (isHit)
        {
            ObjectPool.Instance.PushObject(hit.collider.gameObject);  // 将射中物体放入对象池

              // 获取物体的中心位置和中心线方向
            Vector3 objectCenter = hit.collider.transform.position;
            Vector3 centerLineDirection = hit.collider.transform.forward;  // 物体的前方方向（中心线）

            // 计算射中点到物体中心线的垂直距离
            // TODO:这个距离最好是不要这样写死
            float distanceToCenterLine = CalculateDistanceToCenterLine(hit.point, objectCenter, centerLineDirection);

            // 处理击中效果和连击数
            HandleHitEffectAndCombo(distanceToCenterLine, hit.point);
        }
        else
        {
            ResultDataManager.Instance.ResetCombo();
        }
        feedbacks.PlayFeedbacks();// 播放反馈    
    }

    private void HandleHitEffectAndCombo(float distanceToCenterLine, Vector3 hitPoint)
    {
        GameObject hitType = null;  // 用于存储击中效果的Prefab
        
        //TODO: 这里最好不要按照固定的数值，需要再改
        if (distanceToCenterLine > 0.3f)
        {
            hitType = HitBad;
            ResultDataManager.Instance.AddBad();      // 记一次 Bad
        }
        else if (distanceToCenterLine > 0.1f && distanceToCenterLine <= 0.3f)
        {
            hitType = HitGood;
            ResultDataManager.Instance.AddGood();     // 记一次 Good
        }
        else
        {
            hitType = HitPerfect;
            ResultDataManager.Instance.AddPerfect();  // 记一次 Perfect
        }
        
        ShowDamageText(hitPoint, hitType);  // 显示击中效果
    }


    private void SetTracer(Vector3 endPosition)
    {
        GameObject bullet = ObjectPool.Instance.GetGameObject(bulletPrefab);
        LineRenderer tracer = bullet.GetComponent<LineRenderer>();
        tracer.SetPosition(0, muzzlePos.position);  // 设置起始点
        tracer.SetPosition(1, endPosition);  // 设置终点
    }

    private void ShowDamageText(Vector3 position,GameObject HitType)
    {
        GameObject instance = ObjectPool.Instance.GetGameObject(HitType);
        // 将世界坐标转换为屏幕坐标
        Vector3 screenPos = Camera.main.WorldToScreenPoint(position);

        // 设置文本的屏幕位置
        Transform textTransform = instance.transform.Find("HitText");
        if (textTransform != null)
        {
            RectTransform rectTransform = textTransform.GetComponent<RectTransform>();
            rectTransform.position = screenPos;
        }
        else
        {
            Debug.LogWarning("找不到 HitText 子物体！");
        }

        HitTextController controller = instance.GetComponent<HitTextController>(); //这里协程调用自动控制显示时间
    }

    private float CalculateDistanceToCenterLine(Vector3 hitPoint, Vector3 objectCenter, Vector3 centerLineDirection)
    {
        // 计算射中点到物体中心点的向量
        Vector3 toHitPoint = hitPoint - objectCenter;

        // 计算射中点到中心线的垂直距离
        Vector3 perpendicularToCenterLine = Vector3.ProjectOnPlane(toHitPoint, centerLineDirection);

        // 返回距离的大小（即垂直距离）
        return perpendicularToCenterLine.magnitude;
    }

}