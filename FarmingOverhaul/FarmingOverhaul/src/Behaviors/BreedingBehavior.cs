using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using static FarmingOverhaul.src.Config.FarmingOverhaulServerConfig;

namespace FarmingOverhaul.src.Behaviors
{
    public class BreedingBehavior(Entity entity) : BaseBehavior(entity)
    {
        public override string PropertyNameKey => AttributeKeys.BreedingBehaviorKey;
        public override string PropertyName() => PropertyNameKey;
        public override string TreeKey => PropertyNameKey;

        private AnimalState animalState;
        private AnimalConstants constants;

        public int MinDaysPregnant;
        public int MaxDaysPregnant;

        public bool LateGestation
        {
            get
            {
                return GetBoolFromTree(AttributeKeys.LateGestationKey);
            }
            set
            {
                SetBoolInTree(AttributeKeys.LateGestationKey, value);
            }
        }

        public int PregnancyLength
        {
            get
            {
                return GetIntFromTree(AttributeKeys.PregnancyLengthKey);
            }
            set
            {
                SetIntInTree(AttributeKeys.PregnancyLengthKey, value);
            }
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
            constants = animalState.constants;

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