namespace FarmingOverhaul.src.Systems.Breeding
{
    public class PregnancyState(TreeAccessor treeAccessor)
    {
        private readonly TreeAccessor treeAccessor = treeAccessor;
        private readonly string prefix = nameof(PregnancyState);

        public double LengthDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(LengthDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(LengthDays), value);
        }
        public int FetusAmount
        {
            get => treeAccessor.GetIntFromTree(prefix + nameof(FetusAmount));
            set => treeAccessor.SetIntInTree(prefix + nameof(FetusAmount), value);
        }
        public double StartTotalDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(StartTotalDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(StartTotalDays), value);
        }
    }
}