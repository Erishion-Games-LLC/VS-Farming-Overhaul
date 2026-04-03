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

        public int GetIntFromTree(string key) => tree.GetInt(key);
        public float GetFloatFromTree(string key) => tree.GetFloat(key);
        public double GetDoubleFromTree(string key) => tree.GetDouble(key);
        public long GetLongFromTree(string key) => tree.GetLong(key);
        public bool GetBoolFromTree(string key) => tree.GetBool(key);
        public string GetStringFromTree(string key) => tree.GetString(key);

        public int? TryGetIntFromTree(string key) => tree.TryGetInt(key);
        public float? TryGetFloatFromTree(string key) => tree.TryGetFloat(key);
        public double? TryGetDoubleFromTree(string key) => tree.TryGetDouble(key);
        public long? TryGetLongFromTree(string key) => tree.TryGetLong(key);
        public bool? TryGetBoolFromTree(string key) => tree.TryGetBool(key);

        public void SetIntInTree(string key, int value) { tree.SetInt(key, value); MarkPathDirty(); }
        public void SetFloatInTree(string key, float value) { tree.SetFloat(key, value); MarkPathDirty(); }
        public void SetDoubleInTree(string key, double value) { tree.SetDouble(key, value); MarkPathDirty(); }
        public void SetLongInTree(string key, long value) { tree.SetLong(key, value); MarkPathDirty(); }
        public void SetBoolInTree(string key, bool value) { tree.SetBool(key, value); MarkPathDirty(); }
        public void SetStringInTree(string key, string value) { tree.SetString(key, value); MarkPathDirty(); }
    }

}    