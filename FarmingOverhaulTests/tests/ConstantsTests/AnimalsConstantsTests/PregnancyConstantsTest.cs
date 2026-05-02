using FarmingOverhaul.src.Constants.AnimalsConstants;
using FarmingOverhaul.src.Helpers.Validation;
using static FarmingOverhaulTests.tests.UtilitiesForTesting.TestHelpers;

namespace FarmingOverhaulTests.tests.ConstantsTests.AnimalsConstantsTests
{
    public class PregnancyConstantsTest
    {
        [Fact]
        public void ValidateVariableRange_ReturnsError_WhenVariablesAreInvalid()
        {
            PregnancyConstants pregnancy = new()
            {
                MinDaysPregnant = 5,
                MaxDaysPregnant = 1,
                MinFetusAmount = 5,
                MaxFetusAmount = 1
            };

            List<ValidationResult> results = pregnancy.ValidateVariableRange();

            AssertListHasExactAmount(results, 2);

            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, nameof(PregnancyConstants.MinDaysPregnant));
            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, nameof(PregnancyConstants.MinFetusAmount));
        }

        [Fact]
        public void ValidateVariableRange_ReturnsEmpty_WhenVariablesAreValid()
        {
            PregnancyConstants pregnancy = new()
            {
                MinDaysPregnant = 1,
                MaxDaysPregnant = 2,
                MinFetusAmount = 1,
                MaxFetusAmount = 2
            };

            List<ValidationResult> result = pregnancy.ValidateVariableRange();

            Assert.Empty(result);
        }
    }
}