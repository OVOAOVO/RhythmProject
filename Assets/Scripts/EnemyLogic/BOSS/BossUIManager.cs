using UnityEngine;
using DG.Tweening;
using MoreMountains.Tools;
using System;
public class BossUIManager : MonoBehaviour
{
    public static BossUIManager Instance;

    public MMProgressBar progressBar;

    private RectTransform progressRect;
    private CanvasGroup canvasGroup;
    private TweenOwner tweenOwner;
    private bool isHidden = false;
    public event Action OnUIHiddenDueToZero;
    private void Awake()
    {
        Instance = this;

        if (progressBar != null)
        {
            progressRect = progressBar.GetComponent<RectTransform>();

            canvasGroup = progressBar.GetComponent<CanvasGroup>();
            if (canvasGroup == null)
            {
                canvasGroup = progressBar.gameObject.AddComponent<CanvasGroup>();
            }

            progressRect.anchoredPosition = new Vector2(0f, 600f);
            canvasGroup.alpha = 0f;
        }

        // ✅ 初始化 TweenOwner
        tweenOwner = GetComponent<TweenOwner>();
        if (tweenOwner == null)
        {
            tweenOwner = gameObject.AddComponent<TweenOwner>();
        }
    }

    void Update()
    {
        if (progressBar.BarTarget == 0f && !isHidden)
        {
            isHidden = true;
            HideBossUI(0f);
            OnUIHiddenDueToZero?.Invoke(); // ✅ 广播事件，而不是控制逻辑
        }
    }

    public void ShowBossUI()
    {
        if (progressBar == null) return;

        progressRect.anchoredPosition = new Vector2(50f, -600f);
        canvasGroup.alpha = 0f;

        Sequence seq = DOTween.Sequence();
        seq.Append(progressRect.DOAnchorPos(new Vector2(0, -240f), 1.5f).SetEase(Ease.OutBack));
        seq.Join(canvasGroup.DOFade(1f, 1.5f));

        tweenOwner.RegisterTween(seq);
    }

    public void HideBossUI(float delay)
    {
        if (progressBar == null) return;

        Sequence seq = DOTween.Sequence();
        seq.AppendInterval(delay);
        seq.Append(progressRect.DOAnchorPos(new Vector2(50f, 600f), 0.6f).SetEase(Ease.InBack));
        seq.Join(canvasGroup.DOFade(0f, 0.6f));

        tweenOwner.RegisterTween(seq);
    }
}
