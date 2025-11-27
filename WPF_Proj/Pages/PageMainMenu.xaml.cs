using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WPF_Proj.Pages
{
    /// <summary>
    /// Логика взаимодействия для Page1.xaml
    /// </summary>
    public partial class PageMainMenu : Page
    {
        public PageMainMenu()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageAddpatient());
        }

        private void Button_Click_StartApointment(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageApointmens());
        }

        private void Button_Click_ChangeInfo(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageChangeInfo());
        }
    }
}
