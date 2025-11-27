using System.Windows;
using System.Windows.Controls;
using WPF_Proj.Classes;
using System.Text.RegularExpressions;

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
            // Устанавливаем пароль из PasswordBox
            doc.Password = PasswordBox.Password;

            // Проверяем валидацию
            if (!IsFormValid())
            {
                return;
            }

            try
            {
                using (var db = new DBContext())
                {
                    db.Doctors.Add(doc);
                    db.SaveChanges();
                    MessageBox.Show($"Врач {doc.Name} {doc.LastName} зарегистрирован.",
                                  "Успех", MessageBoxButton.OK, MessageBoxImage.Information);

                    // Сбрасываем форму
                    doc = new Doctor();
                    DataContext = doc;
                    PasswordBox.Password = string.Empty;
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
            // Проверяем только обязательные поля при сохранении
            if (string.IsNullOrWhiteSpace(doc.Name))
            {
                MessageBox.Show("Введите имя врача", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(doc.LastName))
            {
                MessageBox.Show("Введите фамилию врача", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(doc.Specialization))
            {
                MessageBox.Show("Введите специализацию", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (string.IsNullOrWhiteSpace(PasswordBox.Password))
            {
                MessageBox.Show("Введите пароль", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            // Дополнительные проверки формата
            if (!Regex.IsMatch(doc.Name, @"^[a-zA-Zа-яА-ЯёЁ\s\-']+$"))
            {
                MessageBox.Show("Имя должно содержать только буквы", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!Regex.IsMatch(doc.LastName, @"^[a-zA-Zа-яА-ЯёЁ\s\-']+$"))
            {
                MessageBox.Show("Фамилия должна содержать только буквы", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (!Regex.IsMatch(doc.Specialization, @"^[a-zA-Zа-яА-ЯёЁ0-9\s\-\.,]+$"))
            {
                MessageBox.Show("Специализация содержит недопустимые символы", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            if (PasswordBox.Password.Length < 6)
            {
                MessageBox.Show("Пароль должен содержать минимум 6 символов", "Ошибка валидации",
                              MessageBoxButton.OK, MessageBoxImage.Error);
                return false;
            }

            return true;
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}