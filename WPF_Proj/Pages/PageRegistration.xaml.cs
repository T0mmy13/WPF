using System;
using System.Collections.Generic;
using System.Configuration;
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
using WPF_Proj.Classes;

namespace WPF_Proj.Pages
{
    public partial class PageRegistration : Page
    {
        
        Doctor doc = new Doctor();
        public PageRegistration()
        {
            InitializeComponent();
            DataContext = doc;
        }

        private void Button_Click_Reg(object sender, RoutedEventArgs e)
        {
            using (var db = new DBContext())
            {
                db.Doctors.Add(doc);
                db.SaveChanges();
                MessageBox.Show($"Врач {doc.Name} зарегистрирован.");
            }
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}
