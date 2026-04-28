using System.Collections.Generic;
using static FarmingOverhaul.src.HelperFunctions;

namespace FarmingOverhaul.src.Constants.AnimalConstants
{
    public class BreedingConstants : IConstants
    {
        public double MinDaysBeforeBreedAgainFemale;
        public double MaxDaysBeforeBreedAgainFemale;
        public double MinDaysBeforeBreedAgainMale;
        public double MaxDaysBeforeBreedAgainMale;
        public double ImpregnationFailChance;


        //What time a female needs to be away from all males to have the "Ram Effect" happen
        public float TimeAwayFromAnyMaleToTriggerHeat;
        //What radius a male animal needs to be in to trigger the "Ram Effect"
        public float RadiusMaleForHeat;

        public List<string> ValidateVariableRange()
        {
            List<string> errors = [];

            AddIfNotNull(errors, ValidateRange(MinDaysBeforeBreedAgainFemale, MaxDaysBeforeBreedAgainFemale, nameof(MinDaysBeforeBreedAgainFemale)));
            AddIfNotNull(errors, ValidateRange(MinDaysBeforeBreedAgainMale, MaxDaysBeforeBreedAgainMale, nameof(MinDaysBeforeBreedAgainMale)));

            return errors;
        }
    }
}