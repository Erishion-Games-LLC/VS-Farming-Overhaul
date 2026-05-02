using FarmingOverhaul.src.Constants.AnimalsConstants;
using FarmingOverhaul.src.Helpers.Validation;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Config
{
    public class FarmingOverhaulServerConfig : IValidatableConfig
    {
        public const string HareKey = "hare";
        public const string ChickenKey = "chicken";
        public const string PigKey = "pig";
        public const string SheepKey = "sheep";
        public const string GoatKey = "goat";

        public List<ValidationResult> Validate()
        {
            List<ValidationResult> errors = [];

            foreach (var species in Species)
            {
                if (species.Value == null)
                {
                    errors.Add(new ValidationResult(ValidationErrorType.NullClass, $"{species.Key}"));
                    continue;
                }

                List<ValidationResult> speciesErrors = species.Value.Validate();

                foreach (var error in speciesErrors)
                {
                    errors.Add(new ValidationResult(error.Error, $"{species.Key}.{error.ValidatedItem}"));
                }
            }

            return errors;
        }

        public Dictionary<string, AnimalConstants> Species = new()
        {
            //["hare"] = new AnimalConstants

            //["chicken"] = new AnimalConstants 

            //["pig"] = new AnimalConstants

            [SheepKey] = new AnimalConstants
            {
                Estrus = new EstrusConstants
                {
                    BreedingSeason = [EnumMonth.January, EnumMonth.February],
                    EstrusCycleMinDays = 0.0,
                    EstrusCycleMaxDays = 0.0,
                    TimeBeforeHeatMinDays = 0.0,
                    TimeBeforeHeatMaxDays = 0.0,
                    HeatDurationMinDays = 0.0,
                    HeatDurationMaxDays = 0.0,
                    TimeBeforePeakFertilityMinDays = 0.0,
                    TimeBeforePeakFertilityMaxDays = 0.0,
                    PeakFertilityMinDays = 0.0,
                    PeakFertilityMaxDays = 0.0,
                },
                Pregnancy = new PregnancyConstants
                {
                    MinDaysPregnant = 0.1,
                    MaxDaysPregnant = 0.2,
                    MinFetusAmount = 80,
                    MaxFetusAmount = 80,
                    LateGestationPercent = 0.7,
                },
                Breeding = new BreedingConstants
                {
                    MinDaysBeforeBreedAgainFemale = 0.1,
                    MaxDaysBeforeBreedAgainFemale = 0.2,
                    MinDaysBeforeBreedAgainMale = 0.0,
                    MaxDaysBeforeBreedAgainMale = 0.0,
                    ImpregnationFailChance = 0.0,
                    TimeAwayFromAnyMaleToTriggerHeat = 0.0f,
                    RadiusMaleForHeat = 0.0f,
                },
                Energy = new EnergyConstants
                {
                    MaintenanceEnergyReqConst = 0.056,
                    BodyWeightExponent = 0.75,
                    GrowthCoefficient = 0.276,
                    LactationEnergyReqConst = 5.0,
                    KCalPerKgWeightChange = 7000.0,
                    BaseFetusEnergyModifier = 0.1,
                    AdditionalFetusEnergyModifier = 0.05,
                    EarlyGestationFetusEnergyModifier = 0.025,
                    KCalPerKgMilkProduced = 0.0,
                    LowerTempRange = 0.0,
                    UpperTempRange = 0.0
                }
            },
            //["goat"] = new AnimalConstants


            //{
            //    Estrus = new EstrusConstants
            //    {
            //        BreedingSeason = [EnumMonth.January, EnumMonth.February],
            //        EstrusCycleMinDays = ,
            //        EstrusCycleMaxDays = ,
            //        TimeBeforeHeatMinHours = ,
            //        TimeBeforeHeatMaxHours = ,
            //        HeatDurationMinHours = ,
            //        HeatDurationMaxHours = ,
            //        TimeBeforePeakFertilityMinHours = ,
            //        TimeBeforePeakFertilityMaxHours = ,
            //        PeakFertilityMinHours = ,
            //        PeakFertilityMaxHours = ,
            //    },
            //    Pregnancy = new PregnancyConstants
            //    {
            //        MinDaysPregnant = ,
            //        MaxDaysPregnant = ,
            //        MinFetusAmount = ,
            //        MaxFetusAmount = ,
            //        LateGestationPercent = ,
            //    },
            //    Breeding = new BreedingConstants
            //    {
            //        MinDaysBeforeBreedAgainFemale = ,
            //        MaxDaysBeforeBreedAgainFemale = ,
            //        MinDaysBeforeBreedAgainMale = ,
            //        MaxDaysBeforeBreedAgainMale = ,
            //        ImpregnationFailChance = ,
            //        TimeAwayFromAnyMaleToTriggerHeat = ,
            //        RadiusMaleForHeat = ,
            //    },
            //    Energy = new EnergyConstants
            //    {
            //        MaintenanceEnergyReqConst = ,
            //        BodyWeightExponent = ,
            //        GrowthCoefficient = ,
            //        LactationEnergyReqConst = ,
            //        KCalPerKgWeightChange = ,
            //        BaseFetusEnergyModifier = ,
            //        AdditionalFetusEnergyModifier = ,
            //        EarlyGestationFetusEnergyModifier = ,
            //        KCalPerKgMilkProduced = ,
            //        LowerTempRange = ,
            //        UpperTempRange = 
            //    }
            //}

        };
    }
}