using FarmingOverhaul.src.Behaviors;
using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Systems.Breeding
{
    public class CooldownState : IReproductionState
    {
        public double CooldownEndTotalDays = -1;

        public ReproductionState ReproductionState => ReproductionState.Cooldown;

        public IReproductionState Update(double totalDays, EnumMonth month, AnimalState animalState)
        {
            if (totalDays >= CooldownEndTotalDays)
            {
                return new CooldownState();
            }
            else return this;
        }
    }
}