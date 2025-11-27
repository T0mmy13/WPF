using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using WPF_Proj.Classes;
using Microsoft.EntityFrameworkCore;

namespace WPF_Proj.Pages
{
    public partial class PageMainMenu : Page
    {
        private List<Patient> _patients = new List<Patient>();
        private Doctor _currentDoctor;

        public PageMainMenu(Doctor doctor)
        {
            InitializeComponent();
            _currentDoctor = doctor;

            try
            {
                DatabaseInitializer.Initialize();
                LoadDoctorInfo();
                LoadPatients();
                DataContext = this;
                UpdateButtonsState();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка инициализации приложения: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        public int PatientsCount => _patients.Count;

        private void LoadDoctorInfo()
        {
            if (_currentDoctor != null)
            {
                DoctorNameLabel.Content = $"{_currentDoctor.LastName} {_currentDoctor.Name} {_currentDoctor.MiddleName}";
                SpecializationLabel.Content = $"Специализация: {_currentDoctor.Specialization}";
            }
        }

        private void LoadPatients()
        {
            try
            {
                using (var db = new DBContext())
                {
                    _patients = db.Patients?.ToList() ?? new List<Patient>();

                    // Проверяем, есть ли пациенты
                    if (_patients.Any())
                    {
                        var patientIds = _patients.Select(p => p.Id).ToList();
                        var lastAppointments = db.AppointmentStories
                            .Where(a => patientIds.Contains(a.Patient_id))
                            .GroupBy(a => a.Patient_id)
                            .Select(g => new
                            {
                                PatientId = g.Key,
                                LastDate = g.Max(a => a.Date)
                            })
                            .ToList();

                        foreach (var patient in _patients)
                        {
                            var lastAppointment = lastAppointments
                                .FirstOrDefault(la => la.PatientId == patient.Id);
                            patient.LastAppointmentDate = lastAppointment?.LastDate;
                        }
                    }

                    PatientsListBox.ItemsSource = _patients;
                }
            }
            catch (DbUpdateException dbEx)
            {
                _patients = new List<Patient>();
                PatientsListBox.ItemsSource = _patients;
                string errorDetails = GetDbErrorDetails(dbEx);
                MessageBox.Show($"Ошибка базы данных при загрузке пациентов:\n{errorDetails}",
                              "Ошибка базы данных", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            catch (Exception ex)
            {
                _patients = new List<Patient>();
                PatientsListBox.ItemsSource = _patients;
                MessageBox.Show($"Ошибка загрузки пациентов: {ex.Message}", "Ошибка",
                              MessageBoxButton.OK, MessageBoxImage.Warning);
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

        private void UpdateButtonsState()
        {
            bool isPatientSelected = PatientsListBox.SelectedItem != null;
            StartAppointmentButton.IsEnabled = isPatientSelected;
            ChangeInfoButton.IsEnabled = isPatientSelected;
        }

        private void PatientsListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateButtonsState();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.NavigationService.Navigate(new PageAddpatient());
        }

        private void Button_Click_StartApointment(object sender, RoutedEventArgs e)
        {
            var selectedPatient = PatientsListBox.SelectedItem as Patient;
            if (selectedPatient != null)
            {
                this.NavigationService.Navigate(new PageApointmens(selectedPatient, _currentDoctor));
            }
        }

        private void Button_Click_ChangeInfo(object sender, RoutedEventArgs e)
        {
            var selectedPatient = PatientsListBox.SelectedItem as Patient;
            if (selectedPatient != null)
            {
                this.NavigationService.Navigate(new PageChangeInfo(selectedPatient));
            }
        }
    }
}