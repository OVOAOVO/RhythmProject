using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuController : MonoBehaviour
{
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var startButton = root.Q<Button>("startButton");

        startButton.clicked += () =>
        {
            Debug.Log("进入游戏！");
            // TODO: 切换场景或执行逻辑
            // SceneManager.LoadScene("MainGameScene");
        };
    }
}
