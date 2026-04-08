using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace FarmingOverhaul.src.Systems
{
    public class TreeAccessor
    {
        private readonly ITreeAttribute tree;
        private readonly Entity entity;
        private readonly string treeKey;

        public TreeAccessor(ITreeAttribute tree, Entity entity, string treeKey)
        {
            this.tree = tree;
            this.entity = entity;
            this.treeKey = treeKey;
        }

        protected void MarkPathDirty() => entity.WatchedAttributes.MarkPathDirty(treeKey);

        public int GetIntFromWatchedAttributes(string key) => entity.WatchedAttributes.GetInt(key);
        public double GetDoubleFromWatchedAttributes(string key) => entity.WatchedAttributes.GetDouble(key);

        public void SetIntInWatchedAttributes(string key, int value) => entity.WatchedAttributes.SetInt(key, value);
        public void SetDoubleInWatchedAttributes(string key, int value) => entity.WatchedAttributes.SetDouble(key, value);

        public int GetIntFromTree(string key) => tree.GetInt(key.ToLower());
        public float GetFloatFromTree(string key) => tree.GetFloat(key.ToLower());
        public double GetDoubleFromTree(string key) => tree.GetDouble(key.ToLower());
        public long GetLongFromTree(string key) => tree.GetLong(key.ToLower());
        public bool GetBoolFromTree(string key) => tree.GetBool(key.ToLower());
        public string GetStringFromTree(string key) => tree.GetString(key.ToLower());

        public int? TryGetIntFromTree(string key) => tree.TryGetInt(key.ToLower());
        public float? TryGetFloatFromTree(string key) => tree.TryGetFloat(key.ToLower());
        public double? TryGetDoubleFromTree(string key) => tree.TryGetDouble(key.ToLower());
        public long? TryGetLongFromTree(string key) => tree.TryGetLong(key.ToLower());
        public bool? TryGetBoolFromTree(string key) => tree.TryGetBool(key.ToLower());

        public void SetIntInTree(string key, int value) { tree.SetInt(key.ToLower(), value); MarkPathDirty(); }
        public void SetFloatInTree(string key, float value) { tree.SetFloat(key.ToLower(), value); MarkPathDirty(); }
        public void SetDoubleInTree(string key, double value) { tree.SetDouble(key.ToLower(), value); MarkPathDirty(); }
        public void SetLongInTree(string key, long value) { tree.SetLong(key.ToLower(), value); MarkPathDirty(); }
        public void SetBoolInTree(string key, bool value) { tree.SetBool(key.ToLower(), value); MarkPathDirty(); }
        public void SetStringInTree(string key, string value) { tree.SetString(key.ToLower(), value); MarkPathDirty(); }
    }
}    