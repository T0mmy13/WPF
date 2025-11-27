using System.Windows;

namespace WPF_Proj
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            // НЕ применяем сохраненную тему при старте
            // ThemeHelper.ApplySavedTheme(); // ЗАКОММЕНТИРОВАТЬ эту строку
        }
    }
}