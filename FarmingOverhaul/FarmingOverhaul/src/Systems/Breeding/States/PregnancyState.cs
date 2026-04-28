using FarmingOverhaul.src.Constants.AnimalConstants;
using FarmingOverhaul.src.Helpers.StateMachine;
using FarmingOverhaul.src.Systems.Breeding.Enums;
using System;
using Vintagestory.API.Common;
using static FarmingOverhaul.src.HelperFunctions;

namespace FarmingOverhaul.src.Systems.Breeding.States
{
    public class PregnancyState(TreeAccessor treeAccessor, PregnancyConstants pregnancy, Random random, int daysPerMonth) : IState<FemaleReproductionState>
    {
        private readonly TreeAccessor treeAccessor = treeAccessor;
        private readonly Random rand = random;

        private readonly string prefix = nameof(PregnancyState);

        public event Action<int, double>? OnBirth;

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

        public FemaleReproductionState State => FemaleReproductionState.Pregnant;

        public void EnterState(double transitionDays, EnumMonth transitionMonth)
        {
            FetusAmount = (int)SampleNormalDistributionInRange(rand, pregnancy.MinFetusAmount, pregnancy.MaxFetusAmount);
            LengthDays = SampleNormalDistributionInRange(rand, pregnancy.MinDaysPregnant, pregnancy.MaxDaysPregnant);
            StartTotalDays = transitionDays;
        }

        public void ExitState()
        {
        }

        public (FemaleReproductionState nextState, double transitionDays, EnumMonth transitionMonth) UpdateState(double totalDays, EnumMonth month)
        {
            //Check if the pregnancy duration has completed, and if so, trigger the birth event with the appropriate number of children and transition to cooldown.
            if (HasTimeFinished(totalDays, StartTotalDays, LengthDays))
            {
                var children = FetusAmount;
                OnBirth?.Invoke(children, totalDays);
                double transitionDays = StartTotalDays + LengthDays;

                return (FemaleReproductionState.Cooldown, transitionDays, GetMonthFromDay(transitionDays, daysPerMonth));
            }
            else return (FemaleReproductionState.Pregnant, totalDays, month);
        }
    }
}