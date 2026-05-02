using FarmingOverhaul.src.Helpers.Validation;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Config
{
    public static class ConfigValidator
    {
        public static T ValidateConfig<T>(T? config, ILogger logger, string fileName)
    where T : class, IValidatableConfig, new()
        {
            if (config == null)
            {
                logger.Error($"{fileName} is null. Loading default values.");
                config = new T();
                return config;
            }

            List<ValidationResult> errors = config.Validate();

            if (errors.Count > 0)
            {
                logger.Error($"Config {fileName} had errors: ");
                foreach (ValidationResult error in errors)
                {
                    logger.Error(error.CreateMessage());
                }

                logger.Error($"Replacing invalid {fileName} with default values.");

                config = new T();
            }

            return config;
        }
    }
}