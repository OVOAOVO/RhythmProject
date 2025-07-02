using UnityEngine;
using DG.Tweening;

public class BossUIManager : MonoBehaviour
{
    public static BossUIManager Instance;

    public RectTransform bossHPBar;
    public CanvasGroup canvasGroup;

    private void Awake()
    {
        Instance = this;
        canvasGroup.alpha = 0f;
        bossHPBar.anchoredPosition = new Vector2(0, 300f); // 屏幕外
    }

    public void ShowBossUI()
    {
        bossHPBar.DOAnchorPos(new Vector2(0, 0), 1f).SetEase(Ease.OutBack);
        canvasGroup.DOFade(1f, 1f);
    }

    public void HideBossUI()
    {
        bossHPBar.DOAnchorPos(new Vector2(0, 300f), 1f).SetEase(Ease.InBack);
        canvasGroup.DOFade(0f, 1f);
    }
}
