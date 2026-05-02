using System;
using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Config
{
    public static class ConfigManager
    {
        public static FarmingOverhaulServerConfig ServerConfig { get; private set; } = null!;
        public static FarmingOverhaulClientConfig ClientConfig { get; private set; } = null!;


        public static void StartServer(ICoreAPI api)
        {
            ServerConfig = LoadConfig<FarmingOverhaulServerConfig>(api);
        }

        public static void StartClient(ICoreAPI api)
        {
            ClientConfig = LoadConfig<FarmingOverhaulClientConfig>(api);
        }

        private static T LoadConfig<T>(ICoreAPI api)
            where T : class, IValidatableConfig, new()
        {
            string fileName = typeof(T).Name + ".json";

            T? rawConfig = LoadRawConfig<T>(api, fileName);
            T validatedConfig = ConfigValidator.ValidateConfig(rawConfig, api.Logger, fileName);
            api.StoreModConfig(validatedConfig, fileName);

            return validatedConfig;
        }

        private static T? LoadRawConfig<T>(ICoreAPI api, string fileName) where T : class
        {
            try
            {
                T config = api.LoadModConfig<T>(fileName);
                return config;
            }
            catch (Exception e)
            {
                api.Logger.Warning($"Could not load {fileName} due to {e}.");
                return null;
            }
        }
    }
}