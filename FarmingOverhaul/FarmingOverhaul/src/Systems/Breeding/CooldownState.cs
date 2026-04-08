namespace FarmingOverhaul.src.Systems.Breeding
{
    public class CooldownState(TreeAccessor treeAccessor)
    {
        private readonly TreeAccessor treeAccessor = treeAccessor;
        private readonly string prefix = nameof(CooldownState);

        public double CooldownEndTotalDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(CooldownEndTotalDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(CooldownEndTotalDays), value);
        }
    }
}