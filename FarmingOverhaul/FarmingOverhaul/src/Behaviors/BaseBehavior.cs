using FarmingOverhaul.src.Config;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace FarmingOverhaul.src.Behaviors
{
    public abstract class BaseBehavior(Entity entity) : EntityBehavior(entity)
    {
        public abstract string PropertyNameKey { get; }
        protected ITreeAttribute Tree;
        public abstract string TreeKey { get; }

        public FarmingOverhaulServerConfig config;
        public ICoreAPI api;
        public ILogger logger;
        public IWorldAccessor world;

        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);
            api = entity.Api;
            logger = api.Logger;
            world = api.World;
            
            Tree = entity.WatchedAttributes.GetTreeAttribute(TreeKey);

            Tree ??= new TreeAttribute();
            if (entity.World.Side == EnumAppSide.Server)
            {
                entity.WatchedAttributes.SetAttribute(TreeKey, Tree);
            }

            config = Interfacer.SystemManager.ConfigManager.ServerConfig;
            if (Interfacer.SystemManager.ConfigManager.ServerConfig == null)
            {
                logger.Error("FarmingOverhaulServerConfig is null. Exiting Initialize on: " + entity.GetName());
                return;
            }

        }

        protected void MarkPathDirty() => entity.WatchedAttributes.MarkPathDirty(TreeKey);

        protected int GetIntFromTree(string key) =>       Tree.GetInt(key);
        protected float GetFloatFromTree(string key) =>   Tree.GetFloat(key);
        protected double GetDoubleFromTree(string key) => Tree.GetDouble(key);
        protected long GetLongFromTree(string key) =>     Tree.GetLong(key);
        protected bool GetBoolFromTree(string key) =>     Tree.GetBool(key);
        protected string GetStringFromTree(string key) => Tree.GetString(key);

        protected int? TryGetIntFromTree(string key) =>       Tree.TryGetInt(key);
        protected float? TryGetFloatFromTree(string key) =>   Tree.TryGetFloat(key);
        protected double? TryGetDoubleFromTree(string key) => Tree.TryGetDouble(key);
        protected long? TryGetLongFromTree(string key) =>     Tree.TryGetLong(key);
        protected bool? TryGetBoolFromTree(string key) =>     Tree.TryGetBool(key);

        protected void SetIntInTree(string key, int value)       { Tree.SetInt(key, value); MarkPathDirty(); }
        protected void SetFloatInTree(string key, float value)   { Tree.SetFloat(key, value); MarkPathDirty(); }
        protected void SetDoubleInTree(string key, double value) { Tree.SetDouble(key, value); MarkPathDirty(); }
        protected void SetLongInTree(string key, long value)     { Tree.SetLong(key, value); MarkPathDirty(); }
        protected void SetBoolInTree(string key, bool value)     { Tree.SetBool(key, value); MarkPathDirty(); }
        protected void SetStringInTree(string key, string value) { Tree.SetString(key, value); MarkPathDirty(); }
    }
}
