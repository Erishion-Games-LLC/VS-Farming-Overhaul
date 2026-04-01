namespace FarmingOverhaul.src.Systems.Breeding
{
    public class ReproductionStateManager
    {
        public EstrusCycleState EstrusCycle { get; private set; }

        public ReproductionStateManager()
        {
            EstrusCycle = new EstrusCycleState();
        }

        public class EstrusCycleState
        {
            public double StartTotalDays;
            public double LengthDays;

            public double TimeBeforeHeatHours;
            public double HeatDurationHours;

            public double PeakStartTotalDays;
            public double PeakEndTotalDays;

            public EstrusCycleState() { }
        }

        public class PregnancyState
        {
            public double DaysPregnant;
            public int FetusAmount;
            public double TotalEnergyRequiredForPregnancy;

            public PregnancyState() { }
        }
    }
}