using FarmingOverhaul.src.Behaviors;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using static FarmingOverhaul.src.HelperFunctions;
using static FarmingOverhaul.src.Systems.Breeding.BreedingLogic;

namespace FarmingOverhaul.src.Systems.Breeding
{
    public class BreedingBehavior(Entity entity) : BaseBehavior(entity)
    {
        public const string BreedingBehaviorKey = "fobreeding";
        public override string PropertyNameKey => BreedingBehaviorKey;
        public override string PropertyName() => PropertyNameKey;
        public override string TreeKey => PropertyNameKey;

        private AnimalState animalState;
        private WeightBehavior weightBehavior;

        private long updateReproductionTickListenerId = 0;
        private int updateReproductionTimerMs;
        private ReproductionStateManager stateManager;


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

            stateManager = new ReproductionStateManager(Rand, animalState, TreeAccessor, Logger);
            stateManager.OnBirth += GiveBirth;

            /*Calculate how often the reproduction function should be called based on the game speed.
             * We want to call every 30 in game minutes, so we divide 1800 seconds by the game speed to get how many real life seconds should be between each call,
             * and then multiply by 1000 to convert to milliseconds for the timer.
             */
            //updateReproductionTimerMs = (int)(1800 / (Calendar.SpeedOfTime * Calendar.CalendarSpeedMul)) * 1000;
            updateReproductionTimerMs = 15000;

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

        /*Entity Lifecycle for female animals -
         * DONE Naturally generated animals should (If generated during their breeding season) already be or have a chance to be, in their estrus cycle.
         * DONE We constantly check to see if the animal can start its estrus cycle. 
         * DONE If it can, we stop repeatedly checking and actually start the cycle. 
         * DONE The animal is now fully ready to breed.
         * TODO Have female animals exhibit signs of heat. 
         * TODO Implement the Ram Effect
         * TODO Have the male animal impregnate the female with an actual action that THEY decide to do, not just by being close.
         * DONE Check if the impregnation is successful
         * 
         * 
         * Check fertibility level based on when after the current heat started the ewe was inseminated and current weight
         * Combine fertibility level with something else
         * if check passes, ewe gets pregnant. Roll fetus count
        */
        public override void EnableTickListeners()
        {
            if (updateReproductionTickListenerId != 0) return;

            updateReproductionTickListenerId = Api.Event.RegisterGameTickListener(UpdateReproduction, updateReproductionTimerMs);
        }

        public override void DisableTickListeners()
        {
            Api.Event.UnregisterGameTickListener(updateReproductionTickListenerId);
        }

        protected void UpdateReproduction(float deltaTime)
        {
            Logger.Error("Start UpdateReproduction");
            //If animal is dead, stop all ticking and exit
            if (!entity.Alive) { DisableTickListeners(); Logger.Error("entity is dead"); return; }

            stateManager.Update(Calendar.TotalDays, Calendar.MonthName);
        }

        protected void GiveBirth(int fetusAmount)
        {
            while (fetusAmount > 0)
            {
                fetusAmount -= 1;
                SpawnChild(entity);
            }
        }

        //TODO
        //need to add support for father entity when genetics are implemented
        //Spawn a child entity with the appropriate species and type, based on the mother.
        protected void SpawnChild(Entity mother)
        {
            if (ApiSide != EnumAppSide.Server)
            {
                return;
            }
            string gender = DetermineGender(Rand);
            var rand = World.Rand;

            string entityCode = $"{animalState.Species}-{animalState.Type}-baby-{gender}";
            AssetLocation assetLocation = new AssetLocation(entityCode);

            EntityProperties entityType = World.GetEntityType(assetLocation);

            Entity child = World.ClassRegistry.CreateEntity(entityType);

            child.ServerPos.SetFrom(mother.ServerPos);
            child.ServerPos.Motion.X += (rand.NextDouble() - 0.5) / 20.0;
            child.ServerPos.Motion.Z += (rand.NextDouble() - 0.5) / 20.0;
            child.Pos.SetFrom(child.ServerPos);

            child.Attributes.SetString("origin", "reproduction");
            child.WatchedAttributes.SetInt("generation", animalState.Generation + 1);

            World.SpawnEntity(child);
        }

        ////Will be removed once animals need to mate themselves
        //protected bool IsRequiredMaleNearby()
        //{
        //    float range = constants.RadiusMaleForHeat;
        //    Entity[] entities = World.GetEntitiesAround(entity.Pos.XYZ, range, range);

        //    //Search all nearby entities in range for a male matching species
        //    foreach (Entity e in entities)
        //    {
        //        AnimalState aState = e.GetBehavior<AnimalState>();

        //        if (aState == null) continue;

        //        if (aState.Species != animalState.Species) continue;

        //        if (aState.Gender == "male") return true;
        //    }

        //    //No male matching species was found
        //    return false;
        //}     
    }
}