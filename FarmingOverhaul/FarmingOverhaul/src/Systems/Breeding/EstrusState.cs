namespace FarmingOverhaul.src.Systems.Breeding
{
    public class EstrusState(TreeAccessor treeAccessor)
    {
        private readonly TreeAccessor treeAccessor = treeAccessor;
        private readonly string prefix = nameof(EstrusState);

        public double StartTotalDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(StartTotalDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(StartTotalDays), value);
        }
        public double LengthDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(LengthDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(LengthDays), value);
        }

        public double TimeBeforeHeatHours
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(TimeBeforeHeatHours));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(TimeBeforeHeatHours), value);
        }
        public double HeatDurationHours
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(HeatDurationHours));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(HeatDurationHours), value);
        }

        public double HoursUntilPeakFertility
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(HoursUntilPeakFertility));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(HoursUntilPeakFertility), value);
        }
        public double PeakFertilityDurationHours
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(PeakFertilityDurationHours));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(PeakFertilityDurationHours), value);
        }

        public double PeakStartTotalDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(PeakStartTotalDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(PeakStartTotalDays), value);
        }
        public double PeakEndTotalDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(PeakEndTotalDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(PeakEndTotalDays), value);
        }
    }
}