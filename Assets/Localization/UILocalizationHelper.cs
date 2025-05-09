
using UnityEngine.UIElements;

using UnityEngine.Localization;

public static class UILocalizationHelper
{
    public static void BindLocalizedText(VisualElement element, string table, string key)
    {
        var localizedString = new LocalizedString(table, key);
        localizedString.StringChanged += value =>
        {
            if (element is Label label)
                label.text = value;
            else if (element is Button button)
                button.text = value;
        };
        localizedString.RefreshString(); // 立刻触发一次
    }

    public static void SetLocalizedFormattedTextValue(Label label, string table, string key, params object[] args)
    {
        var locStr = new LocalizedString(table, key);
        locStr.Arguments = args;

        locStr.StringChanged += value =>
        {
            label.text = value;
        };
    }
}
