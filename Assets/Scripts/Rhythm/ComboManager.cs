using UnityEngine;
using UnityEngine.UI;

public class ComboManager : MonoBehaviour
{
    private Text comboText;

    // 初始化 Combo 管理器，绑定到 UI 文本组件
    public void Init(string comboTextPath)
    {
        GameObject comboObj = GameObject.Find(comboTextPath);
        if (comboObj != null)
        {
            comboText = comboObj.GetComponent<Text>();
        }
        else
        {
            Debug.LogWarning("Combo Text not found at path: " + comboTextPath);
        }
    }

    // 增加 Combo 数量
    public void IncrementCombo()
    {
        if (comboText == null) return;

        int current = 0;
        int.TryParse(comboText.text, out current);
        current++;
        UpdateCombo(current);
    }

    // 更新 Combo 文本
    public void UpdateCombo(int value)
    {
        if (comboText != null)
        {
            comboText.text = value.ToString();
        }
    }

    // 重置 Combo
    public void ResetCombo()
    {
        UpdateCombo(0);
    }
}
