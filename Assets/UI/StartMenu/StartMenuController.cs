using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
public class StartMenuController : MonoBehaviour
{
    public MMFeedbacks feedbacks; // 反馈系统
    void OnEnable()
    {
        var root = GetComponent<UIDocument>().rootVisualElement;

        var startButton = root.Q<Button>("startButton");

        startButton.clicked += () =>
        {
            StartCoroutine(WaitUntilFeedbacksEnd());
        };
    }

    private IEnumerator WaitUntilFeedbacksEnd()
    {
        feedbacks.PlayFeedbacks();
        yield return new WaitUntil(() => !feedbacks.IsPlaying);
        SceneManager.LoadScene("Game");
    }
}
