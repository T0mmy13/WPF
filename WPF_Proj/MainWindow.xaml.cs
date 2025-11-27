using System.Windows;
using WPF_Proj.Pages;

namespace WPF_Proj
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            MainFrame.Navigate(new PageLogin());
        }

        private void ChangeTheme_Click(object sender, RoutedEventArgs e)
        {
            ThemeHelper.ToggleTheme();
        }
    }
}