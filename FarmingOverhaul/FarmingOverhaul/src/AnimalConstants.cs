using Vintagestory.API.Common;

namespace FarmingOverhaul.src
{
    public class AnimalConstants
    {
        //Constants used by WeightBehavior mainly
        public double MaintenanceEnergyReqCons;
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
    }
}