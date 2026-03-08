using FarmingOverhaul.assets.farmingoverhaul.blocks;
using FarmingOverhaul.assets.farmingoverhaul.configs;
using FarmingOverhaul.assets.farmingoverhaul.items;
using HarmonyLib;
using System;
using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace FarmingOverhaul
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
                    Mod.Logger.Error(fileName + " is null. Loading default values.");
                    config = new T();
                }
                api.StoreModConfig(config, fileName);
            }
            catch (Exception e)
            {
                Mod.Logger.Error(e + ": Could not load " + fileName + ". Loading default values.");
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