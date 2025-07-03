using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;

public class BossUIManager : MonoBehaviour
{
    public static BossUIManager Instance;

    public MMProgressBar progressBar;

    private RectTransform progressRect;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        Instance = this;

        if (progressBar != null)
        {
            progressRect = progressBar.GetComponent<RectTransform>();

            // 添加 CanvasGroup 用于淡入淡出
            canvasGroup = progressBar.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = progressBar.gameObject.AddComponent<CanvasGroup>();
            }

            // 初始化为隐藏状态（飞到上方 + 透明）
            progressRect.anchoredPosition = new Vector2(0f, 600f);
            canvasGroup.alpha = 0f;
        }
    }

    public void ShowBossUI()
    {
        if (progressBar == null) return;

        progressRect.anchoredPosition = new Vector2(50f, -600f);
        canvasGroup.alpha = 0f;

        Sequence seq = DOTween.Sequence();

        // 1.5秒飞入，Ease.OutBack
        seq.Append(progressRect.DOAnchorPos(new Vector2(125f, -150f), 1.5f).SetEase(Ease.OutBack));

        // 淡入动画和位置动画同时进行，持续1.5秒
        seq.Join(canvasGroup.DOFade(1f, 1.5f));

        seq.Play();
    }

    public void HideBossUI()
    {
        if (progressBar == null) return;

        // 隐藏时飞回到屏幕顶部外，比如 y=600，x保持50
        progressRect.DOAnchorPos(new Vector2(50f, 600f), 0.6f).SetEase(Ease.InBack);
        canvasGroup.DOFade(0f, 0.6f);
    }

}
