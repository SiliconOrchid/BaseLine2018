using System.ComponentModel.DataAnnotations;

namespace BaseLine2018.Common.AttributeValidators
{

    /// <summary>
    /// This attribute is a demonstration of using custom validation attributes.  
    /// </summary>
    public class GreaterThanAttribute : ValidationAttribute
    {
        private const string DefaultUnableToValidateNumberMessage = "Unable to validate provided value is a number";
        private const string DefaultGreaterThanMessage = "Must be more than than 0.";

        public int GreaterThanValue { get; set; }
        public string GreaterThanMessage { get; set; }

        protected override ValidationResult IsValid(object value, ValidationContext context)
        {
            if (value == null)
                return ValidationResult.Success;

            if ((int)value <= this.GreaterThanValue)
            {
                return new ValidationResult(this.GreaterThanMessage ?? DefaultGreaterThanMessage);
            }
            return ValidationResult.Success;
        }
    }
}
