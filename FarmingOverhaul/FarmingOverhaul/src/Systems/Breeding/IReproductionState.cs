using FarmingOverhaul.src.Behaviors;
using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Systems.Breeding
{
    public interface IReproductionState
    {
        public ReproductionState ReproductionState { get; }

        public IReproductionState Update(double totalDays, EnumMonth month, AnimalState animalState);
    }
}