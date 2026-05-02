using FarmingOverhaul.src.Constants.AnimalsConstants;
using FarmingOverhaul.src.Systems;
using FarmingOverhaul.src.Systems.Breeding.Enums;
using FarmingOverhaul.src.Systems.Breeding.States;
using Vintagestory.API.Common;

namespace FarmingOverhaulTests.tests.SystemsTests.BreedingTests.StatesTests
{
    public class PregnancyStateTests
    {
        private static PregnancyState CreateStateWithOverride(double minDaysPreg, double maxDaysPreg, int minFetus, int maxFetus, double lateGestPerc)
        {
            TreeAccessor treeAccessor = FakesProvider.CreateTreeAccessor();
            PregnancyConstants pregancy = new()
            {
                MinDaysPregnant = minDaysPreg,
                MaxDaysPregnant = maxDaysPreg,
                MinFetusAmount = minFetus,
                MaxFetusAmount = maxFetus,
                LateGestationPercent = lateGestPerc
            };
            Random random = new(1);
            int daysPerMonth = 9;

            return new PregnancyState(treeAccessor, pregancy, random, daysPerMonth);
        }

        [Fact]
        public void EnterState_SetsValuesInExpectedRange()
        {
            double minDaysPregnant = 1;
            double maxDaysPregnant = 2;
            int minFetusAmount = 1;
            int maxFetusAmount = 5;
            double lateGestationPercent = 0.7;
            PregnancyState pregnancy = CreateStateWithOverride(
                minDaysPregnant, maxDaysPregnant, 
                minFetusAmount, maxFetusAmount, 
                lateGestationPercent);


            double transitionDays = 100;
            EnumMonth transitionMonth = EnumMonth.December;
            pregnancy.EnterState(transitionDays, transitionMonth);


            Assert.InRange(pregnancy.FetusAmount, minFetusAmount, maxFetusAmount);
            Assert.InRange(pregnancy.LengthDays, minDaysPregnant, minDaysPregnant);
            Assert.Equal(transitionDays, pregnancy.StartTotalDays);
        }

        [Fact]
        public void UpdateState_IfPregnancyHasFinished_ReturnCooldown()
        {
            double minDaysPregnant = 1;
            double maxDaysPregnant = 2;
            int minFetusAmount = 1;
            int maxFetusAmount = 5;
            double lateGestationPercent = 0.7;
            PregnancyState pregnancy = CreateStateWithOverride(
                minDaysPregnant, maxDaysPregnant,
                minFetusAmount, maxFetusAmount,
                lateGestationPercent);


            double transitionDays = 100;
            EnumMonth transitionMonth = EnumMonth.December;
            pregnancy.EnterState(transitionDays, transitionMonth);

            //PregnancyLength must be between days 101 and 102. Check at day 105 when it has finished
            double totalDays = 105;
            EnumMonth currentMonth = EnumMonth.December;
            (var nextStateOut, var transitionDaysOut, var transitionMonthOut) = pregnancy.UpdateState(totalDays, currentMonth);

            Assert.Equal(FemaleReproductionState.Cooldown, nextStateOut);
            Assert.
        }
    }
}
