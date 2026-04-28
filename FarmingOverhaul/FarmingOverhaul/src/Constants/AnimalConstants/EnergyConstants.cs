using System.Collections.Generic;
using static FarmingOverhaul.src.HelperFunctions;
namespace FarmingOverhaul.src.Constants.AnimalConstants
{
    public class EnergyConstants : IConstants
    {
        public double MaintenanceEnergyReqConst;
        public double BodyWeightExponent;
        public double GrowthCoefficient;
        public double LactationEnergyReqConst;
        public double KCalPerKgWeightChange;

        public double BaseFetusEnergyModifier;
        public double AdditionalFetusEnergyModifier;
        public double EarlyGestationFetusEnergyModifier;

        public double KCalPerKgMilkProduced;
        public double LowerTempRange;
        public double UpperTempRange;

        public List<string> ValidateVariableRange()
        {
            List<string> errors = [];

            AddIfNotNull(errors, ValidateRange(LowerTempRange, UpperTempRange, nameof(LowerTempRange)));

            return errors;
        }
    }
}