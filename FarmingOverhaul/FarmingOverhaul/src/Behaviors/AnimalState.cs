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
            get { return GetStringFromTree(nameof(Species).ToLower()); }
            set { SetStringInTree(nameof(Species).ToLower(), value); }
        }
        public string Gender
        {
            get { return GetStringFromTree(nameof(Gender).ToLower()); }
            set { SetStringInTree(nameof(Gender).ToLower(), value); }
        }
        public string Age
        {
            get { return GetStringFromTree(nameof(Age).ToLower()); }
            set { SetStringInTree(nameof(Age).ToLower(), value); }
        }
        public string Type
        {
            get { return GetStringFromTree(nameof(Type).ToLower()); }
            set { SetStringInTree(nameof(Type).ToLower(), value); }
        }

        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);

            Species = HelperFunctions.GetSpeciesStringLowerFromEntity(entity);
            Constants = GetAnimalConstants(Species);

            Type = entity.Properties.Variant.TryGetValue(nameof(Type).ToLower());
            Age = entity.Properties.Variant.TryGetValue(nameof(Age).ToLower());
            Gender = entity.Properties.Variant.TryGetValue(nameof(Gender).ToLower());
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
