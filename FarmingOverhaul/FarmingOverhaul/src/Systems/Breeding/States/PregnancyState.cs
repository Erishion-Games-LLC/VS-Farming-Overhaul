using FarmingOverhaul.src.Behaviors;
using System;
using Vintagestory.API.Common;
using static FarmingOverhaul.src.HelperFunctions;

namespace FarmingOverhaul.src.Systems.Breeding.States
{
    public class PregnancyState(TreeAccessor treeAccessor, AnimalState animalState, Random random) : IReproState
    {
        private readonly TreeAccessor treeAccessor = treeAccessor;
        private readonly AnimalState animalState = animalState;
        private readonly Random rand = random;

        private readonly AnimalConstants constants = animalState.Constants;
        private readonly string prefix = nameof(PregnancyState);

        public event Action<int>? OnBirth;

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

        public void EnterState(double totalDays)
        {
            FetusAmount = (int)SampleNormalDistributionInRange(rand, constants.MinFetusAmount, constants.MaxFetusAmount);
            LengthDays = SampleNormalDistributionInRange(rand, constants.MinDaysPregnant, constants.MaxDaysPregnant);
            StartTotalDays = totalDays;
        }

        public void ExitState()
        {
        }

        public ReproductionState UpdateState(double totalDays, EnumMonth month)
        {
            //Check if the pregnancy duration has completed, and if so, trigger the birth event with the appropriate number of children and transition to cooldown.
            if (ShouldGiveBirth(totalDays, StartTotalDays, LengthDays))
            {
                var children = FetusAmount;
                OnBirth?.Invoke(children);
                return ReproductionState.Cooldown;
            }
            else return ReproductionState.Pregnant;
        }

        public static bool ShouldGiveBirth(double totalDays, double pregnancyStartTotalDays, double pregnancyLengthDays)
        {
            return HasTimeFinished(totalDays, pregnancyStartTotalDays, pregnancyLengthDays);
        }
    }
}