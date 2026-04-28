using static FarmingOverhaulTests.tests.Constants.ConstantsFakes;

namespace FarmingOverhaulTests.tests.Constants
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
        [InlineData("Estrus")]
        [InlineData("Pregnancy")]
        [InlineData("Breeding")]
        [InlineData("Energy")]
        public void Validate_NullConstants_ReturnsErrors(string field)
        {
            var constants = CreateDefaultAnimalConstants();
            switch (field)
            {
                case "Estrus": constants.Estrus = null; break;
                case "Pregnancy": constants.Pregnancy = null; break;
                case "Breeding": constants.Breeding = null; break;
                case "Energy": constants.Energy = null; break;
            }

            var errors = constants.Validate();

            Assert.Contains($"{field} constants cannot be null", errors);
        }

        [Fact]
        public void Validate_InvalidEstrusConstants_ReturnsErrors()
        {
            var constants = CreateDefaultAnimalConstants();
            constants.Estrus.EstrusCycleMinDays = -1;
            constants.Estrus.EstrusCycleMaxDays = -2;
            var errors = constants.Validate();
            Assert.Contains("Estrus Cycle Min Days must be greater than or equal to 0", errors);
            Assert.Contains("Estrus Cycle Max Days must be greater than or equal to 0", errors);
        }
    }
}