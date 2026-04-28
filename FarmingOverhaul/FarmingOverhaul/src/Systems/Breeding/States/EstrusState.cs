using FarmingOverhaul.src.Behaviors;
using System;
using Vintagestory.API.Common;
using static FarmingOverhaul.src.HelperFunctions;
using FarmingOverhaul.src.Constants.AnimalConstants;
using FarmingOverhaul.src.Systems.Breeding.Enums;
using FarmingOverhaul.src.Helpers.StateMachine;

namespace FarmingOverhaul.src.Systems.Breeding.States
{
    public class EstrusState(TreeAccessor treeAccessor, AnimalState animalState, Random random) : IState<FemaleReproductionState>
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

        public FemaleReproductionState State => FemaleReproductionState.Estrus;

        public void EnterState(double transitionDays, EnumMonth transitionMonth)
        {
            LengthDays = SampleNormalDistributionInRange(rand, constants.Estrus.EstrusCycleMinDays, constants.Estrus.EstrusCycleMaxDays);
            TimeBeforeHeatHours = SampleNormalDistributionInRange(rand, constants.Estrus.TimeBeforeHeatMinHours, constants.Estrus.TimeBeforeHeatMaxHours);
            HeatDurationHours = SampleNormalDistributionInRange(rand, constants.Estrus.HeatDurationMinHours, constants.Estrus.HeatDurationMaxHours);
            HoursUntilPeakFertility = SampleNormalDistributionInRange(rand, constants.Estrus.TimeBeforePeakFertilityMinHours, constants.Estrus.TimeBeforePeakFertilityMaxHours);
            PeakFertilityDurationHours = SampleNormalDistributionInRange(rand, constants.Estrus.PeakFertilityMinHours, constants.Estrus.PeakFertilityMaxHours);

            //If the animal is naturally generated AND it is has not completed its first Estrus, then it could have been generated at any point in the cycle. 
            if (animalState.Origin != "reproduction" && !animalState.CompletedFirstEstrus)
            {
                StartTotalDays = GenerateRandomDouble(rand, transitionDays - LengthDays, transitionDays);
            }

            //If the animal is not naturally generated, then it starts the cycle at the normal time.
            else
            {
                StartTotalDays = transitionDays;
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

        public (FemaleReproductionState nextState, double transitionDays, EnumMonth transitionMonth) UpdateState(double totalDays, EnumMonth month)
        {
            //Check if a breeding attempt has been made, and if so, determine if it results in pregnancy and transition states accordingly.
            if (breedingAttempt)
            {
                breedingAttempt = false;
                if (ShouldGetPregnant(rand, constants.Breeding.ImpregnationFailChance, PeakStartTotalDays, PeakEndTotalDays, totalDays))
                    return (FemaleReproductionState.Pregnant, totalDays, month);

                else return (FemaleReproductionState.Estrus, totalDays, month);
            }

            //Check if the estrus cycle has completed without impregnation, and transition back to idle if so.
            if (ShouldEndEstrusCycle(totalDays, StartTotalDays, LengthDays))
            {
                return (FemaleReproductionState.Idle, totalDays, month);
            }

            return (FemaleReproductionState.Estrus, totalDays, month);
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

        //DONE
        public static bool ShouldGetPregnant(Random rand, double baseFailChance, double peakFertilityStarts, double peakFertilityEnds, double totalDays)
        {
            //See if impregnation should fail based on the base impregnation fail chance and the modifiers to it.
            if (rand.NextDouble() <= CalculateImpregnationFailChance(baseFailChance, peakFertilityStarts, peakFertilityEnds, totalDays)) return false;

            //Impregnation was successful, so get pregnant.
            return true;
        }

        //TODO
        //Need to scale fail chance with weight, temp, health, age, etc.
        //If the returned double is higher than a random double between 0 and 1, then impregnation fails. If it is lower, impregnation succeeds.
        public static double CalculateImpregnationFailChance(double baseFailChance, double peakFertilityStarts, double peakFertilityEnds, double totalDays)
        {
            if (peakFertilityStarts < 0 || peakFertilityEnds < 0) return baseFailChance;

            double fertilityStrength = GetGaussianWeight(totalDays, peakFertilityStarts, peakFertilityEnds);
            double failChance = baseFailChance * (1.0 - fertilityStrength);

            return Math.Clamp(failChance, 0.0, 1.0);
        }
    }
}