using FarmingOverhaul.src.Helpers.Validation;
using System.Collections.Generic;

namespace FarmingOverhaul.src.Config
{
    public interface IValidatableConfig
    {
        List<ValidationResult> Validate();
    }
}