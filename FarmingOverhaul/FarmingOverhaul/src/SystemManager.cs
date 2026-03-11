using FarmingOverhaul.src.Config;
using Vintagestory.API.Common;

namespace FarmingOverhaul.src
{
    public class SystemManager
    {
        public ConfigManager ConfigManager {  get; set; } = new ConfigManager();

        public void StartServer(ICoreAPI api)
        {
            ConfigManager.Start(api);
        }

        public void StartClient(ICoreAPI api)
        {
            ConfigManager.Start(api);
        }
    }
}
