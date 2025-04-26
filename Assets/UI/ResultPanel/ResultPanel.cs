using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;
using System.Collections;

public class ResultPanel : MonoBehaviour
{
    public MMF_Player retryFeedback;
    public MMF_Player mainMenuFeedback;

    private bool isTransitioning = false;
    private string targetScene = "";

    private void OnEnable()
    {
        isTransitioning = false;
        var root = GetComponent<UIDocument>().rootVisualElement;

        var retryButton = root.Q<Button>("retryButton");
        var exitButton = root.Q<Button>("exitButton");

        retryButton.clicked += () => OnButtonClicked(retryFeedback, "Game");
        exitButton.clicked += () => OnButtonClicked(mainMenuFeedback, "MainMenu");
    }

    private void OnButtonClicked(MMF_Player feedback, string sceneName)
    {
        if (isTransitioning) return;

        isTransitioning = true;
        targetScene = sceneName;
        StartCoroutine(PlayFeedbackAndLoadScene(feedback));
    }

    private IEnumerator PlayFeedbackAndLoadScene(MMF_Player feedback)
    {
        feedback.PlayFeedbacks();
        yield return new WaitUntil(() => !feedback.IsPlaying);
        SceneManager.LoadScene(targetScene);
    }
}
