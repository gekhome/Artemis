
namespace Artemis.Dal.Data
{
    public class HeightRangeAttribute : ValidationAttribute
    {
        private readonly double minValue;
        private readonly double maxValue;

        public HeightRangeAttribute(double minValue, double maxValue)
        {
            this.minValue = minValue;
            this.maxValue = maxValue;
        }

        public string GetErrorMessage() 
            => $"The number must be between {minValue} and {maxValue}";

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            var person = (ActorViewModel)validationContext.ObjectInstance;

            var inputValue = value as double?;
            if (inputValue == null)
            {
                return ValidationResult.Success;
            }

            if (inputValue < minValue || inputValue > maxValue)
            {
                return new ValidationResult(GetErrorMessage());
            }

            return ValidationResult.Success;
        }
    }
}
