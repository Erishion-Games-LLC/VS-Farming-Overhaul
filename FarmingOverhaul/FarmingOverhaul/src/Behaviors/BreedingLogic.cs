using Microsoft.VisualBasic;
using System;
using System.Globalization;
using System.Linq;
using System.Reflection.Metadata;
using Vintagestory.API.Common;
using static FarmingOverhaul.src.HelperFunctions;

namespace FarmingOverhaul.src.Behaviors
{
    public static class BreedingLogic
    {
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

        //TODO
        //need to eventually add support for uneven sex distribution
        public static string DetermineGender(Random rand)
        {
            //Roll a random integer between 0 and 1. If 0, gender is female. If 1, gender is male.
            if (GenerateRandomIntFromMaxInclusive(rand, 1) == 0) return "female";
            else return "male";
        }

        //DONE
        /*If the current amount of time passed is inbetween the min and max amount of time passed for this heat, then the animal is in heat*/
        public static bool IsInHeat(double cycleStartTotalDays, double timeBeforeHeatHours, double heatDurationHours, double totalDays)
        {
            double heatBeginsTotalDays = cycleStartTotalDays + (timeBeforeHeatHours / 24);
            double heatEndsTotalDays = heatBeginsTotalDays + (heatDurationHours / 24);

            if (totalDays >= heatBeginsTotalDays && totalDays <= heatEndsTotalDays) return true;
            else return false;
        }

        //DONE
        //Has the required time passed for the animal to breed again?
        public static bool IsBreedingCooldownActive(double totalDays, double beforeCanBePregnantAgainTotalDays)
        {
            if (totalDays >= beforeCanBePregnantAgainTotalDays) return false;
            else return true;
        }

        //TODO Eventually check based on species specific needs, like shorter daylight hours for sheep.
        //Is the current month in the animals breeding season?
        public static bool IsBreedingSeason(EnumMonth[] breedingSeason, EnumMonth currentMonth)
        {
            if (breedingSeason == null) { return false; }
            if (breedingSeason.Contains(currentMonth)) return true;
            else return false;
        }

        //DONE
        public static bool ShouldGetPregnant(bool isPregnant, Random rand, double baseFailChance, double peakFertilityStarts, double peakFertilityEnds, double totalDays)
        {
            //If already pregnant, exit
            if (isPregnant) return false;

            //See if impregnation should fail based on the base impregnation fail chance and the modifiers to it.
            if (rand.NextDouble() <= CalculateImpregnationFailChance(baseFailChance, peakFertilityStarts, peakFertilityEnds, totalDays)) return false;

            //Impregnation was successful, so get pregnant.
            return true;
        }

        public static bool ShouldGiveBirth(double totalDays, double pregnancyStartTotalDays, double pregnancyLengthDays)
        {
            if (totalDays < pregnancyStartTotalDays + pregnancyLengthDays) return false;

            return true;
        }

        //Attempts to initiate the estrus cycle for the female animal if all required conditions are met.
        public static bool ShouldStartEstrusCycle(double cycleTotalStartDays, bool isPregnant, EnumMonth[] breedingSeason, EnumMonth currentMonth, double totalDays, double beforeCanBePregnantAgainTotalDays)
        {
            //If the animal is pregnant or if the cycle is already started, end the repeating function
            if (cycleTotalStartDays != -1 || isPregnant) { return false; }

            //If it is not the breeding season or if the animal hasn't recovered from a previous pregnancy yet, repeat the function to check again later
            if (!IsBreedingSeason(breedingSeason, currentMonth) || IsBreedingCooldownActive(totalDays, beforeCanBePregnantAgainTotalDays)) { return false; }

            return true;
        }

        public static bool ShouldEndEstrusCycle(double totalDays, double cycleTotalStartDays, double cycleLengthDays)
        {
            if (totalDays - cycleTotalStartDays >= cycleLengthDays) return true;

            else return false;
        }
    }
}
