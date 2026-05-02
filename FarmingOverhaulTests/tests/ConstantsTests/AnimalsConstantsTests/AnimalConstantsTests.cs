using FarmingOverhaul.src.Constants.AnimalsConstants;
using FarmingOverhaul.src.Helpers.Validation;
using static FarmingOverhaulTests.tests.ConstantsTests.ConstantsFakes;
using static FarmingOverhaulTests.tests.UtilitiesForTesting.TestHelpers;

namespace FarmingOverhaulTests.tests.ConstantsTests.AnimalsConstantsTests
{
    public class AnimalConstantsTests
    {
        [Fact]
        public void Validate_AllConstantsValid_ReturnsEmptyList()
        {
            var constants = CreateDefaultAnimalConstants();

            var errors = constants.Validate();

            Assert.Empty(errors);
        }

        [Theory]
        [InlineData(nameof(AnimalConstants.Estrus))]
        [InlineData(nameof(AnimalConstants.Pregnancy))]
        [InlineData(nameof(AnimalConstants.Breeding))]
        [InlineData(nameof(AnimalConstants.Energy))]
        public void Validate_NullConstants_ReturnsErrors(string field)
        {
            var constants = CreateDefaultAnimalConstants();
            switch (field)
            {
                case nameof(AnimalConstants.Estrus): constants.Estrus = null; break;
                case nameof(AnimalConstants.Pregnancy): constants.Pregnancy = null; break;
                case nameof(AnimalConstants.Breeding): constants.Breeding = null; break;
                case nameof(AnimalConstants.Energy): constants.Energy = null; break;
            }

            List<ValidationResult> errors = constants.Validate();

            AssertHasValidationError(errors, ValidationErrorType.NullClass, field);
        }
    }
}