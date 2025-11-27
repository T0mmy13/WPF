using System.Windows;
using System.Windows.Controls;
using WPF_Proj.Classes;
using System.Text.RegularExpressions;

namespace WPF_Proj.Pages
{
    public partial class PageChangeInfo : Page
    {
        private Patient _patient;

        public PageChangeInfo(Patient patient)
        {
            InitializeComponent();
            _patient = patient;
            DataContext = _patient;
        }

        private void Button_SaveChanges(object sender, RoutedEventArgs e)
        {
            if (!IsFormValid())
            {
                return;
            }

            try
            {
                using (var db = new DBContext())
                {
                    // Очищаем телефон перед сохранением
                    _patient.PhoneNumber = Regex.Replace(_patient.PhoneNumber, @"[^\d]", "");

                    db.Patients.Update(_patient);
                    db.SaveChanges();

                    MessageBox.Show($"Данные пациента {_patient.LastName} {_patient.Name} сохранены.",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    this.NavigationService.GoBack();
                }
            }
            catch (System.Exception ex)
            {
                MessageBox.Show($"Ошибка при сохранении: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private bool IsFormValid()
        {
            if (string.IsNullOrWhiteSpace(_patient.Name))
            {
                MessageBox.Show("Введите имя пациента", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(_patient.LastName))
            {
                MessageBox.Show("Введите фамилию пациента", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (_patient.Birthday == default)
            {
                MessageBox.Show("Введите дату рождения", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(_patient.PhoneNumber))
            {
                MessageBox.Show("Введите номер телефона", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Проверка телефона (только цифры)
            string cleanPhone = Regex.Replace(_patient.PhoneNumber, @"[^\d]", "");
            if (cleanPhone.Length < 10)
            {
                MessageBox.Show("Номер телефона должен содержать не менее 10 цифр", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}