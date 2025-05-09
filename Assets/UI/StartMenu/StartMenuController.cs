using MoreMountains.Feedbacks;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using UnityEngine.Localization;
using UnityEngine.Localization.Settings;
public class StartMenuController : MonoBehaviour
{
    public MMF_Player startFeedBack;
    public MMF_Player exitFeedBack; // 退出反馈
    private bool isTransitioning = false;

    void OnEnable()
    {
        isTransitioning = false; // 重置状态
        var root = GetComponent<UIDocument>().rootVisualElement;
        var titleLabel = root.Q<Label>("gameTitle");
        var startButton = root.Q<Button>("startButton");
        var exitButton = root.Q<Button>("exitButton");
        var languageButton = root.Q<Button>("languageButton");
        startButton.clicked += () =>
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                StartCoroutine(WaitUntilStartFeedbacksEnd());
            }
        };

        exitButton.clicked += () =>
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                StartCoroutine(WaitUntilExitFeedbacksEnd());
            }
        };
        languageButton.clicked += LanguageToggle.ToggleLanguage;
        // 绑定本地化文本
        UILocalizationHelper.BindLocalizedText(titleLabel, "LocalizationTables", "gameTitle");
        UILocalizationHelper.BindLocalizedText(startButton, "LocalizationTables", "startButton");
        UILocalizationHelper.BindLocalizedText(exitButton, "LocalizationTables", "exitButton");
        UILocalizationHelper.BindLocalizedText(languageButton, "LocalizationTables", "languageButton");
    }

    private IEnumerator WaitUntilStartFeedbacksEnd()
    {
        startFeedBack.PlayFeedbacks();
        yield return new WaitUntil(() => !startFeedBack.IsPlaying);
        SceneManager.LoadScene("MainMenu"); // 这里是主菜单场景的名称
    }

    private IEnumerator WaitUntilExitFeedbacksEnd()
    {
        exitFeedBack.PlayFeedbacks();
        yield return new WaitUntil(() => !exitFeedBack.IsPlaying);
        Application.Quit(); // 退出游戏
    }
}
