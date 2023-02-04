using System.ComponentModel.DataAnnotations;

namespace AuthenticationAPIApp.Validation
{
    public class NotEmptyAttribute : ValidationAttribute
    {
        public NotEmptyAttribute() : base("{0} is required.") { }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value is string stringValue && string.IsNullOrEmpty(stringValue))
            {
                var errorMessage = FormatErrorMessage(validationContext.DisplayName);
                return new ValidationResult(errorMessage);
            }
            return ValidationResult.Success;
        }
    }


}
