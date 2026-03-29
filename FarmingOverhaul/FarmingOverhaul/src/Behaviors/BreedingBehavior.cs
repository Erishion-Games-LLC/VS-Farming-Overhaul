using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace FarmingOverhaul.src.Behaviors
{
    public class BreedingBehavior(Entity entity) : BaseBehavior(entity)
    {
        public const string BreedingBehaviorKey = "fobreeding";
        public override string PropertyNameKey => BreedingBehaviorKey;
        public override string PropertyName() => PropertyNameKey;
        public override string TreeKey => PropertyNameKey;

        private AnimalState animalState;
        private AnimalConstants constants;

        public double MinDaysPregnant;
        public double MaxDaysPregnant;

        public double PregnancyLengthDays
        {
            get { return GetDoubleFromTree(nameof(PregnancyLengthDays)); }
            set { SetDoubleInTree(nameof(PregnancyLengthDays), value); }
        }
        public bool LateGestation
        {
            get { return GetBoolFromTree(nameof(LateGestation)); }
            set { SetBoolInTree(nameof(LateGestation), value); }
        }


        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);
            animalState = entity.GetBehavior<AnimalState>();

            if (animalState == null)
            {
                logger.Error("FARMING OVERHAUL animal state is null for: " + HelperFunctions.GetSpeciesStringLowerFromEntity(entity));
                return; 
            }
            constants = animalState.Constants;

            MinDaysPregnant = constants.MinDaysPregnant;
            MaxDaysPregnant = constants.MaxDaysPregnant;
        }

        public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
        {
            base.OnEntityReceiveDamage(damageSource, ref damage);
        }



        //private bool InLateGestation(Entity animal)
        //{
        //    if (DaysPregnant / pregnancyLength < Constants.LateGestationPercent)
        //    {
        //        return false;
        //    }
        //    else return true;
        //}
    }
}