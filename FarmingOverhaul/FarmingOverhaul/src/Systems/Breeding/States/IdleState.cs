using FarmingOverhaul.src.Behaviors;
using Vintagestory.API.Common;
using Vintagestory.API.Util;

namespace FarmingOverhaul.src.Systems.Breeding.States
{
    public class IdleState(AnimalState animalState) : IReproState
    {
        private readonly AnimalState animalState = animalState;
        private readonly AnimalConstants constants = animalState.Constants;

        public void EnterState(double totalDays)
        {
        }

        public void ExitState()
        {
        }

        public ReproductionState UpdateState(double totalDays, EnumMonth month)
        {
            if (IsBreedingSeason(constants.BreedingSeason, month))
            {
                return ReproductionState.Estrus;
            }
            else return ReproductionState.Idle;
        }

        //TODO Eventually check based on species specific needs, like shorter daylight hours for sheep.
        public static bool IsBreedingSeason(EnumMonth[] breedingSeason, EnumMonth currentMonth)
        {
            if (breedingSeason == null) { return false; }
            if (breedingSeason.Contains(currentMonth)) return true;
            else return false;
        }
    }
}
