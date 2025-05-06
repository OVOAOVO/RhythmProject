using UnityEngine;
using UnityEngine.UIElements;

public class PauseMenu : MonoBehaviour
{
    private bool isPaused = false;
    private VisualElement pauseMenu;

    void OnEnable()
    {
        var uiDocument = GetComponent<UIDocument>();
        var root = uiDocument.rootVisualElement;

        // 等待 UI 构建完成后设置隐藏
        root.RegisterCallback<GeometryChangedEvent>(_ =>
        {
            pauseMenu = root.Q<VisualElement>("results-panel");
            if (pauseMenu != null)
            {
                pauseMenu.style.display = DisplayStyle.None;
            }
        });
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0f;
            pauseMenu.style.display = DisplayStyle.Flex;
        }
        else
        {
            Time.timeScale = 1f;
            pauseMenu.style.display = DisplayStyle.None;
        }
    }

    void OnDisable()
    {
        Time.timeScale = 1f;
    }
}
