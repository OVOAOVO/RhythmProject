using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;
using System.Collections;

public class ResultPanel : MonoBehaviour
{
    public MMF_Player retryFeedback;
    public MMF_Player mainMenuFeedback;
    public MMF_Player quitFeedback;
    private bool isTransitioning = false;
    private string targetScene = "";

    private void OnEnable()
    {
        isTransitioning = false;
        var root = GetComponent<UIDocument>().rootVisualElement;

        var titleLabel = root.Q<Label>("titleLabel");
        var retryButton = root.Q<Button>("retryButton");
        var mainMenuButton = root.Q<Button>("exitButton");
        var quitButton = root.Q<Button>("quitButton");
        
        // 拿到各个 Label，用 Q<T>，name 对应 UXML 中的 name 属性
        var comboLabel   = root.Q<Label>("comboLabel");
        var perfectLabel = root.Q<Label>("perfectLabel");
        var goodLabel    = root.Q<Label>("goodLabel");
        var badLabel     = root.Q<Label>("badLabel");
        var pureLabel  = root.Q<Label>("scoreLabel");
        if(ResultDataManager.Instance != null)
        {
            retryButton.clicked += () => OnButtonClicked(retryFeedback, ResultDataManager.LastPlayedSceneName);
        }
        else
        {
            retryButton.clicked += () => OnButtonClicked(retryFeedback, "Game");
        }

        mainMenuButton.clicked += () => OnButtonClicked(mainMenuFeedback, "MainMenu");

        quitButton.clicked += () =>
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                StartCoroutine(WaitUntilExitFeedbacksEnd());
            }
        };

        // 绑定本地化文本
        UILocalizationHelper.BindLocalizedText(titleLabel, "LocalizationTables", "ResultLabel");
        UILocalizationHelper.BindLocalizedText(retryButton, "LocalizationTables", "retryButton");
        UILocalizationHelper.BindLocalizedText(mainMenuButton, "LocalizationTables", "MainMenuButton");
        UILocalizationHelper.BindLocalizedText(quitButton, "LocalizationTables", "quitButton");

        UILocalizationHelper.SetLocalizedFormattedTextValue(comboLabel, "LocalizationTables", "comboLabel", ResultDataManager.Instance.MaxCombo);
        UILocalizationHelper.SetLocalizedFormattedTextValue(perfectLabel, "LocalizationTables", "perfectLabel", ResultDataManager.Instance.PerfectCount);
        UILocalizationHelper.SetLocalizedFormattedTextValue(goodLabel, "LocalizationTables", "goodLabel", ResultDataManager.Instance.GoodCount);
        UILocalizationHelper.SetLocalizedFormattedTextValue(badLabel, "LocalizationTables", "badLabel", ResultDataManager.Instance.BadCount);
        UILocalizationHelper.SetLocalizedFormattedTextValue(pureLabel, "LocalizationTables", "scoreLabel", ResultDataManager.Instance.Pure);
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

    private IEnumerator WaitUntilExitFeedbacksEnd()
    {
        quitFeedback.PlayFeedbacks();
        yield return new WaitUntil(() => !quitFeedback.IsPlaying);
        Application.Quit(); // 退出游戏
    }
}
