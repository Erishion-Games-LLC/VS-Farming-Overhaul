using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using static FarmingOverhaul.src.Config.FarmingOverhaulServerConfig;

namespace FarmingOverhaul.src.Behaviors
{
    public class AnimalState(Entity entity) : BaseBehavior(entity)
    {
        public override string PropertyNameKey => AttributeKeys.AnimalStateKey;
        public override string TreeKey => PropertyNameKey;

        protected string Gender { get; private set; }
        protected string Age { get; private set; }
        public string Type { get; private set; }

        public string Species { get; private set; }
        public AnimalConstants constants { get; private set; }


        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);

            Species = HelperFunctions.GetSpeciesStringLowerFromEntity(entity);
            constants = GetAnimalConstants(Species);

            Type = entity.Properties.Variant.TryGetValue(AttributeKeys.TypeKey);
            Age = entity.Properties.Variant.TryGetValue(AttributeKeys.AgeKey);
            Gender = entity.Properties.Variant.TryGetValue(AttributeKeys.GenderKey);

        }

        public override string PropertyName()
        {
            return PropertyNameKey;
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
