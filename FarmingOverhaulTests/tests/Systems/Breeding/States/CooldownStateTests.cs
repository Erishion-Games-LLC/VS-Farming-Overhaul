using FarmingOverhaul.src.Systems;
using FarmingOverhaul.src.Systems.Breeding;
using FarmingOverhaul.src.Systems.Breeding.States;
using Vintagestory.API.Common;
using static FarmingOverhaul.src.Systems.Breeding.States.CooldownState;

namespace FarmingOverhaulTests.tests.Systems.Breeding.States
{
    public class CooldownStateTests
    {
        /* Covered functions include:
         * Ensure Tree Accessor works
         * Enter State - Checks that EndTotalDays is set to a value within the expected range.
         * Update State - Checks that ReproductionState.Idle is returned when ShouldEndCooldown is true.
         * Update State - Checks that ReproductionState.Cooldown is returned when ShouldEndCooldown is false.
         * Should End Cooldown - Checks that true is returned when totalDays is equal to or greater than endTotalDays.
         * Should End Cooldown - Checks that false is returned when totalDays is less than endTotalDays.
         */

        double minDays = 1;
        double maxDays = 5;
        double totalDays = 10;

        EnumMonth month = EnumMonth.January;
        Random random = new();

        private CooldownState CreateState()
        {
            var treeAccessor = FakesProvider.CreateTreeAccessor();
            return new CooldownState(treeAccessor, random, minDays, maxDays);
        }

        private CooldownState CreateStateWithOverrides(double minDays, double maxDays)
        {
            var treeAccessor = FakesProvider.CreateTreeAccessor();
            return new CooldownState(treeAccessor, random, minDays, maxDays);
        }


        [Fact]
        public void EnsureValuesAreStoredInTreeAccessor()
        {
            var treeAccessor = FakesProvider.CreateTreeAccessor();
            CooldownState cooldownState1 = new(treeAccessor, random, minDays, maxDays);
            CooldownState cooldownState2 = new(treeAccessor, random, minDays, maxDays);


            cooldownState1.EndTotalDays = 55;


            Assert.Equal(55, cooldownState2.EndTotalDays);
        }


        //Update EnterState tests
        [Fact]
        public void EnterState_WhenMinEqualsMax_SetsExactValue()
        {
            double minDays = 1;
            double maxDays = 1;

            CooldownState cooldownState = CreateStateWithOverrides(minDays, maxDays);


            cooldownState.EnterState(totalDays);


            Assert.Equal(totalDays + minDays, cooldownState.EndTotalDays);
        }

        [Fact]
        public void EnterState_SetsEndTotalDaysWithinExpectedRange()
        {
            CooldownState cooldownState = CreateState();


            cooldownState.EnterState(totalDays);


            Assert.InRange(cooldownState.EndTotalDays, totalDays + minDays, totalDays + maxDays);
        }


        ////UpdateState tests
        //[Fact]
        //public void UpdateState_WhenShouldEndCooldownIsTrue_ReturnsIdle()
        //{            
        //    CooldownState cooldownState = CreateState();
        //    cooldownState.EndTotalDays = 1;


        //    ReproductionState newState = cooldownState.UpdateState(totalDays, month);

            
        //    Assert.Equal(ReproductionState.Idle, newState);
        //}

        //[Fact]
        //public void UpdateState_WhenShouldEndCooldownIsFalse_ReturnsCooldown()
        //{
        //    CooldownState cooldownState = CreateState();
        //    cooldownState.EndTotalDays = totalDays + 1;


        //    ReproductionState newState = cooldownState.UpdateState(totalDays, month);


        //    Assert.Equal(ReproductionState.Cooldown, newState);
        //}


        //ShouldEndCooldown tests
        [Fact]
        public void ShouldEndCooldown_WhenTotalDaysIsEqualToEndTotalDays_ReturnsTrue()
        {
            double totalDays = 10.0;
            double endTotalDays = 10.0;
            

            bool result = ShouldEndCooldown(totalDays, endTotalDays);


            Assert.True(result);
        }

        [Fact]
        public void ShouldEndCooldown_WhenTotalDaysIsGreaterThanEndTotalDays_ReturnsTrue()
        {
            double totalDays = 10.000000001;
            double endTotalDays = 10.00000000;


            bool result = ShouldEndCooldown(totalDays, endTotalDays);


            Assert.True(result);
        }

        [Fact]
        public void ShouldEndCooldown_WhenTotalDaysIsLessThanEndTotalDays_ReturnsFalse()
        {
            double totalDays = 9.9999999;
            double endTotalDays = 10.00000;


            bool result = ShouldEndCooldown(totalDays, endTotalDays);


            Assert.False(result);
        }
    }
}