using UnityEngine;

public class MouseDirectionControllerXY : MonoBehaviour
{
    public Transform quadCenter;
    public Material material;
    public float angleThreshold;
    void Update()
    {
        // ��ȡ�����Ļ����
        Vector3 mouseScreen = Input.mousePosition;

        Vector3 quadScreen = Camera.main.WorldToScreenPoint(quadCenter.position);

        // ������������ quadScreen ����ϵ��ƫ����
        Vector2 mouseUV = new Vector2(mouseScreen.x - quadScreen.x, mouseScreen.y - quadScreen.y);

        // ����ԭʼ�����ĳ���
        float length = mouseUV.magnitude;

        // ��鳤���Ƿ�Ϊ 0���Ա������ 0
        if (length != 0)
        {
            mouseUV = new Vector2(mouseUV.x / length, mouseUV.y / length);
        }
        float radiusInRadians = angleThreshold * Mathf.Deg2Rad;
        
        // ���²��ʲ���
        material.SetVector("_MouseVector", new Vector4(mouseUV.x, mouseUV.y, 0, 0));
        material.SetFloat("_RadiusInRadians", radiusInRadians);
    }
}
