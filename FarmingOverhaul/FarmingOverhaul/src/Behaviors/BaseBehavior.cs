using FarmingOverhaul.src.Config;
using FarmingOverhaul.src.Systems;
using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace FarmingOverhaul.src.Behaviors
{
    public abstract class BaseBehavior(Entity entity) : EntityBehavior(entity)
    {
        public const string BaseBehaviorKey = "fobasebehavior";
        public abstract string PropertyNameKey { get; }
        protected ITreeAttribute Tree;
        public abstract string TreeKey { get; }

        protected TreeAccessor TreeAccessor => new(Tree, entity, TreeKey);

        public EntityProperties Properties { get; private set; }
        public JsonObject Attributes { get; private set; }
        public FarmingOverhaulServerConfig Config;
        public ICoreAPI Api;
        public ILogger Logger;
        public IWorldAccessor World;
        public IGameCalendar Calendar;
        public Random Rand;
        public EnumAppSide ApiSide;

        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);           
            Api = entity.Api;
            Logger = Api.Logger;
            World = Api.World;
            Calendar = World.Calendar;
            Rand = World.Rand;
            ApiSide = Api.Side;
           
            Tree = entity.WatchedAttributes.GetTreeAttribute(TreeKey);

            Tree ??= new TreeAttribute();
            if (ApiSide == EnumAppSide.Server) { entity.WatchedAttributes.SetAttribute(TreeKey, Tree); }

            if (Interfacer.SystemManager.ConfigManager.ServerConfig == null)
            {
                Logger.Error("FarmingOverhaulServerConfig is null. Exiting Initialize on: " + entity.GetName());
                return;
            }

            Config = Interfacer.SystemManager.ConfigManager.ServerConfig;
        }

        public virtual void EnableTickListeners()
        {
        }

        public virtual void DisableTickListeners()
        {
        }
    }
}