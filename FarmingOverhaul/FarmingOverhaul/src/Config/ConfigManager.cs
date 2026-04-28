using System;
using System.Collections.Generic;
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
            T validatedConfig = ValidateConfig(rawConfig, api, fileName);
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

        private static T ValidateConfig<T>(T? config, ICoreAPI api, string fileName)
            where T : class, IValidatableConfig ,new()
        {
            if (config == null)
            {
                api.Logger.Error($"{fileName} is null. Loading default values.");
                config = new T();
                return config;
            }

            List<string> errors = config.Validate();

            if (errors.Count > 0)
            {
                api.Logger.Error($"Config {fileName} had errors: ");
                foreach (string error in errors)
                {
                    api.Logger.Error(error);
                }

                api.Logger.Error($"Replacing invalid {fileName} with default values.");

                config = new T();
            }

            return config;
        }
    }
}