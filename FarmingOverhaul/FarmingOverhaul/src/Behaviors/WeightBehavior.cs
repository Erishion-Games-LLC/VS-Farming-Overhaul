using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace FarmingOverhaul.src.Behaviors
{
    public class WeightBehavior(Entity entity) : BaseBehavior(entity)
    {
        public const string WeightBehaviorKey = "foweight";
        public override string PropertyNameKey => WeightBehaviorKey;
        public override string PropertyName() => PropertyNameKey;
        public override string TreeKey => PropertyNameKey;

        private AnimalState animalState;
        private AnimalConstants constants;

        public double BodyWeight { get; set; } = 0;
        public double CaloriesConsumed { get; set; } = 0;
        public bool belowTempThresh { get; set; } = false;

        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);
            animalState = entity.GetBehavior<AnimalState>();

            if (animalState == null)
            {
                Logger.Error("FARMING OVERHAUL animal state is null for: " + HelperFunctions.GetSpeciesStringLowerFromEntity(entity));
                return;
            }
            constants = animalState.Constants;
        }


        //public void AdjustAnimalWeight(double totalMaintenanceEnergyReq)
        //{
        //    double excessCalories = caloriesConsumed - totalMaintenanceEnergyReq;

        //    if (excessCalories != 0)
        //    {
        //        this.bodyWeight += excessCalories / Constants.KCalPerKgWeightChange;
        //    }
        //}

        //public double CalculateTotalMaintenanceEnergyReq()
        //{
        //    double temperatureModifer = 1;
        //    double pregnancyEnergyReq;
        //    double metabolicBodyWeight = Math.Pow(bodyWeight, Constants.BodyWeightExponent);
        //    double baseMaintenanceEnergyReq = Constants.MaintenanceEnergyReqConst * metabolicBodyWeight;
        //    double lactationEnergyReq = Constants.LactationEnergyReqConst * kgMilkProducedDaily;

        //    if (InLateGestation())
        //        pregnancyEnergyReq = (Constants.BaseFetusEnergyModifier + (Constants.AdditionalFetusEnergyModifier * (FetusAmount - 1))) * bodyWeight;
        //    else if (FetusAmount > 0)
        //        pregnancyEnergyReq = Constants.EarlyGestationFetusEnergyModifier * FetusAmount * bodyWeight;
        //    else
        //        pregnancyEnergyReq = 0;

        //    //if (localTemperature > Constants.UpperTempRange)
        //    //{
        //    //    //have it scale based on high much higher it is
        //    //    temperatureModifer = 1.25;
        //    //}
        //    //if (localTemperature < Constants.LowerTempRange)
        //    //{
        //    //    //have it scale based on high much lower it is
        //    //    temperatureModifer = 1.25;
        //    //}
        //    return (baseMaintenanceEnergyReq + lactationEnergyReq + pregnancyEnergyReq) * temperatureModifer;
        //}
    }
}
