using FarmingOverhaul.src.Helpers.Validation;
using System.Collections.Generic;
using static FarmingOverhaul.src.Helpers.HelperFunctions;
namespace FarmingOverhaul.src.Constants.AnimalsConstants
{
    public class PregnancyConstants : IConstants
    {
        public double MinDaysPregnant;
        public double MaxDaysPregnant;
        public int MinFetusAmount;
        public int MaxFetusAmount;
        public double LateGestationPercent;

        public List<ValidationResult> ValidateVariableRange()
        {
            List<ValidationResult> errors = [];

            AddIfNotNull(errors, ValidateRange(MinDaysPregnant, MaxDaysPregnant, nameof(MinDaysPregnant)));
            AddIfNotNull(errors, ValidateRange(MinFetusAmount, MaxFetusAmount, nameof(MinFetusAmount)));

            return errors;
        }
    }
}