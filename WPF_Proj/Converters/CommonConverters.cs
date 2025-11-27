using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Data;

namespace WPF_Proj.Converters
{
    public class CommonConverters : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter == null) return value;

            string converterType = parameter.ToString();

            switch (converterType)
            {
                case "Phone":
                    return ConvertPhone(value);

                case "Age":
                    return ConvertAge(value);

                case "IsAdult":
                    return ConvertIsAdult(value);

                case "DaysSinceLastAppointment":
                    return ConvertDaysSinceLastAppointment(value);

                default:
                    return value;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter?.ToString() == "Phone")
            {
                return ConvertBackPhone(value);
            }

            return value;
        }

        private object ConvertPhone(object value)
        {
            if (value == null || string.IsNullOrEmpty(value.ToString()))
                return string.Empty;

            string phone = value.ToString();

            // Если телефон уже отформатирован, возвращаем как есть
            if (phone.Contains("(") || phone.Contains(")") || phone.Contains("-"))
                return phone;

            string cleanPhone = Regex.Replace(phone, @"[^\d]", "");

            // Форматируем только если есть достаточное количество цифр
            if (cleanPhone.Length >= 10)
            {
                if (cleanPhone.Length == 10) // 9001234567
                {
                    return $"+7 ({cleanPhone.Substring(0, 3)}) {cleanPhone.Substring(3, 3)}-{cleanPhone.Substring(6, 2)}-{cleanPhone.Substring(8, 2)}";
                }
                else if (cleanPhone.Length == 11) // 79001234567 или 89001234567
                {
                    string digits = cleanPhone.Substring(1); // убираем первую цифру (7 или 8)
                    return $"+7 ({digits.Substring(0, 3)}) {digits.Substring(3, 3)}-{digits.Substring(6, 2)}-{digits.Substring(8, 2)}";
                }
            }

            // Если цифр мало, возвращаем как есть (пользователь еще вводит)
            return phone;
        }

        private object ConvertBackPhone(object value)
        {
            if (value == null) return string.Empty;

            string phone = value.ToString();

            // Очищаем номер для хранения в БД - оставляем только цифры
            string cleanPhone = Regex.Replace(phone, @"[^\d]", "");

            // Если номер начинается с 7 или 8 и имеет 11 цифр - оставляем как есть
            // Если 10 цифр - добавляем 7 в начало
            if (cleanPhone.Length == 10 && long.TryParse(cleanPhone, out _))
            {
                return "7" + cleanPhone;
            }

            return cleanPhone;
        }

        private object ConvertAge(object value)
        {
            if (value is DateTime birthday)
            {
                var today = DateTime.Today;
                var age = today.Year - birthday.Year;
                if (birthday.Date > today.AddYears(-age)) age--;
                return age;
            }
            return 0;
        }

        private object ConvertIsAdult(object value)
        {
            if (value is DateTime birthday)
            {
                var today = DateTime.Today;
                var age = today.Year - birthday.Year;
                if (birthday.Date > today.AddYears(-age)) age--;
                return age >= 18 ? "Совершеннолетний" : "Несовершеннолетний";
            }
            return "Несовершеннолетний";
        }

        private object ConvertDaysSinceLastAppointment(object value)
        {
            if (value is DateTime lastAppointmentDate)
            {
                var days = (DateTime.Today - lastAppointmentDate).Days;
                return days == 0 ? "Сегодня" :
                       days == 1 ? "1 день назад" :
                       days < 7 ? $"{days} дней назад" :
                       $"{days} дней назад";
            }
            return "Первый прием в клинике";
        }
    }
}