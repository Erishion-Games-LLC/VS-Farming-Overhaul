using FarmingOverhaul.src.Behaviors;
using System;
using System.Globalization;
using Vintagestory.API.Common;
using Vintagestory.API.MathTools;
using static FarmingOverhaul.src.HelperFunctions;
using static FarmingOverhaul.src.Systems.Breeding.BreedingLogic;

namespace FarmingOverhaul.src.Systems.Breeding
{
    public partial class ReproductionStateManager
    {
        //nullable for now
        public IReproductionState? CurrentState { get; private set; }
        public EstrusCycleState EstrusCycle { get; } = new EstrusCycleState();
        public PregnancyState Pregnancy { get; } = new PregnancyState();
        public CooldownState Cooldown { get; } = new CooldownState();
        private readonly Random rand;
        private readonly AnimalState animalState;
        private readonly AnimalConstants constants;

        public bool CompletedFirstEstrus { get; set; }
        private double totalDays;
        private EnumMonth month;

        public event Action<int>? OnBirth;

        public ReproductionStateManager(Random random, AnimalState state, TreeAccessor tree)
        {
            rand = random;
            animalState = state;
            constants = animalState.Constants;
        }

        public void Update(double totalDays, EnumMonth month)
        {
            this.totalDays = totalDays;
            this.month = month;

            IReproductionState returnedStateClass = CurrentState.Update(totalDays, month, animalState);

            if (returnedStateClass != CurrentState)
            {
                CurrentState = returnedStateClass;
            }
        }


        //    switch (CurrentState)
        //    {
        //        case ReproductionState.Idle:     UpdateIdleState();     break;
        //        case ReproductionState.Estrus:   UpdateEstrusState();   break;
        //        case ReproductionState.Pregnant: UpdatePregnantState(); break;
        //        case ReproductionState.Cooldown: UpdateCooldownState(); break;
        //    }
        //}

        private void UpdateIdleState()
        {
            if (IsBreedingSeason(constants.BreedingSeason, month))
            {
                StartEstrusCycle();
                TransitionState(ReproductionState.Estrus);
            }
        }

        private void UpdateEstrusState()
        {
            if (ShouldEndEstrusCycle(totalDays, EstrusCycle.StartTotalDays, EstrusCycle.LengthDays))
            {
                EndEstrusCycle();
                TransitionState(ReproductionState.Idle);
            }
        }

        private void UpdatePregnantState()
        {
            if (ShouldGiveBirth(totalDays, Pregnancy.StartTotalDays, Pregnancy.LengthDays))
            {
                OnBirth?.Invoke(Pregnancy.FetusAmount);
                EndPregnancy();
                TransitionState(ReproductionState.Cooldown);
            }
        }

        private void UpdateCooldownState()
        {

        }

        private void TransitionState(ReproductionState newState)
        {
            CurrentState = newState;
        }

        //DONE
        //Initializes the estrus cycle for the animal, setting the duration and timing parameters for the current cycle.
        protected void StartEstrusCycle()
        {
            EstrusCycle.LengthDays = SampleNormalDistributionInRange(rand, constants.EstrusCycleMinDays, constants.EstrusCycleMaxDays);
            EstrusCycle.TimeBeforeHeatHours = SampleNormalDistributionInRange(rand, constants.EstrusCycleTimeBeforeHeatMinHours, constants.EstrusCycleTimeBeforeHeatMaxHours);
            EstrusCycle.HeatDurationHours = SampleNormalDistributionInRange(rand, constants.EstrusCycleHeatDurationMinHours, constants.EstrusCycleHeatDurationMaxHours);
            EstrusCycle.HoursUntilPeakFertility = SampleNormalDistributionInRange(rand, constants.EstrusCycleTimeBeforePeakFertilityMinHours, constants.EstrusCycleTimeBeforePeakFertilityMaxHours);
            EstrusCycle.PeakFertilityDurationHours = SampleNormalDistributionInRange(rand, constants.EstrusCyclePeakFertilityMinHours, constants.EstrusCyclePeakFertilityMaxHours);

            //If the animal is naturally generated AND it is has not completed its first Estrus, then it could have been generated at any point in the cycle. 
            if (animalState.Origin != "reproduction" && !CompletedFirstEstrus)
            {
                EstrusCycle.StartTotalDays = GenerateRandomDouble(rand, totalDays - EstrusCycle.LengthDays, totalDays);
            }

            //If the animal is not naturally generated, then it starts the cycle at the normal time.
            else
            {
                EstrusCycle.StartTotalDays = totalDays;
            }

            EstrusCycle.PeakStartTotalDays = EstrusCycle.StartTotalDays + (EstrusCycle.HoursUntilPeakFertility / 24);
            EstrusCycle.PeakEndTotalDays = EstrusCycle.PeakStartTotalDays + (EstrusCycle.PeakFertilityDurationHours / 24);
        }

        //DONE
        //End the animals estrus cycle. Set the variables needed during this cycle to -1.
        protected void EndEstrusCycle()
        {
            CompletedFirstEstrus = true;

            EstrusCycleLengthDays = -1;
            EstrusCycleTimeBeforeHeatHours = -1;
            EstrusCycleHeatDurationHours = -1;
            EstrusCycleHoursUntilPeakFertility = -1;
            EstrusCyclePeakFertilityHours = -1;

            EstrusCycleTotalStartDays = -1;
            TotalDaysWhenPeakFertilityStarts = -1;
            TotalDaysWhenPeakFertilityEnds = -1;
        }

        protected void StartPregnancy()
        {
            Pregnancy.FetusAmount = (int)SampleNormalDistributionInRange(rand, constants.MinFetusAmount, constants.MaxFetusAmount);
            Pregnancy.LengthDays = SampleNormalDistributionInRange(rand, constants.MinDaysPregnant, constants.MaxDaysPregnant);
            Pregnancy.StartTotalDays = totalDays;
            TransitionState(ReproductionState.Pregnant);
        }

        //Set up the variables pertaining to the end of pregnancy, and then restart the looping function to try and begin the estrus cycle anew
        protected void EndPregnancy()
        {            
            Pregnancy.LengthDays = -1;
            Pregnancy.StartTotalDays = -1;
            Cooldown.CooldownEndTotalDays = totalDays + SampleNormalDistributionInRange(rand, constants.MinDaysBeforeBreedAgainFemale, constants.MaxDaysBeforeBreedAgainFemale);
            TransitionState(ReproductionState.Cooldown);
        }
    }
}