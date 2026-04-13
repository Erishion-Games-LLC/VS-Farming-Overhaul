using FarmingOverhaul.src.Systems;
using Moq;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;

namespace FarmingOverhaulTests.tests.Systems.Breeding.States
{
    public class FakesProvider
    {
        public static TreeAccessor CreateTreeAccessor()
        {
            var tree = new TreeAttribute();
            var entity = new Mock<Entity>().Object;
            var treeKey = "testTreeKey";

            entity.WatchedAttributes.SetAttribute(treeKey, tree);

            return new TreeAccessor(tree, entity, treeKey);
        }
    }
}