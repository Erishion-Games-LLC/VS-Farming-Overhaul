using FarmingOverhaul.src.Config;
using FarmingOverhaul.src.Constants.AnimalsConstants;
using FarmingOverhaulTests.tests.UtilitiesForTesting;
using Vintagestory.API.Common;

namespace FarmingOverhaulTests.tests.ConfigTests
{
    public class ConfigValidatorTests
    {
        [Fact]
        public void ValidateConfig_IfConfigIsNull_ReturnNewConfig()
        {
            FarmingOverhaulServerConfig config = null;
            string fileName = "FarmingOverhaulServerConfig.json";
            ILogger logger = new TestLogger();

            var validatedConfig = ConfigValidator.ValidateConfig(config, logger, fileName);

            Assert.NotNull(validatedConfig);
            Assert.Equivalent(new FarmingOverhaulServerConfig(), validatedConfig);
        }

        [Fact]
        public void ValidateConfig_IfConfigIsValid_ReturnsUnchangedConfig()
        {
            FarmingOverhaulServerConfig config = new();
            string fileName = "FarmingOverhaulServerConfig.json";
            ILogger logger = new TestLogger();

            var validatedConfig = ConfigValidator.ValidateConfig(config, logger, fileName);

            Assert.Same(config, validatedConfig);           
        }

        [Fact]
        public void ValidateConfig_IfConfigHasErrors_ReturnNewConfig()
        {
            FarmingOverhaulServerConfig config = new();
            AnimalConstants sheepConstants = config.Species[FarmingOverhaulServerConfig.SheepKey];
            //make the pregnancy constants invalid
            sheepConstants.Pregnancy.MinDaysPregnant = 5;
            sheepConstants.Pregnancy.MaxDaysPregnant = 1;

            string fileName = "FarmingOverhaulServerConfig.json";
            ILogger logger = new TestLogger();

            var validatedConfig = ConfigValidator.ValidateConfig(config, logger, fileName);

            Assert.NotSame(config, validatedConfig);
            Assert.Equivalent(new FarmingOverhaulServerConfig(), validatedConfig);
        }
    }
}