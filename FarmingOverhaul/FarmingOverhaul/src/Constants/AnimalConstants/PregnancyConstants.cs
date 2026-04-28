using System.Collections.Generic;
using static FarmingOverhaul.src.HelperFunctions;
namespace FarmingOverhaul.src.Constants.AnimalConstants
{
    public class PregnancyConstants : IConstants
    {
        public double MinDaysPregnant;
        public double MaxDaysPregnant;
        public int MinFetusAmount;
        public int MaxFetusAmount;
        public double LateGestationPercent;

        public List<string> ValidateVariableRange()
        {
            List<string> errors = [];

            AddIfNotNull(errors, ValidateRange(MinDaysPregnant, MaxDaysPregnant, nameof(MinDaysPregnant)));
            AddIfNotNull(errors, ValidateRange(MinFetusAmount, MaxFetusAmount, nameof(MinFetusAmount)));

            return errors;
        }
    }
}