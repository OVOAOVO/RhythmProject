using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using MoreMountains.Feedbacks;
public class MainMenuController : MonoBehaviour
{
    public MMF_Player game;
    public MMF_Player songA;
    public MMF_Player songB;
    public MMF_Player songC;
    public MMF_Player exitButtonFeedback;
    private bool isTransitioning = false;  
    private string targetScene = "";
    private void OnEnable()
    {
        isTransitioning = false;
        var root = GetComponent<UIDocument>().rootVisualElement;

        var gameButton = root.Q<Button>("GameTest");
        var songAButton = root.Q<Button>("songA");
        var songBButton = root.Q<Button>("songB");    
        var songCButton = root.Q<Button>("songC");
        var exitButton = root.Q<Button>("quitButton");
        var PSLabel = root.Q<Label>("PSLabel");

        gameButton.clicked += () => OnButtonClicked(game, "Game");
        songAButton.clicked += () => OnButtonClicked(songA, "SONG_A");
        songBButton.clicked += () => OnButtonClicked(songB, "SONG_B");
        songCButton.clicked += () => OnButtonClicked(songC, "SONG_C");
        exitButton.clicked += () =>
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                StartCoroutine(WaitUntilExitFeedbacksEnd());
            }
        };

        UILocalizationHelper.BindLocalizedText(gameButton, "LocalizationTables", "GameTest");
        UILocalizationHelper.BindLocalizedText(songAButton, "LocalizationTables", "songA");
        UILocalizationHelper.BindLocalizedText(songBButton, "LocalizationTables", "songB");
        UILocalizationHelper.BindLocalizedText(songCButton, "LocalizationTables", "songC");
        UILocalizationHelper.BindLocalizedText(exitButton, "LocalizationTables", "quitButton");
        UILocalizationHelper.BindLocalizedText(PSLabel, "LocalizationTables", "PSLabel");
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
        exitButtonFeedback.PlayFeedbacks();
        yield return new WaitUntil(() => !exitButtonFeedback.IsPlaying);
        Application.Quit(); // 退出游戏
    }
}

    
