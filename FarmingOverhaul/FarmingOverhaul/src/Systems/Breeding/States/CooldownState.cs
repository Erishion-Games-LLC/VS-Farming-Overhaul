using System;
using Vintagestory.API.Common;
using static FarmingOverhaul.src.HelperFunctions;

namespace FarmingOverhaul.src.Systems.Breeding.States
{
    public class CooldownState(TreeAccessor treeAccessor, Random random, double minDaysBeforeBreedAgainFemale, double maxDaysBeforeBreedAgainFemale) : IReproState
    {
        private readonly TreeAccessor treeAccessor = treeAccessor;
        private readonly Random rand = random;
        private readonly double minDaysBeforeBreedAgainFemale = minDaysBeforeBreedAgainFemale;
        private readonly double maxDaysBeforeBreedAgainFemale = maxDaysBeforeBreedAgainFemale;

        private readonly string prefix = nameof(CooldownState);

        public double EndTotalDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(EndTotalDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(EndTotalDays), value);
        }

        public void EnterState(double totalDays)
        {
            EndTotalDays = totalDays + SampleNormalDistributionInRange
                (rand, minDaysBeforeBreedAgainFemale, maxDaysBeforeBreedAgainFemale);
        }

        public void ExitState()
        {
        }

        public ReproductionState UpdateState(double totalDays, EnumMonth month)
        {
            if (ShouldEndCooldown(totalDays, EndTotalDays))
            {
                return ReproductionState.Idle;
            }
            else return ReproductionState.Cooldown;
        }

        public static bool ShouldEndCooldown(double totalDays, double endTotalDays)
        {
            return HasTimeFinished(totalDays, 0, endTotalDays);
        }
    }
}