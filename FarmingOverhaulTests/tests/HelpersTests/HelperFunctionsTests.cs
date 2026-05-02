using FarmingOverhaul.src.Helpers.Validation;
using static FarmingOverhaul.src.Helpers.HelperFunctions;
using static FarmingOverhaulTests.tests.UtilitiesForTesting.TestHelpers;

namespace FarmingOverhaulTests.tests.HelpersTests
{
    public class HelperFunctionsTests
    {
        //Currently only fully tests AddIfNotNull and ValidateRange

        [Theory]
        [InlineData("Test", 1)]
        [InlineData(null, 0)]
        public void AddIfNotNull_OnlyAddsNonNull(string? input, int expectedCount)
        {
            List<string> list = [];

            AddIfNotNull(list, input);

            AssertListHasExactAmount(list, expectedCount);
        }

        [Fact]
        public void AddIfNotNull_MultipleCalls_BuildsListCorrectly()
        {
            List<string> list = [];
            string nullString = null;
            string[] expected = ["Test1", "Test2"];


            AddIfNotNull(list, "Test1");
            AddIfNotNull(list, nullString);
            AddIfNotNull(list, "Test2");


            AssertListHasExactAmount(list, 2);
            Assert.Equal(expected, list);
        }

        [Fact]
        public void ValidateRange_ReturnsError_WhenMinGreaterThanMax()
        {
            var min = 1;
            var max = 0;

            var result = ValidateRange(min, max, "Test");

            Assert.Equal(ValidationErrorType.MinGreaterThanMax, result.Error);
        }

        [Theory]
        [InlineData(1, 1, "Test")]
        [InlineData(0, 1, "Test")]
        public void ValidateRange_ReturnsNull_WhenMinEqualOrLessThanMax(double min, double max, string item)
        {
            var result = ValidateRange(min, max, item);

            Assert.Null(result);
        }
    }
}