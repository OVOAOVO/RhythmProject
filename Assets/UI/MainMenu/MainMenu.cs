using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.SceneManagement;
using System.Collections;
using MoreMountains.Feedbacks;
public class MainMenuController : MonoBehaviour
{
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

        var songAButton = root.Q<Button>("songA");
        var songBButton = root.Q<Button>("songB");    
        var songCButton = root.Q<Button>("songC");
        var exitButton = root.Q<Button>("quitButton");
        songAButton.clicked += () => OnButtonClicked(songA, "Game");
        songBButton.clicked += () => OnButtonClicked(songB, "Game");
        songCButton.clicked += () => OnButtonClicked(songC, "Game");
        exitButton.clicked += () =>
        {
            if (!isTransitioning)
            {
                isTransitioning = true;
                StartCoroutine(WaitUntilExitFeedbacksEnd());
            }
        };
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

    
