using FarmingOverhaul.src.Behaviors;
using FarmingOverhaul.src.Config;
using FarmingOverhaul.src.Systems.Breeding.Behaviors;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace FarmingOverhaul.src
{
    public class FarmingOverhaulModSystem : ModSystem
    {
        public static string? ModID;    

        public override void Start(ICoreAPI api)
        {
            ModID = Mod.Info.ModID;
            RegisterClasses(api);
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            ConfigManager.StartServer(api);
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            ConfigManager.StartClient(api);
        }

        private static void RegisterClasses(ICoreAPI api)
        {
            //Register Behaviors
            api.RegisterEntityBehaviorClass(BaseBehavior.BaseBehaviorKey, typeof(BaseBehavior));
            api.RegisterEntityBehaviorClass(BaseBreedingBehavior.BaseBreedingBehaviorKey, typeof(BaseBreedingBehavior));
            api.RegisterEntityBehaviorClass(AnimalState.AnimalStateKey, typeof(AnimalState));
            api.RegisterEntityBehaviorClass(WeightBehavior.WeightBehaviorKey, typeof(WeightBehavior));

            api.RegisterEntityBehaviorClass(FemaleBreedingBehavior.FemaleBreedingBehaviorKey, typeof(FemaleBreedingBehavior));
            //api.RegisterEntityBehaviorClass(MaleBreedingBehavior.MaleBreedingBehaviorKey, typeof(MaleBreedingBehavior));
            api.RegisterEntityBehaviorClass(LactationBehavior.LactationKey, typeof(LactationBehavior));
        }
    }
}