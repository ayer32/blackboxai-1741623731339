using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace TaskManagementSystem.ViewModels
{
    public class ValidationResultViewModel
    {
        public bool IsValid { get; set; }
        public Dictionary<string, List<string>> Errors { get; set; }
        public string GeneralError { get; set; }

        public ValidationResultViewModel()
        {
            Errors = new Dictionary<string, List<string>>();
        }

        public static ValidationResultViewModel Success()
        {
            return new ValidationResultViewModel { IsValid = true };
        }

        public static ValidationResultViewModel Fail(string error)
        {
            return new ValidationResultViewModel
            {
                IsValid = false,
                GeneralError = error
            };
        }

        public static ValidationResultViewModel FromModelState(Microsoft.AspNetCore.Mvc.ModelBinding.ModelStateDictionary modelState)
        {
            var result = new ValidationResultViewModel
            {
                IsValid = modelState.IsValid
            };

            foreach (var entry in modelState)
            {
                if (entry.Value.Errors.Any())
                {
                    result.Errors[entry.Key] = entry.Value.Errors
                        .Select(e => e.ErrorMessage)
                        .ToList();
                }
            }

            return result;
        }

        public void AddError(string key, string error)
        {
            if (!Errors.ContainsKey(key))
            {
                Errors[key] = new List<string>();
            }
            Errors[key].Add(error);
            IsValid = false;
        }
    }

    public class FormValidationRules
    {
        public Dictionary<string, FieldValidationRules> Fields { get; set; }
        public List<string> RequiredFields { get; set; }
        public Dictionary<string, string> CustomMessages { get; set; }

        public FormValidationRules()
        {
            Fields = new Dictionary<string, FieldValidationRules>();
            RequiredFields = new List<string>();
            CustomMessages = new Dictionary<string, string>();
        }
    }

    public class FieldValidationRules
    {
        public bool Required { get; set; }
        public string Type { get; set; }
        public int? MinLength { get; set; }
        public int? MaxLength { get; set; }
        public string Pattern { get; set; }
        public string[] AllowedValues { get; set; }
        public Dictionary<string, object> CustomRules { get; set; }
        public Dictionary<string, string> Messages { get; set; }

        public FieldValidationRules()
        {
            CustomRules = new Dictionary<string, object>();
            Messages = new Dictionary<string, string>();
        }
    }

    public class ValidationError
    {
        public string Field { get; set; }
        public string Message { get; set; }
        public string Code { get; set; }
        public Dictionary<string, object> Metadata { get; set; }

        public ValidationError()
        {
            Metadata = new Dictionary<string, object>();
        }
    }

    public class CustomValidationAttribute : ValidationAttribute
    {
        private readonly string _validationType;
        private readonly object[] _parameters;

        public CustomValidationAttribute(string validationType, params object[] parameters)
        {
            _validationType = validationType;
            _parameters = parameters;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            switch (_validationType.ToLower())
            {
                case "future":
                    return ValidateFutureDate(value);
                case "past":
                    return ValidatePastDate(value);
                case "range":
                    return ValidateRange(value);
                case "custom":
                    return ValidateCustom(value, validationContext);
                default:
                    return ValidationResult.Success;
            }
        }

        private ValidationResult ValidateFutureDate(object value)
        {
            if (value is DateTime date)
            {
                return date > DateTime.Now
                    ? ValidationResult.Success
                    : new ValidationResult("Date must be in the future");
            }
            return ValidationResult.Success;
        }

        private ValidationResult ValidatePastDate(object value)
        {
            if (value is DateTime date)
            {
                return date < DateTime.Now
                    ? ValidationResult.Success
                    : new ValidationResult("Date must be in the past");
            }
            return ValidationResult.Success;
        }

        private ValidationResult ValidateRange(object value)
        {
            if (value is int intValue && _parameters.Length >= 2)
            {
                int min = Convert.ToInt32(_parameters[0]);
                int max = Convert.ToInt32(_parameters[1]);
                return intValue >= min && intValue <= max
                    ? ValidationResult.Success
                    : new ValidationResult($"Value must be between {min} and {max}");
            }
            return ValidationResult.Success;
        }

        private ValidationResult ValidateCustom(object value, ValidationContext validationContext)
        {
            // Custom validation logic can be implemented here
            return ValidationResult.Success;
        }
    }

    public class ValidationHelper
    {
        public static ValidationResultViewModel ValidateObject<T>(T model) where T : class
        {
            var validationContext = new ValidationContext(model);
            var validationResults = new List<ValidationResult>();
            var result = new ValidationResultViewModel();

            if (!Validator.TryValidateObject(model, validationContext, validationResults, true))
            {
                foreach (var validationResult in validationResults)
                {
                    foreach (var memberName in validationResult.MemberNames)
                    {
                        result.AddError(memberName, validationResult.ErrorMessage);
                    }
                }
            }
            else
            {
                result.IsValid = true;
            }

            return result;
        }

        public static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        public static bool IsStrongPassword(string password)
        {
            return !string.IsNullOrEmpty(password) &&
                   password.Length >= 8 &&
                   password.Any(char.IsUpper) &&
                   password.Any(char.IsLower) &&
                   password.Any(char.IsDigit) &&
                   password.Any(ch => !char.IsLetterOrDigit(ch));
        }

        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            return !string.IsNullOrEmpty(phoneNumber) &&
                   System.Text.RegularExpressions.Regex.IsMatch(phoneNumber, @"^\+?[\d\s-]+$");
        }

        public static bool IsValidUrl(string url)
        {
            return Uri.TryCreate(url, UriKind.Absolute, out Uri uriResult) &&
                   (uriResult.Scheme == Uri.UriSchemeHttp || uriResult.Scheme == Uri.UriSchemeHttps);
        }
    }
}
