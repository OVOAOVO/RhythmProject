using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UIElements;

public class StartMenuController : MonoBehaviour
{
    public MMFeedbacks feedbacks; // 反馈系统
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var startButton = root.Q<Button>("startButton");

        startButton.clicked += () =>
        {
            Debug.Log("进入游戏！");
            // TODO: 切换场景或执行逻辑
            // SceneManager.LoadScene("MainGameScene");
            feedbacks.PlayFeedbacks(); // 播放反馈
        };
    }
}
