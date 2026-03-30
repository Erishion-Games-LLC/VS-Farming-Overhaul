using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using static FarmingOverhaul.src.HelperFunctions;

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
        private WeightBehavior weightBehavior;


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
            weightBehavior = entity.GetBehavior<WeightBehavior>();

            if (animalState == null || weightBehavior == null)
            {
                Logger.Error("FARMING OVERHAUL missing required behaviors for Breeding Behavior to function: " + GetSpeciesStringLowerFromEntity(entity));
                return;
            }
            constants = animalState.Constants;

            if (ApiSide == EnumAppSide.Server) { EnableTickListeners(); }            
        }

        public override void OnEntityReceiveDamage(DamageSource damageSource, ref float damage)
        {
            base.OnEntityReceiveDamage(damageSource, ref damage);
        }
        public override void OnEntityDeath(DamageSource damageSourceForDeath)
        {
            base.OnEntityDeath(damageSourceForDeath);
            DieOrDespawn();
        }
        public override void OnEntityDespawn(EntityDespawnData despawn)
        {
            base.OnEntityDespawn(despawn);
            DieOrDespawn();
        }

        public void DieOrDespawn()
        {
            DisableTickListeners();
        }
        public override void EnableTickListeners()
        {
        }
        
        public override void DisableTickListeners()
        {
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