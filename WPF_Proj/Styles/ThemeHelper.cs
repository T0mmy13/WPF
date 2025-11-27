using System;
using System.Linq;
using System.Windows;

namespace WPF_Proj
{
    public static class ThemeHelper
    {
        private static readonly string[] _themePaths = {
            "Styles/Colors/DefaultColors.xaml",
            "Styles/Colors/DarkColors.xaml"
        };

        private static bool _isDarkTheme = false;

        public static void ApplyTheme(bool isDark)
        {
            try
            {
                string themePath = isDark ? _themePaths[1] : _themePaths[0];

                var newTheme = new ResourceDictionary
                {
                    Source = new Uri(themePath, UriKind.Relative)
                };

                // Удаляем старые темы цветов
                var oldThemes = Application.Current.Resources.MergedDictionaries
                    .Where(d => d.Source != null && d.Source.OriginalString.Contains("Colors/"))
                    .ToList();

                foreach (var theme in oldThemes)
                {
                    Application.Current.Resources.MergedDictionaries.Remove(theme);
                }

                // Добавляем новую тему
                Application.Current.Resources.MergedDictionaries.Add(newTheme);
                _isDarkTheme = isDark;

                // Сохраняем корректный путь
                Properties.Settings.Default.ThemePath = themePath;
                Properties.Settings.Default.Save();
            }
            catch (Exception ex)
            {
                // Без уведомлений
            }
        }

        public static void ToggleTheme()
        {
            ApplyTheme(!_isDarkTheme);
        }
    }
}