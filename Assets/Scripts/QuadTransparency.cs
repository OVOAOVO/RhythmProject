using UnityEngine;

public class MouseDirectionControllerXY : MonoBehaviour
{
    public Transform quadCenter;
    public Material material;
    public float angleThreshold;
    void Update()
    {

        // 获取鼠标屏幕坐标
        Vector3 mousePos = Input.mousePosition;

        // 将物体中心点从世界坐标转换为屏幕坐标
        Vector3 centerScreen = Camera.main.WorldToScreenPoint(quadCenter.position);
        // 计算鼠标相对于物体中心的屏幕空间向量
        Vector2 mouseUV = new Vector2(mousePos.x - centerScreen.x, mousePos.y - centerScreen.y);

        // 计算原始向量的长度
        float length = mouseUV.magnitude;

        // 检查长度是否为 0，以避免除以 0
        if (length != 0)
        {
            mouseUV = new Vector2(mouseUV.x / length, mouseUV.y / length);
        }

        float radiusInRadians = angleThreshold;

        // 更新材质参数
        material.SetVector("_MouseVector", new Vector4(mouseUV.x, mouseUV.y, 0, 0));
        material.SetFloat("_Radius", radiusInRadians);
    }
}
