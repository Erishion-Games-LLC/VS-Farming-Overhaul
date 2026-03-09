using FarmingOverhaul.src.Blocks;
using FarmingOverhaul.src.Config;
using FarmingOverhaul.src.Items;
using HarmonyLib;
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace FarmingOverhaul.src
{
    public class FarmingOverhaulModSystem : ModSystem
    {
        private static FarmingOverhaulServerConfig? serverConfig;
        private static FarmingOverhaulClientConfig? clientConfig;
        private Harmony? harmony;

        public override void Start(ICoreAPI api)
        {
            api.RegisterBlockClass(Mod.Info.ModID + ".trampoline", typeof(BlockTrampoline));
            api.RegisterItemClass(Mod.Info.ModID + ".thornsblade", typeof(ItemThornsBlade));
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            serverConfig = LoadConfig<FarmingOverhaulServerConfig>(api);
            InitializeHarmony();
        }

        public override void StartClientSide(ICoreClientAPI api)
        {
            clientConfig = LoadConfig<FarmingOverhaulClientConfig>(api);
        }

        private T LoadConfig<T>(ICoreAPI api) where T : new()
        {
            T config;
            string fileName = typeof(T).Name + ".json";

            try
            {
                config = api.LoadModConfig<T>(fileName);
                if (config == null)
                {
                    Mod.Logger.Warning(fileName + " is null. Loading default values.");
                    config = new T();
                }
                api.StoreModConfig(config, fileName);
            }
            catch (Exception e)
            {
                Mod.Logger.Warning(e + ": Could not load " + fileName + ". Loading default values.");
                config = new T();
            }
            return config;
        }

        private void InitializeHarmony()
        {
            harmony = new(Mod.Info.ModID);
            harmony.PatchAll();
        }
    }
}