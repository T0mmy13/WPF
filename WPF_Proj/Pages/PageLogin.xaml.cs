using System.Windows;
using System.Windows.Controls;
using WPF_Proj.Classes;

namespace WPF_Proj.Pages
{
    public partial class PageLogin : Page
    {
        public PageLogin()
        {
            InitializeComponent();
        }

        private void Button_Click_Reg(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageRegistration());
        }

        private void Button_Click_Sign(object sender, RoutedEventArgs e)
        {
            // Проверяем валидацию
            if (Validation.GetHasError(idTextBox) || string.IsNullOrEmpty(passBox.Password))
            {
                MessageBox.Show("Заполните все поля правильно", "Ошибка ввода",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            // Проверяем существование врача в БД
            try
            {
                using (var db = new DBContext())
                {
                    int doctorId = int.Parse(idTextBox.Text);
                    var doctor = db.Doctors.Find(doctorId);

                    if (doctor != null && doctor.Password == passBox.Password)
                    {
                        // Передаем врача напрямую в PageMainMenu
                        this.NavigationService.Navigate(new PageMainMenu(doctor));
                    }
                    else
                    {
                        MessageBox.Show("Неверный ID или пароль", "Ошибка входа",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
            }
            catch
            {
                MessageBox.Show("Неверный ID или пароль", "Ошибка входа",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}