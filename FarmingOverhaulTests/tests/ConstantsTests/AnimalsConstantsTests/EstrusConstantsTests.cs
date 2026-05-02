using FarmingOverhaul.src.Constants.AnimalsConstants;
using FarmingOverhaul.src.Helpers.Validation;
using static FarmingOverhaulTests.tests.UtilitiesForTesting.TestHelpers;

namespace FarmingOverhaulTests.tests.ConstantsTests.AnimalsConstantsTests
{
    public class EstrusConstantsTests
    {
        [Fact]
        public void ValidateVariableRange_ReturnsError_WhenVariablesAreInvalid()
        {
            EstrusConstants estrus = new()
            {
                EstrusCycleMinDays = 5,
                EstrusCycleMaxDays = 1,

                TimeBeforeHeatMinDays = 5,
                TimeBeforeHeatMaxDays = 1,

                HeatDurationMinDays = 5,
                HeatDurationMaxDays = 1,

                TimeBeforePeakFertilityMinDays = 5,
                TimeBeforePeakFertilityMaxDays = 1,

                PeakFertilityMinDays = 5,
                PeakFertilityMaxDays = 1,
            };

            List<ValidationResult> results = estrus.ValidateVariableRange();

            AssertListHasExactAmount(results, 5);

            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, nameof(EstrusConstants.EstrusCycleMinDays));
            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, nameof(EstrusConstants.TimeBeforeHeatMinDays));
            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, nameof(EstrusConstants.HeatDurationMinDays));
            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, nameof(EstrusConstants.TimeBeforePeakFertilityMinDays));
            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, nameof(EstrusConstants.PeakFertilityMinDays));
        }

        [Fact]
        public void ValidateVariableRange_ReturnsEmpty_WhenVariablesAreValid()
        {
            EstrusConstants estrus = new()
            {
                EstrusCycleMinDays = 1,
                EstrusCycleMaxDays = 2,

                TimeBeforeHeatMinDays = 1,
                TimeBeforeHeatMaxDays = 2,

                HeatDurationMinDays = 1,
                HeatDurationMaxDays = 2,

                TimeBeforePeakFertilityMinDays = 1,
                TimeBeforePeakFertilityMaxDays = 2,

                PeakFertilityMinDays = 1,
                PeakFertilityMaxDays = 2,
            };

            List<ValidationResult> result = estrus.ValidateVariableRange();

            Assert.Empty(result);
        }
    }
}