using System;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Config
{
    public class ConfigManager
    {
        public FarmingOverhaulServerConfig? ServerConfig { get; private set; }
        public FarmingOverhaulClientConfig? ClientConfig { get; private set; }

        public void Start(ICoreAPI api)
        {
            if (api.Side == EnumAppSide.Server)
            {
                ServerConfig = LoadConfig<FarmingOverhaulServerConfig>(api);
                ServerConfig.Validate();
            }
            else if (api.Side == EnumAppSide.Client)
            {
                ClientConfig = LoadConfig<FarmingOverhaulClientConfig>(api);
            }
            else
            {
                api.Logger.Error("Farming Overhaul Config Manager started on api side other than client or server");
            }
        }
        
        private static T LoadConfig<T>(ICoreAPI api) where T : class, new()
        {
            string fileName = typeof(T).Name + ".json";
            List<string> errors = [];
            try
            {
                T config = api.LoadModConfig<T>(fileName);

                if (config == null)
                {
                    api.Logger.Warning(fileName + " is null. Loading default values.");
                    config = new T();
                }

                if (config is FarmingOverhaulServerConfig serverConfig)
                {
                    errors = serverConfig.Validate();

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
                }

                api.StoreModConfig(config, fileName);
                return config;
            }
            catch (Exception e)
            {
                api.Logger.Warning(e + ": Could not load " + fileName + ". Loading default values.");
                return new T();
            }
        }
    }
}