using FarmingOverhaul.src.Helpers.StateMachine;
using FarmingOverhaul.src.Systems.Breeding.Enums;
using System;
using Vintagestory.API.Common;
using static FarmingOverhaul.src.HelperFunctions;

namespace FarmingOverhaul.src.Systems.Breeding.States
{
    public class CooldownState(TreeAccessor treeAccessor, Random random, double minDaysBeforeBreedAgainFemale, double maxDaysBeforeBreedAgainFemale, int daysPerMonth) : IState<FemaleReproductionState>  
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

        public FemaleReproductionState State => FemaleReproductionState.Cooldown;

        public void EnterState(double transitionDays, EnumMonth transitionMonth)
        {
            EndTotalDays = transitionDays + SampleNormalDistributionInRange
                (rand, minDaysBeforeBreedAgainFemale, maxDaysBeforeBreedAgainFemale);
        }

        public void ExitState()
        {
        }

        public (FemaleReproductionState nextState, double transitionDays, EnumMonth transitionMonth) UpdateState(double totalDays, EnumMonth month)
        {
            //If cooldown has ended, transition to Idle and output the day that cooldown ended.
            if (ShouldEndCooldown(totalDays, EndTotalDays))
            {
                return (FemaleReproductionState.Idle, EndTotalDays, GetMonthFromDay(EndTotalDays, daysPerMonth));
            }
            else return (FemaleReproductionState.Cooldown, totalDays, month);
        }

        public static bool ShouldEndCooldown(double totalDays, double endTotalDays)
        {
            return totalDays >= endTotalDays;
        }
    }
}