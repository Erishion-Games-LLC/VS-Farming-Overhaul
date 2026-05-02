using FarmingOverhaul.src.Systems;
using FarmingOverhaul.src.Systems.Breeding.Enums;
using FarmingOverhaul.src.Systems.Breeding.States;
using FarmingOverhaulTests.tests.SystemsTests.BreedingTests.StatesTests;
using Vintagestory.API.Common;
using static FarmingOverhaul.src.Helpers.HelperFunctions;

namespace FarmingOverhaulTests.tests.Systems.Breeding.States
{
    public class CooldownStateTests
    {
        int daysPerMonth = 9;

        private CooldownState CreateStateWithOverrides(double minDays, double maxDays)
        {            
            Random random = new(1);
            TreeAccessor treeAccessor = FakesProvider.CreateTreeAccessor();

            return new CooldownState(treeAccessor, random, minDays, maxDays, daysPerMonth);
        }


        [Fact]
        public void EnterState_SetsEndTotalDaysWithinRange()
        {
            double minDays = 1;
            double maxDays = 2;
            double transitionDays = 100;
            CooldownState cooldownState = CreateStateWithOverrides(minDays, maxDays);

            cooldownState.EnterState(transitionDays, EnumMonth.January);

            Assert.InRange(cooldownState.EndTotalDays, transitionDays + minDays, transitionDays + maxDays);
        }

        [Fact]
        public void UpdateState_WhenCooldownEnded_ReturnIdle()
        {
            double minDays = 1;
            double maxDays = 2;
            CooldownState cooldownState = CreateStateWithOverrides(minDays, maxDays);

            double transitionDaysIn = 100;
            EnumMonth transitionMonthIn = EnumMonth.January;
            cooldownState.EnterState(transitionDaysIn, transitionMonthIn);

            //endTotalDays can only be between 101 and 102 days. Check at day 105, when we know cooldown has to be over.
            double totalDays = 105;
            EnumMonth currentMonth = EnumMonth.December;
            (var nextState, var transitionDaysOut, var transitionMonthOut) = cooldownState.UpdateState(totalDays, currentMonth);


            Assert.Equal(FemaleReproductionState.Idle, nextState);
            Assert.Equal(cooldownState.EndTotalDays, transitionDaysOut);
            Assert.Equal(GetMonthFromDay(cooldownState.EndTotalDays, daysPerMonth), transitionMonthOut);
        }

        [Fact]
        public void UpdateState_WhenCooldownNotOver_ReturnCooldown()
        {
            double minDays = 1;
            double maxDays = 2;
            CooldownState cooldownState = CreateStateWithOverrides(minDays, maxDays);

            double transitionDaysIn = 100;
            var transitionMonthIn = EnumMonth.January;
            cooldownState.EnterState(transitionDaysIn, transitionMonthIn);

            //endTotalDays can only be between 101 and 102 days. Check at day 100.5, when we know cooldown has not ended yet
            double totalDays = 100.5;
            EnumMonth currentMonth = EnumMonth.December;


            (var nextState, var transitionDaysOut, var transitionMonthOut) = cooldownState.UpdateState(totalDays, currentMonth);

            Assert.Equal(FemaleReproductionState.Cooldown, nextState);
            Assert.Equal(totalDays, transitionDaysOut);
            Assert.Equal(currentMonth, transitionMonthOut);
        }


        [Theory]
        [InlineData(5, 5, true)]
        [InlineData(5, 4, true)]
        [InlineData(4, 5, false)]
        private void ShouldEndCooldown_ReturnsExpectedValue(double totalDays, double endTotalDays, bool expectedOutcome)
        {
            bool result = CooldownState.ShouldEndCooldown(totalDays, endTotalDays);

            Assert.Equal(expectedOutcome, result);
        }
    }
}