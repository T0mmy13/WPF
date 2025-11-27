using Microsoft.EntityFrameworkCore;
using System;

namespace WPF_Proj.Classes
{
    public static class DatabaseInitializer
    {
        public static void Initialize()
        {
            try
            {
                using (var db = new DBContext())
                {
                    // Проверяем соединение с БД
                    if (db.Database.CanConnect())
                    {
                        // Создаем базу и таблицы, если их нет
                        db.Database.EnsureCreated();

                        // Проверяем существование таблиц
                        var doctorTableExists = db.Doctors != null;
                        var patientTableExists = db.Patients != null;
                        var appointmentTableExists = db.AppointmentStories != null;

                        Console.WriteLine("База данных успешно инициализирована");
                    }
                    else
                    {
                        throw new Exception("Не удается подключиться к базе данных");
                    }
                }
            }
            catch (Exception ex)
            {
                // Логируем ошибку
                System.Diagnostics.Debug.WriteLine($"Ошибка инициализации БД: {ex.Message}");
                throw; // Пробрасываем исключение дальше
            }
        }
    }
}