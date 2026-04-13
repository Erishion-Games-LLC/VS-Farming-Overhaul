using FarmingOverhaul.src.Behaviors;
using FarmingOverhaul.src.Systems;
using FarmingOverhaul.src.Systems.Breeding;
using FarmingOverhaulTests.tests.Systems.Breeding.States;
using Moq;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

public class ReproductionStateManagerTests
{
    private ReproductionStateManager CreateManager()
    {
        var treeAccessor = FakesProvider.CreateTreeAccessor();
        var loggerMock = new Mock<ILogger>();
        var random = new Random();
        var animalState = new Mock<AnimalState>();
        return new ReproductionStateManager(random, animalState.Object, treeAccessor, loggerMock.Object);
    }

    [Fact]
    public void IsTransitionValid_ShouldReturnTrueForValidTransitions()
    {
        var manager = CreateManager();

        // Act & Assert
        Assert.True(manager.IsTransitionValid(ReproductionState.Idle, ReproductionState.Estrus));
        Assert.True(manager.IsTransitionValid(ReproductionState.Estrus, ReproductionState.Pregnant));
        Assert.True(manager.IsTransitionValid(ReproductionState.Pregnant, ReproductionState.Cooldown));
        Assert.True(manager.IsTransitionValid(ReproductionState.Cooldown, ReproductionState.Idle));
    }
}