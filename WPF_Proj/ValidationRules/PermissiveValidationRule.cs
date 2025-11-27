using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace WPF_Proj.ValidationRules
{
    public class PermissiveValidationRule : ValidationRule
    {
        public string ValidationType { get; set; }
        public string FieldName { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string stringValue = value as string;

            // Разрешаем пустые строки во время ввода
            if (string.IsNullOrEmpty(stringValue))
                return ValidationResult.ValidResult;

            string errorMessage = GetErrorMessage(stringValue);

            if (!string.IsNullOrEmpty(errorMessage))
            {
                // ВАЖНО: возвращаем ValidationResult(false) чтобы показать ошибку
                // Но это НЕ должно блокировать ввод благодаря другим настройкам
                return new ValidationResult(false, errorMessage);
            }

            return ValidationResult.ValidResult;
        }

        private string GetErrorMessage(string input)
        {
            switch (ValidationType)
            {
                case "OnlyLetters":
                    if (!Regex.IsMatch(input, @"^[a-zA-Zа-яА-ЯёЁ\s\-']*$"))
                        return $"{FieldName} - можно вводить только буквы, пробелы, дефисы и апострофы";
                    break;

                case "OnlyDigits":
                    if (!Regex.IsMatch(input, @"^\d*$"))
                        return $"{FieldName} - можно вводить только цифры";
                    break;

                case "Phone":
                    if (!Regex.IsMatch(input, @"^[\d\s\-\+\(\)]*$"))
                        return $"{FieldName} - можно вводить только цифры и символы +-()";
                    break;

                case "Specialization":
                    if (!Regex.IsMatch(input, @"^[a-zA-Zа-яА-ЯёЁ0-9\s\-\.,]*$"))
                        return $"{FieldName} - содержит недопустимые символы";
                    break;

                case "Date":
                    // Для DatePicker - всегда разрешаем
                    break;
            }

            return null;
        }
    }
}