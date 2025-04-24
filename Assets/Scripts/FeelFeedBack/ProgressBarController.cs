using UnityEngine;
using MoreMountains.Feedbacks;
using MoreMountains.Tools;

public class ProgressBarController : MonoBehaviour
{
    public MMProgressBar progressBar;

    // 比如调用减少 10 的方法
    public void DecreaseProgress()
    {
        if (progressBar != null)
        {
            progressBar.Minus10Percent();
        }
    }
}
