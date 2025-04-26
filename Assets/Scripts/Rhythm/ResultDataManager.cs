using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
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

            // 订阅场景加载完成事件
            SceneManager.sceneLoaded += OnSceneLoaded;

            // 初始化 UI 绑定
            BindComboText();
            // 第一次 Awake 时，也先重置一次
            ResetAll();
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        // 取消订阅，防止内存泄漏
        if (Instance == this)
            SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // 只在进入 “Game” 场景时重置
        if (scene.name == "Game")
        {
            ResetAll();
        }
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
