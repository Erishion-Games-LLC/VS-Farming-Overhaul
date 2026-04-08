using FarmingOverhaul.src.Behaviors;
using System;
using Vintagestory;
using Vintagestory.API.Common;
using Vintagestory.Common;
using static FarmingOverhaul.src.HelperFunctions;
using static FarmingOverhaul.src.Systems.Breeding.BreedingLogic;

namespace FarmingOverhaul.src.Systems.Breeding
{
    public class ReproductionStateManager
    {
        public ReproductionState CurrentState
        {
            get => (ReproductionState)treeAccessor.GetIntFromTree(nameof(CurrentState));
            private set => treeAccessor.SetIntInTree(nameof(CurrentState), (int)value);
        }
        private EstrusState Estrus { get; }
        private PregnancyState Pregnancy { get; }
        private CooldownState Cooldown { get; }
        private readonly Random rand;
        private readonly AnimalState animalState;
        private readonly AnimalConstants constants;
        private readonly TreeAccessor treeAccessor;

        private double totalDays;
        private EnumMonth month;

        public event Action<int>? OnBirth;

        public ReproductionStateManager(Random random, AnimalState state, TreeAccessor treeAccessor)
        {
            rand = random;
            animalState = state;
            constants = animalState.Constants;
            this.treeAccessor = treeAccessor;

            Estrus = new EstrusState(treeAccessor);
            Pregnancy = new PregnancyState(treeAccessor);
            Cooldown = new CooldownState(treeAccessor);
        }

        public void Update(double totalDays, EnumMonth month)
        {
            this.totalDays = totalDays;
            this.month = month;

            switch (CurrentState)
            {
                case ReproductionState.Idle:     UpdateIdleState();     break;
                case ReproductionState.Estrus:   UpdateEstrusState();   break;
                case ReproductionState.Pregnant: UpdatePregnantState(); break;
                case ReproductionState.Cooldown: UpdateCooldownState(); break;
            }
        }

        private void UpdateIdleState()
        {
            if (!IsBreedingSeason(constants.BreedingSeason, month))
            {
                return;
            }

            TransitionState(ReproductionState.Estrus);
        }

        private void UpdateEstrusState()
        {
            if (!ShouldEndEstrusCycle(totalDays, Estrus.StartTotalDays, Estrus.LengthDays))
            {
                return;
            }

            TransitionState(ReproductionState.Idle);
        }

        private void UpdatePregnantState()
        {
            if (!ShouldGiveBirth(totalDays, Pregnancy.StartTotalDays, Pregnancy.LengthDays))
            {
                return;
            }

            var children = Pregnancy.FetusAmount;
            TransitionState(ReproductionState.Cooldown);
            OnBirth?.Invoke(children);

        }

        private void UpdateCooldownState()
        {
            if (!ShouldEndCooldown(totalDays, Cooldown.CooldownEndTotalDays))
            {
                return;
            }

            TransitionState(ReproductionState.Idle);
        }
        
        private void TransitionState(ReproductionState newState)
        {
            if (!IsTransitionValid(CurrentState, newState))
            {                
                return;
            }

            OnExitState(CurrentState);
            CurrentState = newState;
            OnEnterState(newState);
        }

        private void OnEnterState(ReproductionState state)
        {
            switch (state)
            {
                case ReproductionState.Idle:
                    break;
                case ReproductionState.Estrus:
                    StartEstrusCycle();
                    break;
                case ReproductionState.Pregnant:
                    StartPregnancy();
                    break;
                case ReproductionState.Cooldown:
                    Cooldown.CooldownEndTotalDays = totalDays + SampleNormalDistributionInRange
                        (rand, constants.MinDaysBeforeBreedAgainFemale, constants.MaxDaysBeforeBreedAgainFemale);
                    break;
            }
        }

        private void OnExitState(ReproductionState state)
        {
            switch (state)
            {
                case ReproductionState.Idle:
                    break;
                case ReproductionState.Estrus:
                    if (!animalState.CompletedFirstEstrus)
                    {
                        animalState.CompletedFirstEstrus = true;
                    }
                    break;
                case ReproductionState.Pregnant:
                    break;
                case ReproductionState.Cooldown:
                    break;
            }
        }

        private static bool IsTransitionValid(ReproductionState oldState, ReproductionState newState)
        {
            if (oldState == newState) 
            {
                //Need to print a message using logger, but I don't have access to it in this class
                return false; 
            }

            return (oldState, newState) switch
            {
                (ReproductionState.Idle, ReproductionState.Estrus) => true,
                (ReproductionState.Estrus, ReproductionState.Idle) => true,
                (ReproductionState.Estrus, ReproductionState.Pregnant) => true,
                (ReproductionState.Pregnant, ReproductionState.Cooldown) => true,
                (ReproductionState.Cooldown, ReproductionState.Idle) => true,
                //Need to print a message using logger, but I don't have access to it in this class
                _ => false
            };
        }

        //DONE
        //Initializes the estrus cycle for the animal, setting the duration and timing parameters for the current cycle.
        private void StartEstrusCycle()
        {
            Estrus.LengthDays = SampleNormalDistributionInRange(rand, constants.EstrusCycleMinDays, constants.EstrusCycleMaxDays);
            Estrus.TimeBeforeHeatHours = SampleNormalDistributionInRange(rand, constants.EstrusCycleTimeBeforeHeatMinHours, constants.EstrusCycleTimeBeforeHeatMaxHours);
            Estrus.HeatDurationHours = SampleNormalDistributionInRange(rand, constants.EstrusCycleHeatDurationMinHours, constants.EstrusCycleHeatDurationMaxHours);
            Estrus.HoursUntilPeakFertility = SampleNormalDistributionInRange(rand, constants.EstrusCycleTimeBeforePeakFertilityMinHours, constants.EstrusCycleTimeBeforePeakFertilityMaxHours);
            Estrus.PeakFertilityDurationHours = SampleNormalDistributionInRange(rand, constants.EstrusCyclePeakFertilityMinHours, constants.EstrusCyclePeakFertilityMaxHours);

            //If the animal is naturally generated AND it is has not completed its first Estrus, then it could have been generated at any point in the cycle. 
            if (animalState.Origin != "reproduction" && !animalState.CompletedFirstEstrus)
            {
                Estrus.StartTotalDays = GenerateRandomDouble(rand, totalDays - Estrus.LengthDays, totalDays);
            }

            //If the animal is not naturally generated, then it starts the cycle at the normal time.
            else
            {
                Estrus.StartTotalDays = totalDays;
            }

            Estrus.PeakStartTotalDays = Estrus.StartTotalDays + (Estrus.HoursUntilPeakFertility / 24);
            Estrus.PeakEndTotalDays = Estrus.PeakStartTotalDays + (Estrus.PeakFertilityDurationHours / 24);
        }

        //Needs to be called from the male animal when breeding occurs
        public void TryStartPregnancy()
        {
            if (CurrentState != ReproductionState.Estrus) { return; }
            if (!ShouldGetPregnant(rand, constants.ImpregnationFailChance, Estrus.PeakStartTotalDays, Estrus.PeakEndTotalDays, totalDays)) { return; }

            TransitionState(ReproductionState.Pregnant);
        }

        private void StartPregnancy()
        {
            Pregnancy.FetusAmount = (int)SampleNormalDistributionInRange(rand, constants.MinFetusAmount, constants.MaxFetusAmount);
            Pregnancy.LengthDays = SampleNormalDistributionInRange(rand, constants.MinDaysPregnant, constants.MaxDaysPregnant);
            Pregnancy.StartTotalDays = totalDays;
        }
    }
}