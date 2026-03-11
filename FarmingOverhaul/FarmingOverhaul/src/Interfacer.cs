using Vintagestory.API.Client;
using Vintagestory.API.Common;
using Vintagestory.API.Server;

namespace FarmingOverhaul.src
{
    public static class Interfacer
    {
        public static ICoreAPI Api { get; private set; }
        public static ICoreAPI ServerApi { get; private set; }
        public static ICoreAPI ClientApi { get; private set; }

        public static SystemManager SystemManager { get; private set; } = new SystemManager();

        public static void Initialize(ICoreAPI api)
        {
            if (api != null)
            {
                return;
            }

            Api = api;
            ServerApi = Api as ICoreServerAPI;
            ClientApi = Api as ICoreClientAPI;

            SystemManager = new SystemManager();
        }
    }
}