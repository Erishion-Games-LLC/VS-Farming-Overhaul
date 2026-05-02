using FarmingOverhaul.src.Constants.AnimalsConstants;
using Vintagestory.API.Common;

namespace FarmingOverhaulTests.tests.ConstantsTests
{
    public static class ConstantsFakes
    {
        public static AnimalConstants CreateDefaultAnimalConstants()
        {
            var animalConstants = new AnimalConstants
            {
                Estrus = CreateDefaultEstrus(),
                Pregnancy = CreateDefaultPregnancy(),
                Breeding = CreateDefaultBreeding(),
                Energy = CreateDefaultEnergy()
            };
            return animalConstants;
        }

        public static EstrusConstants CreateDefaultEstrus()
        {
            var estrus = new EstrusConstants
            {
                BreedingSeason = [EnumMonth.March],
                EstrusCycleMinDays = 1,
                EstrusCycleMaxDays = 2,
                TimeBeforeHeatMinDays = 1,
                TimeBeforeHeatMaxDays = 2,
                HeatDurationMinDays = 1,
                HeatDurationMaxDays = 2,
                TimeBeforePeakFertilityMinDays = 1,
                TimeBeforePeakFertilityMaxDays = 2,
                PeakFertilityMinDays = 1,
                PeakFertilityMaxDays = 2
            };
            return estrus;
        }

        public static PregnancyConstants CreateDefaultPregnancy()
        {
            var pregnancy = new PregnancyConstants
            {
                MinDaysPregnant = 1,
                MaxDaysPregnant = 2,
                MinFetusAmount = 1,
                MaxFetusAmount = 2,
                LateGestationPercent = 0.5
            };
            return pregnancy;
        }

        public static BreedingConstants CreateDefaultBreeding()
        {
            var breeding = new BreedingConstants
            {
                MinDaysBeforeBreedAgainFemale = 1,
                MaxDaysBeforeBreedAgainFemale = 2,
                MinDaysBeforeBreedAgainMale = 1,
                MaxDaysBeforeBreedAgainMale = 2,
                ImpregnationFailChance = 0.1,
                TimeAwayFromAnyMaleToTriggerHeat = 1,
                RadiusMaleForHeat = 1
            };
            return breeding;
        }

        public static EnergyConstants CreateDefaultEnergy()
        {
            var energy = new EnergyConstants
            {
                MaintenanceEnergyReqConst = 1,
                BodyWeightExponent = 1,
                GrowthCoefficient = 1,
                LactationEnergyReqConst = 1,
                KCalPerKgWeightChange = 1,
                BaseFetusEnergyModifier = 1,
                AdditionalFetusEnergyModifier = 1,
                EarlyGestationFetusEnergyModifier = 1,
                KCalPerKgMilkProduced = 1,
                LowerTempRange = 1,
                UpperTempRange = 2
            };
            return energy;
        }
    }
}
