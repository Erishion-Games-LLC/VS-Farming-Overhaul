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

        protected string Gender { get; private set; }
        protected string Age { get; private set; }
        public string Type { get; private set; }
        public AnimalConstants Constants { get; private set; }

        public string Species { get; private set; }


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
            if (config.Species.TryGetValue(species, out var constants))
            {
                return constants;
            }
            return new AnimalConstants();
        }
    }
}
