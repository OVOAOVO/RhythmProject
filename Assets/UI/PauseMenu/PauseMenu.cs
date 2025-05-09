using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using MoreMountains.Feedbacks;
using System.Collections;
using UnityEngine.EventSystems;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    private VisualElement pauseMenu;
    public MMF_Player recoverFeedback;
    public MMF_Player MainMenuFeedback;
    public MMF_Player quitFeedback;
    private bool isTransitioning = false;   
    
    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        if (uiDocument == null)
        {
            Debug.LogError("UIDocument component is missing!");
            return;
        }

        var root = uiDocument.rootVisualElement;
        
        // 获取 pauseMenu 元素
        pauseMenu = root.Q<VisualElement>("VisualElement");
        if (pauseMenu == null)
        {
            Debug.LogError("Pause menu (results-panel) not found!");
            return;
        }

        var retryButton = root.Q<Button>("retryButton");
        var quitButton = root.Q<Button>("quitButton");
        var mainMenuButton = root.Q<Button>("MainMenuButton");
        var pauseLabel = root.Q<Label>("titleLabel");

        recoverFeedback.ForceTimescaleMode = true;
        quitFeedback.ForceTimescaleMode = true;
        MainMenuFeedback.ForceTimescaleMode = true;

        retryButton.clicked += () => OnButtonClicked(recoverFeedback);
        quitButton.clicked += () =>
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                StartCoroutine(WaitUntilExitFeedbacksEnd());
            }
        };
        mainMenuButton.clicked += () => OnMainButtonClicked(MainMenuFeedback);
        UILocalizationHelper.BindLocalizedText(retryButton, "LocalizationTables", "continueButton");
        UILocalizationHelper.BindLocalizedText(quitButton, "LocalizationTables", "quitButton");
        UILocalizationHelper.BindLocalizedText(mainMenuButton, "LocalizationTables", "MainMenuButton");
        UILocalizationHelper.BindLocalizedText(pauseLabel, "LocalizationTables", "titleLabel");
    }

    void Update()
    {
        // 打印一下确认输入是否正常响应
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Debug.Log("Escape key pressed");
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (pauseMenu == null)
            return;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.style.display = DisplayStyle.Flex;

            // 暂停音乐
            if (Conductor.Instance != null)
                Conductor.Instance.PauseMusic();
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.style.display = DisplayStyle.None;

            // 继续音乐
            if (Conductor.Instance != null)
                Conductor.Instance.ResumeMusic();
        }
    }

    private void OnButtonClicked(MMF_Player feedback)
    {
        if (isTransitioning) return;

        isTransitioning = true;
        StartCoroutine(PlayFeedbackAndReocver(feedback));
    }

    private void OnMainButtonClicked(MMF_Player feedback)
    {
        if (isTransitioning) return;

        isTransitioning = true;
        StartCoroutine(PlayFeedbackAndLoadScene(feedback));
    }

    private IEnumerator PlayFeedbackAndReocver(MMF_Player feedback)
    {
        feedback.PlayFeedbacks();
        yield return new WaitUntil(() => !feedback.IsPlaying);
        
        isTransitioning = false;
        isPaused = !isPaused;
        Time.timeScale = 1f;
        pauseMenu.style.display = DisplayStyle.None;

        // 继续音乐
        if (Conductor.Instance != null)
            Conductor.Instance.ResumeMusic();
    }

        private IEnumerator PlayFeedbackAndLoadScene(MMF_Player feedback)
    {
        feedback.PlayFeedbacks();
        yield return new WaitUntil(() => !feedback.IsPlaying);
        
        isTransitioning = false;
        isPaused = !isPaused;
        Time.timeScale = 1f;
        pauseMenu.style.display = DisplayStyle.None;

        // 继续音乐
        if (Conductor.Instance != null)
            Conductor.Instance.ResumeMusic();
        SceneManager.LoadScene("MainMenu"); // 加载主菜单场景
    }

    private IEnumerator WaitUntilExitFeedbacksEnd()
    {
        quitFeedback.PlayFeedbacks();
        yield return new WaitUntil(() => !quitFeedback.IsPlaying);
        Application.Quit(); // 退出游戏
    }
}
