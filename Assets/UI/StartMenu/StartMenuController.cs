using UnityEngine;
using UnityEngine.UIElements; // 引入 UIElements 命名空间
public class StartMenuController : MonoBehaviour
{

    void OnEnable()
    {
        // 获取当前界面的 UI 根元素
        var root = GetComponent<UIDocument>().rootVisualElement;

        var label = root.Q<Label>("myLabel");
        var button = root.Q<Button>("myButton");

        // 注册按钮点击事件
        button.clicked += () =>
        {
            label.text = "你点了按钮 ";
        };
    }
}
