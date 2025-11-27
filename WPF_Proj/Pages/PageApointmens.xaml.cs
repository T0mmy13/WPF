using System;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_Proj.Classes;
using Microsoft.EntityFrameworkCore;

namespace WPF_Proj.Pages
{
    public partial class PageApointmens : Page
    {
        private Patient _currentPatient;
        private Doctor _currentDoctor;

        public PageApointmens(Patient patient, Doctor doctor)
        {
            InitializeComponent();
            _currentPatient = patient;
            _currentDoctor = doctor;
            LoadPatientInfo();
            LoadAppointmentHistory();
        }

        private void LoadPatientInfo()
        {
            if (_currentPatient != null)
            {
                PatientNameLabel.Content = $"{_currentPatient.LastName} {_currentPatient.Name} {_currentPatient.MiddleName}";
                BirthdayLabel.Content = $"Дата рождения: {_currentPatient.Birthday:dd.MM.yyyy}";
                AgeLabel.Content = $"Возраст: {_currentPatient.Age} лет";
                AdultStatusLabel.Content = _currentPatient.IsAdult ? "Совершеннолетний" : "Несовершеннолетний";
                PhoneLabel.Content = $"Телефон: {_currentPatient.PhoneNumber}";
            }
        }

        private void LoadAppointmentHistory()
        {
            try
            {
                using (var db = new DBContext())
                {
                    var appointments = db.AppointmentStories
                        .Where(a => a.Patient_id == _currentPatient.Id)
                        .OrderByDescending(a => a.Date)
                        .ToList();

                    AppointmentsListBox.ItemsSource = appointments;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка при загрузке истории приемов: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void Button_SaveAppointment_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                string diagnosis = DiagnosisTextBox.Text.Trim();
                string recommendations = RecommendationsTextBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(diagnosis))
                {
                    MessageBox.Show("Введите диагноз", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка длины диагноза
                if (diagnosis.Length > 500)
                {
                    MessageBox.Show("Диагноз слишком длинный (максимум 500 символов)", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                // Проверка длины рекомендаций
                if (recommendations.Length > 1000)
                {
                    MessageBox.Show("Рекомендации слишком длинные (максимум 1000 символов)", "Ошибка",
                                  MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }

                using (var db = new DBContext())
                {
                    // Проверяем существование пациента и врача
                    var patientExists = db.Patients.Any(p => p.Id == _currentPatient.Id);
                    var doctorExists = db.Doctors.Any(d => d.Id == _currentDoctor.Id);

                    if (!patientExists)
                    {
                        MessageBox.Show($"Пациент с ID {_currentPatient.Id} не найден", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    if (!doctorExists)
                    {
                        MessageBox.Show($"Врач с ID {_currentDoctor.Id} не найден", "Ошибка",
                                      MessageBoxButton.OK, MessageBoxImage.Error);
                        return;
                    }

                    var appointment = new AppointmentStories
                    {
                        Patient_id = _currentPatient.Id,
                        Doctor_id = _currentDoctor.Id,
                        Date = DateTime.Now,
                        Diagnosis = diagnosis,
                        Recomendations = string.IsNullOrWhiteSpace(recommendations) ? null : recommendations
                    };

                    db.AppointmentStories.Add(appointment);
                    db.SaveChanges();

                    MessageBox.Show("Прием успешно сохранен!", "Успех",
                                  MessageBoxButton.OK, MessageBoxImage.Information);

                    LoadAppointmentHistory();
                    DiagnosisTextBox.Text = "";
                    RecommendationsTextBox.Text = "";
                }
            }
            catch (DbUpdateException dbEx)
            {
                string errorDetails = GetDbErrorDetails(dbEx);
                MessageBox.Show($"Ошибка сохранения в базу данных:\n{errorDetails}", "Ошибка БД",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Неожиданная ошибка:\n{ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private string GetDbErrorDetails(DbUpdateException dbEx)
        {
            string details = $"Сообщение: {dbEx.Message}\n";

            if (dbEx.InnerException != null)
            {
                details += $"Внутренняя ошибка: {dbEx.InnerException.Message}\n";

                if (dbEx.InnerException.InnerException != null)
                {
                    details += $"Детали: {dbEx.InnerException.InnerException.Message}";
                }
            }

            return details;
        }

        private void Button_Click_Back(object sender, RoutedEventArgs e)
        {
            this.NavigationService.GoBack();
        }
    }
}