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
        protected abstract string PropertyNameKey { get; }
        protected ITreeAttribute Tree;
        public override string PropertyName() => PropertyNameKey;
        protected virtual string TreeKey => PropertyNameKey;

        protected TreeAccessor TreeAccessor;

        protected EntityProperties Properties { get; private set; }
        protected JsonObject Attributes { get; private set; }
        protected FarmingOverhaulServerConfig Config;
        protected ICoreAPI Api;
        protected ILogger Logger;
        protected IWorldAccessor World;
        protected IGameCalendar Calendar;
        protected Random Rand;
        protected EnumAppSide ApiSide;

        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);           
            Api = entity.Api;
            Logger = Api.Logger;
            World = Api.World;
            Calendar = World.Calendar;
            Rand = World.Rand;
            ApiSide = Api.Side;
            Config = ConfigManager.ServerConfig;


            Tree = entity.WatchedAttributes.GetTreeAttribute(TreeKey) ?? new TreeAttribute();
            TreeAccessor = new(Tree, entity, TreeKey);

            if (ApiSide == EnumAppSide.Server) 
            { 
                entity.WatchedAttributes.SetAttribute(TreeKey, Tree);
                EnableTickListeners();
            }
        }

        public virtual void EnableTickListeners()
        {
        }

        public virtual void DisableTickListeners()
        {
        }

        public virtual void SetRequiredBehaviors()
        {
        }

        public override void OnEntityDeath(DamageSource damageSourceForDeath)
        {
            base.OnEntityDeath(damageSourceForDeath);
            DieOrDespawn();
        }
        public override void OnEntityDespawn(EntityDespawnData despawn)
        {
            base.OnEntityDespawn(despawn);
            DieOrDespawn();
        }

        protected void DieOrDespawn()
        {
            DisableTickListeners();
        }
    }
}