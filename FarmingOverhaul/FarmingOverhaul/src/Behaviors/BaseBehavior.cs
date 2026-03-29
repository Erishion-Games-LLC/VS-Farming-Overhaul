using FarmingOverhaul.src.Config;
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

        protected void MarkPathDirty() => entity.WatchedAttributes.MarkPathDirty(TreeKey);

        protected int GetIntFromWatchedAttributes(string key) => entity.WatchedAttributes.GetInt(key);
        protected double GetDoubleFromWatchedAttributes(string key) => entity.WatchedAttributes.GetDouble(key);

        protected void SetIntInWatchedAttributes(string key, int value) => entity.WatchedAttributes.SetInt(key, value);
        protected void SetDoubleInWatchedAttributes(string key, int value) => entity.WatchedAttributes.SetDouble(key, value);

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
