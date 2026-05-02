using FarmingOverhaul.src.Helpers.Validation;

namespace FarmingOverhaulTests.tests.HelpersTests.Validation
{
    public class ValidationResultTests
    {
        [Theory]
        [InlineData(ValidationErrorType.NullClass, "Item", "Item class is null")]
        [InlineData(ValidationErrorType.MinGreaterThanMax, "Field", "Field min value cannot be greater than max value. Must be less than or equal to")]
        [InlineData((ValidationErrorType)999, "Test", "Unknown error")]
        public void CreateMessage_ReturnsExpected(ValidationErrorType error, string item, string expected)
        {
            ValidationResult result = new(error, item);

            Assert.Equal(expected, result.CreateMessage());
        }
    }
}