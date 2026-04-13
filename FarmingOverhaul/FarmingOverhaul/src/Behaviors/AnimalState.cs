using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace FarmingOverhaul.src.Behaviors
{
    public class AnimalState(Entity entity) : BaseBehavior(entity)
    {
        public const string AnimalStateKey = "foanimalstate";
        public override string PropertyNameKey => AnimalStateKey;
        public override string PropertyName() => PropertyNameKey;
        public override string TreeKey => PropertyNameKey;

        public AnimalConstants Constants { get; private set; }

        public string Species
        {
            get { return TreeAccessor.GetStringFromTree(nameof(Species)); }
            set { TreeAccessor.SetStringInTree(nameof(Species), value); }
        }
        public string Gender
        {
            get { return TreeAccessor.GetStringFromTree(nameof(Gender)); }
            set { TreeAccessor.SetStringInTree(nameof(Gender), value); }
        }
        public string Age
        {
            get { return TreeAccessor.GetStringFromTree(nameof(Age)); }
            set { TreeAccessor.SetStringInTree(nameof(Age), value); }
        }
        public string Type
        {
            get { return TreeAccessor.GetStringFromTree(nameof(Type)); }
            set { TreeAccessor.SetStringInTree(nameof(Type), value); }
        }
        public int Generation
        {
            get { return TreeAccessor.GetIntFromWatchedAttributes(nameof(Generation)); }
            set { TreeAccessor.SetIntInWatchedAttributes(nameof(Generation), value); }
        }
        public bool CompletedFirstEstrus
        {
            get => TreeAccessor.GetBoolFromTree(nameof(CompletedFirstEstrus));
            set => TreeAccessor.SetBoolInTree(nameof(CompletedFirstEstrus), value);
        }

        public string Origin;

        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);

            Species = HelperFunctions.GetSpeciesStringLowerFromEntity(entity);
            Constants = GetAnimalConstants(Species);

            Type = entity.Properties.Variant.TryGetValue(nameof(Type).ToLower());
            Age = entity.Properties.Variant.TryGetValue(nameof(Age).ToLower());
            Gender = entity.Properties.Variant.TryGetValue(nameof(Gender).ToLower());
            Origin = TreeAccessor.GetStringFromWatchedAttributes(nameof(Origin).ToLower());
        }

        private AnimalConstants GetAnimalConstants(string species)
        {
            if (Config.Species.TryGetValue(species, out var constants))
            {
                return constants;
            }
            return new AnimalConstants();
        }
    }
}