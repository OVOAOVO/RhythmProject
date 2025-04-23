using UnityEngine;

public class RotationQuad : MonoBehaviour
{
    public Transform quadCenter;
    
    void Update()
    {
        // 获取鼠标屏幕坐标
        Vector3 mouseScreen = Input.mousePosition;

        // 获取四边形中心的屏幕坐标
        Vector3 quadScreen = Camera.main.WorldToScreenPoint(quadCenter.position);

        // 计算鼠标相对于四边形中心的屏幕坐标差
        Vector2 mouseUV = new Vector2(mouseScreen.x - quadScreen.x, mouseScreen.y - quadScreen.y);

        // 计算偏移的长度
        float length = mouseUV.magnitude;

        // 如果鼠标没有在四边形中心直接上方，计算旋转角度
        if (length != 0)
        {
            // 归一化鼠标偏移
            mouseUV = new Vector2(mouseUV.x / length, mouseUV.y / length);

            // 计算角度 (使用反正切函数 atan2 计算角度)
            float angle = Mathf.Atan2(mouseUV.y, mouseUV.x) * Mathf.Rad2Deg;

            // 只绕 X 轴旋转，根据角度调整
            transform.rotation = Quaternion.Euler( 180-angle, 90,-90 ); // 加上 -90 使起始位置正确
        }
    }
}
