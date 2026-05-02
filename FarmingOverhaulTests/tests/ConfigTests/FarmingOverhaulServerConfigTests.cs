using FarmingOverhaul.src.Config;
using FarmingOverhaul.src.Helpers.Validation;
using FarmingOverhaulTests.tests.ConstantsTests;
using static FarmingOverhaulTests.tests.UtilitiesForTesting.TestHelpers;

namespace FarmingOverhaulTests.tests.ConfigTests
{
    public class FarmingOverhaulServerConfigTests
    {
        static string SheepKey = FarmingOverhaulServerConfig.SheepKey;
        static string GoatKey = FarmingOverhaulServerConfig.GoatKey;


        private static FarmingOverhaulServerConfig CreateConfigStub()
        {
            var config = new FarmingOverhaulServerConfig
            {
                Species = new()
                {
                    [SheepKey] = ConstantsFakes.CreateDefaultAnimalConstants(),
                    [GoatKey] = ConstantsFakes.CreateDefaultAnimalConstants()
                }
            };
            return config;
        }

        [Fact]
        public void Validate_IfValidSpecies_ReturnEmptyList()
        {
            var config = CreateConfigStub();

            var results = config.Validate();

            Assert.Empty(results);
        }

        [Fact]
        public void Validate_MultipleSpecies_HandlesCorrectly()
        {
            var config = CreateConfigStub();

            config.Species[SheepKey].Pregnancy.MinDaysPregnant = 10;
            config.Species[SheepKey].Pregnancy.MaxDaysPregnant = 1;

            config.Species[GoatKey].Pregnancy.MinDaysPregnant = 10;
            config.Species[GoatKey].Pregnancy.MaxDaysPregnant = 1;

            var results = config.Validate();

            Assert.Contains(results, result => result.Error == ValidationErrorType.MinGreaterThanMax && result.ValidatedItem.StartsWith($"{SheepKey}."));
            Assert.Contains(results, result => result.Error == ValidationErrorType.MinGreaterThanMax && result.ValidatedItem.StartsWith($"{GoatKey}."));
        }

        [Fact]
        public void Validate_WhenSpeciesIsNull_ReturnsNullClassError()
        {
            var config = CreateConfigStub();

            config.Species[SheepKey] = null;

            var results = config.Validate();

            AssertHasValidationError(results, ValidationErrorType.NullClass, SheepKey);
        }

        [Fact]
        public void Validate_MultipleErrorsPerSpecies_AllAreReturned()
        {
            var config = CreateConfigStub();

            config.Species[SheepKey].Pregnancy.MinDaysPregnant = 10;
            config.Species[SheepKey].Pregnancy.MaxDaysPregnant = 1;
            config.Species[SheepKey].Pregnancy.MinFetusAmount = 10;
            config.Species[SheepKey].Pregnancy.MaxFetusAmount = 1;


            config.Species[GoatKey].Pregnancy.MinDaysPregnant = 10;
            config.Species[GoatKey].Pregnancy.MaxDaysPregnant = 1;
            config.Species[GoatKey].Pregnancy.MinFetusAmount = 10;
            config.Species[GoatKey].Pregnancy.MaxFetusAmount = 1;

            var results = config.Validate();

            AssertListHasExactAmount(results, 4);
            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, $"{SheepKey}.MinDaysPregnant");
            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, $"{SheepKey}.MinFetusAmount");
            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, $"{GoatKey}.MinDaysPregnant");
            AssertHasValidationError(results, ValidationErrorType.MinGreaterThanMax, $"{GoatKey}.MinFetusAmount");
        }
    }
}