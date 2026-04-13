using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Systems.Breeding
{
    public interface IReproState
    {
        ReproductionState UpdateState(double totalDays, EnumMonth month);
        void EnterState(double totalDays);
        void ExitState();
    }
}