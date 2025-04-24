using UnityEngine;
using MoreMountains.Feedbacks;

public class ClickToPlayFeedback : MonoBehaviour
{
    [Header("Feedback to Play on Click")]
    public MMFeedbacks feedbacks;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // 鼠标左键按下
        {
            if (feedbacks != null)
            {
                feedbacks.PlayFeedbacks();
            }
        }
    }
}
