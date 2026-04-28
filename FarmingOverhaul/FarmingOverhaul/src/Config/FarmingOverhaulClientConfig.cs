using System.Collections.Generic;

namespace FarmingOverhaul.src.Config
{
    public class FarmingOverhaulClientConfig : IValidatableConfig
    {
        public int testClient1 = 0;

        public List<string> Validate()
        {
            return [];
        }
    }
}