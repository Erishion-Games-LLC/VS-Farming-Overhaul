namespace FarmingOverhaul.src.Helpers.Validation
{
    public class ValidationResult(ValidationErrorType error, string item)
    {
        public ValidationErrorType Error { get; } = error;
        public string ValidatedItem { get; } = item;

        public string CreateMessage()
        {
            return Error switch
            {
                ValidationErrorType.NullClass => $"{ValidatedItem} class is null",
                ValidationErrorType.MinGreaterThanMax => $"{ValidatedItem} min value cannot be greater than max value. Must be less than or equal to",
                _ => "Unknown error",
            };
        }
    }
}