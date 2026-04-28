using System.Collections.Generic;

namespace FarmingOverhaul.src.Config
{
    public interface IValidatableConfig
    {
        List<string> Validate();
    }
}