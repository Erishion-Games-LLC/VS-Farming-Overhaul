using FarmingOverhaul.src.Behaviors;
using System;
using Vintagestory.API.Common;
using static FarmingOverhaul.src.Helpers.HelperFunctions;
using static FarmingOverhaul.src.Helpers.MathHelpers;
using FarmingOverhaul.src.Systems.Breeding.Enums;
using FarmingOverhaul.src.Helpers.StateMachine;
using FarmingOverhaul.src.Constants.AnimalsConstants;

namespace FarmingOverhaul.src.Systems.Breeding.States
{
    public class EstrusState(TreeAccessor treeAccessor, AnimalState animalState, Random random) : IState<FemaleReproductionState>
    {
        private readonly TreeAccessor treeAccessor = treeAccessor;
        private readonly AnimalState animalState = animalState;
        private readonly Random rand = random;

        private readonly EstrusConstants estrus = animalState.Constants.Estrus;
        private readonly BreedingConstants breeding = animalState.Constants.Breeding;
        private readonly string prefix = nameof(EstrusState);
        private const string BreedAnimalOriginKey = "reproduction";

        private bool BreedingAttempt
        {
            get => treeAccessor.GetBoolFromTree(prefix + nameof(BreedingAttempt));
            set => treeAccessor.SetBoolInTree(prefix + nameof(BreedingAttempt), value);
        }

        public double StartTotalDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(StartTotalDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(StartTotalDays), value);
        }
        public double EndTotalDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(EndTotalDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(EndTotalDays), value);
        }

        public double HeatBeginsTotalDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(HeatBeginsTotalDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(HeatBeginsTotalDays), value);
        }
        public double HeatEndsTotalDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(HeatEndsTotalDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(HeatEndsTotalDays), value);
        }

        public double PeakFertilityBeginsTotalDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(PeakFertilityBeginsTotalDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(PeakFertilityBeginsTotalDays), value);
        }
        public double PeakFertilityEndsTotalDays
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(PeakFertilityEndsTotalDays));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(PeakFertilityEndsTotalDays), value);
        }

        public FemaleReproductionState State => FemaleReproductionState.Estrus;

        public void EnterState(double transitionDays, EnumMonth transitionMonth)
        {
            double EstrusCycleLengthDays = SampleNormalDistributionInRange(rand, estrus.EstrusCycleMinDays, estrus.EstrusCycleMaxDays);

            //If the animal is naturally generated AND it is has not completed its first Estrus, then it could have been generated at any point in the cycle. 
            if (animalState.Origin != BreedAnimalOriginKey && !animalState.CompletedFirstEstrus)
            {
                StartTotalDays = GenerateRandomDouble(rand, transitionDays - EstrusCycleLengthDays, transitionDays);                
            }

            //If the animal is not naturally generated, then it starts the cycle at the normal time.
            else
            {
                StartTotalDays = transitionDays;
            }

            EndTotalDays = StartTotalDays + EstrusCycleLengthDays;

            double TimeBeforeHeatDays = SampleNormalDistributionInRange(rand, estrus.TimeBeforeHeatMinDays, estrus.TimeBeforeHeatMaxDays);
            double HeatDurationDays = SampleNormalDistributionInRange(rand, estrus.HeatDurationMinDays, estrus.HeatDurationMaxDays);
            (HeatBeginsTotalDays, HeatEndsTotalDays) = CreateSubWindowWithOffsetsFromStart(StartTotalDays, EndTotalDays, TimeBeforeHeatDays, HeatDurationDays);
            (PeakFertilityBeginsTotalDays, PeakFertilityEndsTotalDays) = CreateSubWindowWithPercent(HeatBeginsTotalDays, HeatEndsTotalDays, estrus.PeakFertilityStartsXPercentAfterHeatBegins, estrus.PearkFertilityEndsXPercentAfterHeatBegins);
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
            if (BreedingAttempt)
            {
                BreedingAttempt = false;

                if (ShouldGetPregnant(rand, breeding.ImpregnationFailChance, PeakFertilityBeginsTotalDays, PeakFertilityEndsTotalDays, totalDays))
                    return (FemaleReproductionState.Pregnant, totalDays, month);

                else return (FemaleReproductionState.Estrus, totalDays, month);
            }

            //Check if the estrus cycle has completed without impregnation, and transition back to idle if so.
            if (totalDays >= EndTotalDays)
            {
                return (FemaleReproductionState.Idle, totalDays, month);
            }

            return (FemaleReproductionState.Estrus, totalDays, month);
        }

        //Needs to be called from the male animal when breeding occurs
        public void OnBreedingAttempt()
        {
            BreedingAttempt = true;
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

        //DONE
        /*If the current amount of time passed is inbetween the min and max amount of time passed for this heat, then the animal is in heat*/
        public static bool IsInHeat(double cycleStartTotalDays, double timeBeforeHeatHours, double heatDurationHours, double totalDays, float hoursPerDay)
        {
            double heatBeginsTotalDays = cycleStartTotalDays + (timeBeforeHeatHours / hoursPerDay);
            double heatEndsTotalDays = heatBeginsTotalDays + (heatDurationHours / hoursPerDay);

            if (totalDays >= heatBeginsTotalDays && totalDays <= heatEndsTotalDays) return true;
            else return false;
        }
    }
}