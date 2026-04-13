using FarmingOverhaul.src.Behaviors;
using System;
using Vintagestory.API.Common;
using static FarmingOverhaul.src.Systems.Breeding.BreedingLogic;
using static FarmingOverhaul.src.HelperFunctions;

namespace FarmingOverhaul.src.Systems.Breeding.States
{
    public class EstrusState(TreeAccessor treeAccessor, AnimalState animalState, Random random) : IReproState
    {
        private readonly TreeAccessor treeAccessor = treeAccessor;
        private readonly AnimalState animalState = animalState;
        private readonly Random rand = random;

        private readonly AnimalConstants constants = animalState.Constants;
        private readonly string prefix = nameof(EstrusState);

        private bool breedingAttempt = false;

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

        public void EnterState(double totalDays)
        {
            LengthDays = SampleNormalDistributionInRange(rand, constants.EstrusCycleMinDays, constants.EstrusCycleMaxDays);
            TimeBeforeHeatHours = SampleNormalDistributionInRange(rand, constants.EstrusCycleTimeBeforeHeatMinHours, constants.EstrusCycleTimeBeforeHeatMaxHours);
            HeatDurationHours = SampleNormalDistributionInRange(rand, constants.EstrusCycleHeatDurationMinHours, constants.EstrusCycleHeatDurationMaxHours);
            HoursUntilPeakFertility = SampleNormalDistributionInRange(rand, constants.EstrusCycleTimeBeforePeakFertilityMinHours, constants.EstrusCycleTimeBeforePeakFertilityMaxHours);
            PeakFertilityDurationHours = SampleNormalDistributionInRange(rand, constants.EstrusCyclePeakFertilityMinHours, constants.EstrusCyclePeakFertilityMaxHours);

            //If the animal is naturally generated AND it is has not completed its first Estrus, then it could have been generated at any point in the cycle. 
            if (animalState.Origin != "reproduction" && !animalState.CompletedFirstEstrus)
            {
                StartTotalDays = GenerateRandomDouble(rand, totalDays - LengthDays, totalDays);
            }

            //If the animal is not naturally generated, then it starts the cycle at the normal time.
            else
            {
                StartTotalDays = totalDays;
            }

            PeakStartTotalDays = StartTotalDays + (HoursUntilPeakFertility / 24);
            PeakEndTotalDays = PeakStartTotalDays + (PeakFertilityDurationHours / 24);
        }

        public void ExitState()
        {
            if (!animalState.CompletedFirstEstrus)
            {
                animalState.CompletedFirstEstrus = true;
            }
        }

        public ReproductionState UpdateState(double totalDays, EnumMonth month)
        { 
            //Check if a breeding attempt has been made, and if so, determine if it results in pregnancy and transition states accordingly.
            if (breedingAttempt)
            {
                breedingAttempt = false;
                if (ShouldGetPregnant(rand, constants.ImpregnationFailChance, PeakStartTotalDays, PeakEndTotalDays, totalDays))
                return ReproductionState.Pregnant; 

                else return ReproductionState.Estrus;
            }

            //Check if the estrus cycle has completed without impregnation, and transition back to idle if so.
            if (ShouldEndEstrusCycle(totalDays, StartTotalDays, LengthDays))
            {
                return ReproductionState.Idle;
            }

            return ReproductionState.Estrus;
        }

        //Needs to be called from the male animal when breeding occurs
        public void OnBreedingAttempt()
        {
            breedingAttempt = true;
        }

        public static bool ShouldEndEstrusCycle(double totalDays, double cycleTotalStartDays, double cycleLengthDays)
        {
            return HasTimeFinished(totalDays, cycleTotalStartDays, cycleLengthDays);
        }
    }
}