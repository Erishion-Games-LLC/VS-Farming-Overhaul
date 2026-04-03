using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Systems.Breeding
{
    public class EstrusCycleState : IReproductionState
    {
        public double StartTotalDays = -1;
        public double LengthDays = -1;

        public double TimeBeforeHeatHours = -1;
        public double HeatDurationHours = -1;

        public double HoursUntilPeakFertility = -1;
        public double PeakFertilityDurationHours = -1;

        public double PeakStartTotalDays = -1;
        public double PeakEndTotalDays = -1;

        public ReproductionState ReproductionState => ReproductionState.Estrus;

        public void Update(double totalDays, EnumMonth month)
        {
            throw new System.NotImplementedException();
        }
    }
}
