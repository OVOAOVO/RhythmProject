using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;

public class StartMenuController : MonoBehaviour
{
    public MMFeedbacks feedbacks;
    private bool isTransitioning = false;

    void OnEnable()
    {
        isTransitioning = false; // 重置状态
        var root = GetComponent<UIDocument>().rootVisualElement;
        var startButton = root.Q<Button>("startButton");

        startButton.clicked += () =>
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                StartCoroutine(WaitUntilFeedbacksEnd());
            }
        };
    }

    private IEnumerator WaitUntilFeedbacksEnd()
    {
        feedbacks.PlayFeedbacks();
        yield return new WaitUntil(() => !feedbacks.IsPlaying);
        SceneManager.LoadScene("MainMenu"); // 这里是主菜单场景的名称
    }
}
