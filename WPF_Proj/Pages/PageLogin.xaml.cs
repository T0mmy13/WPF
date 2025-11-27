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
    public partial class PageLogin : Page
    {
        string Id;
        
        public PageLogin()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void Button_Click_Reg(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageRegistration());
        }

        private void Button_Click_Sign(object sender, RoutedEventArgs e)
        {
            if(!string.IsNullOrEmpty(Id) || !string.IsNullOrEmpty(passBox.Password))
            {

            }
            else
            {

            }

            this.NavigationService.Navigate(new PageMainMenu());
        }
    }
}