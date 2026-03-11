using FarmingOverhaul.src.Blocks;
using FarmingOverhaul.src.Items;
using HarmonyLib;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace FarmingOverhaul.src
{
    public class FarmingOverhaulModSystem : ModSystem
    {
        private Harmony? harmony;

        public override void Start(ICoreAPI api)
        {
            api.RegisterBlockClass(Mod.Info.ModID + ".trampoline", typeof(BlockTrampoline));
            api.RegisterItemClass(Mod.Info.ModID + ".thornsblade", typeof(ItemThornsBlade));
            Interfacer.Initialize(api);
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            Interfacer.SystemManager.StartServer(api);
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
    }
}