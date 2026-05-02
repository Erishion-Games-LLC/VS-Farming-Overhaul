using FarmingOverhaul.src.Helpers.Validation;
using System.Collections.Generic;

namespace FarmingOverhaul.src.Constants
{
    public interface IConstants
    {
        public List<ValidationResult> ValidateVariableRange();
    }
}