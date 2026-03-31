using System.Collections.Generic;
using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Config
{
    public class FarmingOverhaulServerConfig
    {
        public Dictionary<string, AnimalConstants> Species = new()
        {
            //["hare"] = new AnimalConstants 
            //{             
            //    //MaintenanceEnergyReqConst = ,
            //    //BodyWeightExponent = ,
            //    //GrowthCoefficient = ,
            //    //LactationEnergyReqConst = ,
            //    //KCalPerKgWeightChange = ,
            //    //MinDaysPregnant = ,
            //    //MaxDaysPregnant = ,
            //    //LateGestationPercent = ,
            //    //BaseFetusEnergyModifier = ,
            //    //AdditionalFetusEnergyModifier = ,
            //    //EarlyGestationFetusEnergyModifier = ,
            //    //KCalPerKgMilkProduced = ,
            //    //LowerTempRange = ,
            //    //UpperTempRange =
            //},
            //["chicken"] = new AnimalConstants 
            //{
            //    //MaintenanceEnergyReqConst = ,
            //    //BodyWeightExponent = ,
            //    //GrowthCoefficient = ,
            //    //LactationEnergyReqConst = ,
            //    //KCalPerKgWeightChange = ,
            //    //MinDaysPregnant = ,
            //    //MaxDaysPregnant = ,
            //    //LateGestationPercent = ,
            //    //BaseFetusEnergyModifier = ,
            //    //AdditionalFetusEnergyModifier = ,
            //    //EarlyGestationFetusEnergyModifier = ,
            //    //KCalPerKgMilkProduced = ,
            //    //LowerTempRange = ,
            //    //UpperTempRange =
            //},
            //["pig"] = new AnimalConstants
            //{
            //    //MaintenanceEnergyReqConst = ,
            //    //BodyWeightExponent = ,
            //    //GrowthCoefficient = ,
            //    //LactationEnergyReqConst = ,
            //    //KCalPerKgWeightChange = ,
            //    //MinDaysPregnant = ,
            //    //MaxDaysPregnant = ,
            //    //LateGestationPercent = ,
            //    //BaseFetusEnergyModifier = ,
            //    //AdditionalFetusEnergyModifier = ,
            //    //EarlyGestationFetusEnergyModifier = ,
            //    //KCalPerKgMilkProduced = ,
            //    //LowerTempRange = ,
            //    //UpperTempRange =
            //},
            ["sheep"] = new AnimalConstants
            {
                MaintenanceEnergyReqConst = 0.056,
                BodyWeightExponent = 0.75,
                GrowthCoefficient = 0.276,
                LactationEnergyReqConst = 5.0,
                KCalPerKgWeightChange = 7000.0,
                MinDaysPregnant = 0.1,
                MaxDaysPregnant = 0.2,
                MinDaysBeforeBreedAgainFemale = 0.1,
                MaxDaysBeforeBreedAgainFemale = 0.2,
                MinDaysBeforeBreedAgainMale = 0.0,
                MaxDaysBeforeBreedAgainMale = 0.0,
                LateGestationPercent = 0.7,
                TimeAwayFromAnyMaleToTriggerHeat = 0.0f,
                RadiusMaleForHeat = 0.0f,
                BreedingSeason = [EnumMonth.January, EnumMonth.February],
                EstrusCycleMinDays = 0.0,
                EstrusCycleMaxDays = 0.0,
                EstrusCycleTimeBeforeHeatMinHours = 0.0,
                EstrusCycleTimeBeforeHeatMaxHours = 0.0,
                EstrusCycleHeatDurationMinHours = 0.0,
                EstrusCycleHeatDurationMaxHours = 0.0,
                EstrusCycleTimeBeforePeakFertilityMinHours = 0.0,
                EstrusCycleTimeBeforePeakFertilityMaxHours = 0.0,
                EstrusCyclePeakFertilityMinHours = 0.0,
                EstrusCyclePeakFertilityMaxHours = 0.0,
                MinFetusAmount = 80,
                MaxFetusAmount = 80,
                BaseFetusEnergyModifier = 0.1,
                AdditionalFetusEnergyModifier = 0.05,
                EarlyGestationFetusEnergyModifier = 0.025,
                ImpregnationFailChance = 0.0,
                KCalPerKgMilkProduced = 0.0,
                LowerTempRange = 0.0,
                UpperTempRange = 0.0
            },
            //["goat"] = new AnimalConstants
            //{
            //    //MaintenanceEnergyReqConst = ,
            //    //BodyWeightExponent = ,
            //    //GrowthCoefficient = ,
            //    //LactationEnergyReqConst = ,
            //    //KCalPerKgWeightChange = ,
            //    //MinDaysPregnant = ,
            //    //MaxDaysPregnant = ,
            //    //LateGestationPercent = ,
            //    //BaseFetusEnergyModifier = ,
            //    //AdditionalFetusEnergyModifier = ,
            //    //EarlyGestationFetusEnergyModifier = ,
            //    //KCalPerKgMilkProduced = ,
            //    //LowerTempRange = ,
            //    //UpperTempRange =
            //},

            //MaintenanceEnergyReqConst = ,
            //BodyWeightExponent = ,
            //GrowthCoefficient = ,
            //LactationEnergyReqConst = ,
            //KCalPerKgWeightChange = ,
            //MinDaysPregnant = ,
            //MaxDaysPregnant = ,
            //LateGestationPercent = ,
            //BaseFetusEnergyModifier = ,
            //AdditionalFetusEnergyModifier = ,
            //EarlyGestationFetusEnergyModifier = ,
            //KCalPerKgMilkProduced = ,
            //LowerTempRange = ,
            //UpperTempRange = 
        };
    }
}