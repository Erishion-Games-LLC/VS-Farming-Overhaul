using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Systems.Breeding
{
    public class PregnancyState : IReproductionState
    {
        public double LengthDays = -1;
        public int FetusAmount = -1;
        public double StartTotalDays = -1;

        public ReproductionState ReproductionState => ReproductionState.Pregnant;

        public void Update(double totalDays, EnumMonth month)
        {
            throw new System.NotImplementedException();
        }
    }
}