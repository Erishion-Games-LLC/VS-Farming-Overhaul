using System.Collections.Generic;

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
            //["sheep"] = new AnimalConstants
            //{ 
            //    MaintenanceEnergyReqConst = 0.056,
            //    BodyWeightExponent = 0.75,
            //    GrowthCoefficient = 0.276,
            //    LactationEnergyReqConst = 5,
            //    KCalPerKgWeightChange = 7000,
            //    MinDaysPregnant = 1,
            //    MaxDaysPregnant = 9999,
            //    LateGestationPercent = 0.70,
            //    BaseFetusEnergyModifier = 0.1,
            //    AdditionalFetusEnergyModifier = 0.05,
            //    EarlyGestationFetusEnergyModifier = 0.025,
            //    KCalPerKgMilkProduced = 0,
            //    LowerTempRange = 0,
            //    UpperTempRange = 0
            //},
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

        public class AnimalConstants
        {
            public double MaintenanceEnergyReqConst;
            public double BodyWeightExponent;
            public double GrowthCoefficient;

            public double LactationEnergyReqConst;
            public int KCalPerKgWeightChange;

            public int MinDaysPregnant;
            public int MaxDaysPregnant;
            public double LateGestationPercent;

            public double BaseFetusEnergyModifier;
            public double AdditionalFetusEnergyModifier;
            public double EarlyGestationFetusEnergyModifier;

            public double KCalPerKgMilkProduced;
            public double LowerTempRange;
            public double UpperTempRange;
        }
    }
}