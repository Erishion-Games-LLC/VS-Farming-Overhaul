using FarmingOverhaul.src.Helpers.Validation;

namespace FarmingOverhaulTests.tests.UtilitiesForTesting
{
    public static class TestHelpers
    {
        public static void AssertHasValidationError(List<ValidationResult> results, ValidationErrorType error, string field)
        {
            Assert.Contains(results, result => result.ValidatedItem == field && result.Error == error);
        }

        public static void AssertListHasExactAmount<T>(List<T> list, int expected)
        {
            Assert.Equal(expected, list.Count);
        }
    }
}