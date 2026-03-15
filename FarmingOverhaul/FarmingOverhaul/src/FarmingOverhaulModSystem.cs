using FarmingOverhaul.src.Behaviors;
using FarmingOverhaul.src.Blocks;
using FarmingOverhaul.src.Items;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Server;

namespace FarmingOverhaul.src
{
    public class FarmingOverhaulModSystem : ModSystem
    {
        private Harmony? harmony;
        
        public override void Start(ICoreAPI api)
        {
            string ModID = Mod.Info.ModID;
            api.RegisterBlockClass(ModID + ".trampoline", typeof(BlockTrampoline));
            api.RegisterItemClass(ModID + ".thornsblade", typeof(ItemThornsBlade));
            api.RegisterEntityBehaviorClass(AttributeKeys.BreedingBehaviorKey, typeof(BreedingBehavior));
            api.RegisterEntityBehaviorClass(AttributeKeys.AnimalStateKey, typeof(AnimalState));
            api.RegisterEntityBehaviorClass(AttributeKeys.BaseBehaviorKey, typeof(BaseBehavior));
            api.RegisterEntityBehaviorClass(AttributeKeys.LactationKey, typeof(LactationBehavior));

            Interfacer.Initialize(api);
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            Interfacer.SystemManager.StartServer(api);

            api.Event.OnEntityLoaded += OnEntityLoadedHandler;

            InitializeHarmony();
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            Interfacer.SystemManager.StartClient(api);
        }

        private void InitializeHarmony()
        {
            if (harmony != null)
            {
                return;
            }

            harmony = new(Mod.Info.ModID);
            harmony.PatchAll();
        }

        private void OnEntityLoadedHandler(Entity entity)
         {
            string species = HelperFunctions.GetSpeciesStringLowerFromEntity(entity);

            //if entity should have the custom breeding behavior but doesn't for some reason, add it.
            if (Interfacer.SystemManager.ConfigManager.ServerConfig.Species.ContainsKey(species))
            {
                if (!entity.HasBehavior(AttributeKeys.BreedingBehaviorKey))
                {
                    entity.Api.Logger.Notification("Didn't have breeding behavior but should: " + species);
                    entity.AddBehavior(new BreedingBehavior(entity));
                }
            }
        }
    }
}