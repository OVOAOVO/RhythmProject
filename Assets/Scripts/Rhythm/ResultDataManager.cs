using UnityEngine;
using UnityEngine.UI;

public class ResultDataManager : MonoBehaviour
{
    // —— 数据统计部分 —— //
    public int MaxCombo { get; private set; } = 0;
    public int PerfectCount { get; private set; } = 0;
    public int GoodCount { get; private set; } = 0;
    public int BadCount { get; private set; } = 0;

    private int currentCombo = 0;

    private Text comboText;
    private string ComboPath = "Canvas/ComboText/Combo";  // 预设路径      
    // —— 单例 —— //
    public static ResultDataManager Instance { get; private set; }

    private void Awake()
    {
        // 单例初始化
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }
        // 初始绑定一次
        BindComboText();
        // 初始化 UI
        UpdateComboUI(0);
    }

    // —— 对外接口 —— //

    /// <summary>记录一次 Perfect，同时加连击</summary>
    public void AddPerfect()
    {
        PerfectCount++;
        AddCombo();
    }

    /// <summary>记录一次 Good，同时加连击</summary>
    public void AddGood()
    {
        GoodCount++;
        AddCombo();
    }

    /// <summary>记录一次 Bad，同时加连击</summary>
    public void AddBad()
    {
        BadCount++;
        AddCombo();
    }

    /// <summary>重置所有数据（可在关卡开始/重试时调用）</summary>
    public void ResetAll()
    {
        PerfectCount = 0;
        GoodCount = 0;
        BadCount = 0;
        MaxCombo = 0;
        currentCombo = 0;
        UpdateComboUI(0);
    }

    /// <summary>仅重置当前连击数，不影响历史最大连击或其他统计</summary>
    public void ResetCombo()
    {
        currentCombo = 0;
        UpdateComboUI(0);
    }

    // —— 内部方法 —— //

    /// <summary>累加连击计数，并刷新最大连击与 UI</summary>
    private void AddCombo()
    {
        currentCombo++;
        if (currentCombo > MaxCombo)
            MaxCombo = currentCombo;

        UpdateComboUI(currentCombo);
    }

    /// <summary>更新 UI 文本显示</summary>
    private void UpdateComboUI(int combo)
    {
        if (comboText == null)//切换场景重新绑定
            BindComboText();

        if (comboText != null)
            comboText.text = combo.ToString();
    }

    private void BindComboText()
    {
        if (comboText == null)
        {
            GameObject obj = GameObject.Find(ComboPath);
            if (obj != null)
                comboText = obj.GetComponent<Text>();
        }
    }
}
