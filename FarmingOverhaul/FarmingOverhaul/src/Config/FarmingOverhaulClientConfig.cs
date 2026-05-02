using FarmingOverhaul.src.Helpers.Validation;
using System.Collections.Generic;

namespace FarmingOverhaul.src.Config
{
    public class FarmingOverhaulClientConfig : IValidatableConfig
    {
        public int testClient1 = 0;

        public List<ValidationResult> Validate()
        {
            return [];
        }
    }
}