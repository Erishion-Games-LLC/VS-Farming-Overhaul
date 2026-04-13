using System.Collections.Generic;
using Vintagestory.API.Common;

namespace FarmingOverhaul.src
{
    public class AnimalConstants
    {
        //Constants used by WeightBehavior mainly
        public double MaintenanceEnergyReqConst;
        public double BodyWeightExponent;
        public double GrowthCoefficient;
        public double LactationEnergyReqConst;
        public double KCalPerKgWeightChange;

        //Constants used by BreedingBehavior mainly
        public double MinDaysPregnant;
        public double MaxDaysPregnant;
        public double MinDaysBeforeBreedAgainFemale;
        public double MaxDaysBeforeBreedAgainFemale;
        public double MinDaysBeforeBreedAgainMale;
        public double MaxDaysBeforeBreedAgainMale;
        public double LateGestationPercent;

        //What time a female needs to be away from all males to have the "Ram Effect" happen
        public float TimeAwayFromAnyMaleToTriggerHeat;
        //What radius a male animal needs to be in to trigger the "Ram Effect"
        public float RadiusMaleForHeat;
        //What months the animal is able to breed in. Defines when animal can start estrus cycle
        public EnumMonth[] BreedingSeason;
        //Estrus cycle is how long from start of heat to next start of heat
        public double EstrusCycleMinDays;
        public double EstrusCycleMaxDays;
        //Determines how long after estrus cycle starts heat begins
        public double EstrusCycleTimeBeforeHeatMinHours;
        public double EstrusCycleTimeBeforeHeatMaxHours;
        //Determines how long heat will actually last for
        public double EstrusCycleHeatDurationMinHours;
        public double EstrusCycleHeatDurationMaxHours;
        //Determines when after heat begins, will peak fertility be reached and when it will decline.
        public double EstrusCycleTimeBeforePeakFertilityMinHours;
        public double EstrusCycleTimeBeforePeakFertilityMaxHours;
        public double EstrusCyclePeakFertilityMinHours;
        public double EstrusCyclePeakFertilityMaxHours;


        public int MinFetusAmount;
        public int MaxFetusAmount;
        public double BaseFetusEnergyModifier;
        public double AdditionalFetusEnergyModifier;
        public double EarlyGestationFetusEnergyModifier;
        public double ImpregnationFailChance; 


        //
        public double KCalPerKgMilkProduced;
        public double LowerTempRange;
        public double UpperTempRange;


        public List<string> Validate()
        {
            List<string> errors = [];

            ValidateRange(errors, MinDaysPregnant, MaxDaysPregnant, nameof(MinDaysPregnant));
            ValidateRange(errors, MinDaysBeforeBreedAgainFemale, MaxDaysBeforeBreedAgainFemale, nameof(MinDaysBeforeBreedAgainFemale));
            ValidateRange(errors, MinDaysBeforeBreedAgainMale, MaxDaysBeforeBreedAgainMale, nameof(MinDaysBeforeBreedAgainMale));
            ValidateRange(errors, EstrusCycleMinDays, EstrusCycleMaxDays, nameof(EstrusCycleMinDays));
            ValidateRange(errors, EstrusCycleTimeBeforeHeatMinHours, EstrusCycleTimeBeforeHeatMaxHours, nameof(EstrusCycleTimeBeforeHeatMinHours));
            ValidateRange(errors, EstrusCycleHeatDurationMinHours, EstrusCycleHeatDurationMaxHours, nameof(EstrusCycleHeatDurationMinHours));
            ValidateRange(errors, EstrusCycleTimeBeforePeakFertilityMinHours, EstrusCycleTimeBeforePeakFertilityMaxHours, nameof(EstrusCycleTimeBeforePeakFertilityMinHours));
            ValidateRange(errors, EstrusCyclePeakFertilityMinHours, EstrusCyclePeakFertilityMaxHours, nameof(EstrusCyclePeakFertilityMinHours));
            ValidateRange(errors, MinFetusAmount, MaxFetusAmount, nameof(MinFetusAmount));

            return errors;
        }

        public static void ValidateRange(List<string> errors, double min, double max, string field)
        {
            if (min > max)
            {
                errors.Add($"{field} min value is greater than max value. Must be less than or equal to.");
            }
        }
    }
}