using FarmingOverhaul.src.Constants.AnimalsConstants;
using FarmingOverhaul.src.Helpers.Validation;
using static FarmingOverhaulTests.tests.UtilitiesForTesting.TestHelpers;

namespace FarmingOverhaulTests.tests.ConstantsTests.AnimalsConstantsTests
{
    public class BreedingConstantsTest
    {
        [Fact]
        public void ValidateVariableRange_ReturnsError_WhenVariablesAreInvalid()
        {
            BreedingConstants breeding = new()
            {
                MinDaysBeforeBreedAgainFemale = 5,
                MaxDaysBeforeBreedAgainFemale = 1,
                MinDaysBeforeBreedAgainMale = 5,
                MaxDaysBeforeBreedAgainMale = 1
            };

            List<ValidationResult> results = breeding.ValidateVariableRange();

            AssertListHasExactAmount(results, 2);
            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, nameof(BreedingConstants.MinDaysBeforeBreedAgainFemale));
            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, nameof(BreedingConstants.MinDaysBeforeBreedAgainMale));
        }

        [Fact]
        public void ValidateVariableRange_ReturnsEmpty_WhenVariablesAreValid()
        {
            BreedingConstants breeding = new()
            {
                MinDaysBeforeBreedAgainFemale = 1,
                MaxDaysBeforeBreedAgainFemale = 2,
                MinDaysBeforeBreedAgainMale = 1,
                MaxDaysBeforeBreedAgainMale = 2
            };

            List<ValidationResult> result = breeding.ValidateVariableRange();

            Assert.Empty(result);
        }
    }
}