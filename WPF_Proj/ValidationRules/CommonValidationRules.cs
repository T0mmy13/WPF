using System;
using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace WPF_Proj.ValidationRules
{
    public class CommonValidationRules : ValidationRule
    {
        private const int MAX_AGE_YEARS = 150;

        public string ValidationType { get; set; }
        public string FieldName { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (string.IsNullOrEmpty(ValidationType))
                return ValidationResult.ValidResult;

            string stringValue = value as string;

            if (string.IsNullOrEmpty(stringValue))
                return ValidationResult.ValidResult;

            switch (ValidationType)
            {
                case "OnlyLetters":
                    return ValidateOnlyLetters(stringValue);
                case "OnlyDigits":
                    return ValidateOnlyDigits(stringValue);
                case "Phone":
                    return ValidatePhone(stringValue);
                case "Date":
                    return ValidateDate(value);
                case "Specialization":
                    return ValidateSpecialization(stringValue);
                default:
                    return ValidationResult.ValidResult;
            }
        }

        private ValidationResult ValidateOnlyLetters(string input)
        {
            if (!Regex.IsMatch(input, @"^[a-zA-Zа-яА-ЯёЁ\s\-']*$"))
                return new ValidationResult(false, $"{FieldName} - можно вводить только буквы");
            return ValidationResult.ValidResult;
        }

        private ValidationResult ValidateOnlyDigits(string input)
        {
            if (!Regex.IsMatch(input, @"^\d*$"))
                return new ValidationResult(false, $"{FieldName} - можно вводить только цифры");
            return ValidationResult.ValidResult;
        }

        private ValidationResult ValidatePhone(string input)
        {
            if (!Regex.IsMatch(input, @"^[\d\s\-\+\(\)]*$"))
                return new ValidationResult(false, $"{FieldName} - можно вводить только цифры и символы +-()");
            return ValidationResult.ValidResult;
        }

        private ValidationResult ValidateDate(object value)
        {
            if (value is DateTime date)
            {
                if (date > DateTime.Now)
                    return new ValidationResult(false, $"{FieldName} - не может быть в будущем");

                if (date < DateTime.Now.AddYears(-MAX_AGE_YEARS))
                    return new ValidationResult(false, $"{FieldName} - некорректная дата");
            }
            return ValidationResult.ValidResult;
        }

        private ValidationResult ValidateSpecialization(string input)
        {
            if (!Regex.IsMatch(input, @"^[a-zA-Zа-яА-ЯёЁ0-9\s\-\.,]*$"))
                return new ValidationResult(false, $"{FieldName} - содержит недопустимые символы");
            return ValidationResult.ValidResult;
        }
    }
}