using FarmingOverhaul.assets.farmingoverhaul.configs;
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

        public override void Start(ICoreAPI api)
        {
        }

        public override void StartServerSide(ICoreServerAPI api)
        {
            serverConfig = LoadConfig<FarmingOverhaulServerConfig>(api);
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
    }
}