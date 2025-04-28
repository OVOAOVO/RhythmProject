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

        
        // 拿到各个 Label，用 Q<T>，name 对应 UXML 中的 name 属性
        var comboLabel   = root.Q<Label>("comboLabel");
        var perfectLabel = root.Q<Label>("perfectLabel");
        var goodLabel    = root.Q<Label>("goodLabel");
        var badLabel     = root.Q<Label>("badLabel");

        if(ResultDataManager.Instance != null)
        {
            // 从单例里读值，刷新文本
            comboLabel.text   = $"Max Combo: {ResultDataManager.Instance.MaxCombo}";
            perfectLabel.text = $"Perfect: {ResultDataManager.Instance.PerfectCount}";
            goodLabel.text    = $"Good: {ResultDataManager.Instance.GoodCount}";
            badLabel.text     = $"Bad: {ResultDataManager.Instance.BadCount}";
            retryButton.clicked += () => OnButtonClicked(retryFeedback, ResultDataManager.LastPlayedSceneName);
        }
        else
        {
            retryButton.clicked += () => OnButtonClicked(retryFeedback, "Game");
        }

        exitButton.clicked += () => OnButtonClicked(mainMenuFeedback, "MainMenu");
    }

    private void OnButtonClicked(MMF_Player feedback, string sceneName)
    {
        if (isTransitioning) return;

        isTransitioning = true;
        targetScene = sceneName;
        //ResultDataManager.Instance.ResetAll(); // 重置数据
        StartCoroutine(PlayFeedbackAndLoadScene(feedback));
    }

    private IEnumerator PlayFeedbackAndLoadScene(MMF_Player feedback)
    {
        feedback.PlayFeedbacks();
        yield return new WaitUntil(() => !feedback.IsPlaying);
        SceneManager.LoadScene(targetScene);
    }
}
