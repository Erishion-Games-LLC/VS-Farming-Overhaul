//using Vintagestory.API.Common.Entities;
//using Vintagestory.API.Datastructures;

//namespace FarmingOverhaul.src.Systems.Breeding.Behaviors
//{
//    internal class MaleBreedingBehavior(Entity entity) : BaseBreedingBehavior(entity)
//    {
//        public const string MaleBreedingBehaviorKey = "fomalebreeding";
//        protected override string PropertyNameKey => MaleBreedingBehaviorKey;

//        private MaleReproductionStateManager stateManager;

//        public override void Initialize(EntityProperties properties, JsonObject attributes)
//        {
//            base.Initialize(properties, attributes);

//            stateManager = new MaleReproductionStateManager(Rand, animalState, TreeAccessor, Logger);
//        }

//        protected override void UpdateReproduction(float deltaTime)
//        {
//            base.UpdateReproduction(deltaTime);
//            stateManager.Update(Calendar.TotalDays, Calendar.MonthName);
//        }
//    }
//}
