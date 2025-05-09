
using UnityEngine.Localization.Settings;
public static class LanguageToggle
{
    public static void ToggleLanguage()
    {
        var current = LocalizationSettings.SelectedLocale;
        var locales = LocalizationSettings.AvailableLocales.Locales;
        var nextLocale = current.Identifier.Code == "en"
            ? locales.Find(l => l.Identifier.Code == "zh-Hans")
            : locales.Find(l => l.Identifier.Code == "en");

        LocalizationSettings.SelectedLocale = nextLocale;
    }
}
