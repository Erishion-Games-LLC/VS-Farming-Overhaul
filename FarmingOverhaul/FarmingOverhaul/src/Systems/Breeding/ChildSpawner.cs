using FarmingOverhaul.src.Behaviors;
using System;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using static FarmingOverhaul.src.HelperFunctions;

namespace FarmingOverhaul.src.Systems.Breeding
{
    public static class ChildSpawner
    {
        //TODO
        //need to add support for father entity when genetics are implemented
        //Spawn a child entity with the appropriate species and type, based on the mother.
        public static void SpawnChild(Entity mother, AnimalState motherAnimalState, double birthTotalDays)
        {
            ICoreAPI api = mother.Api;

            if (api.Side != EnumAppSide.Server)
            {
                return;
            }

            IWorldAccessor World = api.World;

            Entity child = CreateChild(motherAnimalState, World);

            ModifyChildDefaultPostionAndMotion(child, mother, World.Rand);

            ModifyChildAttributes(child, motherAnimalState, birthTotalDays, World.Calendar.HoursPerDay);

            World.SpawnEntity(child);
        }

        private static Entity CreateChild(AnimalState motherAnimalState, IWorldAccessor World)
        {
            string childGender = DetermineGender(World.Rand);
            string childEntityCode = GenerateChildEntityCode(motherAnimalState.Species, motherAnimalState.Type, childGender);
            
            AssetLocation childAssetLocation = new(childEntityCode);
            EntityProperties childEntityType = World.GetEntityType(childAssetLocation) ?? throw new InvalidOperationException($"Invalid child entity code: {childAssetLocation}");
           
            Entity child = World.ClassRegistry.CreateEntity(childEntityType) ?? throw new InvalidOperationException($"Failed to create entity for {childEntityType}");
           
            return child;
        }

        private static string GenerateChildEntityCode(string species, string type, string gender)
        {
            return $"{species}-{type}-baby-{gender}";
        }

        //Ensure the child is spawned next to mother, with a small variance
        private static void ModifyChildDefaultPostionAndMotion(Entity child, Entity mother, Random rand)
        {
            //Modify its default position and motion
            child.ServerPos.SetFrom(mother.ServerPos);
            child.ServerPos.Motion.X += (rand.NextDouble() - 0.5) / 20.0;
            child.ServerPos.Motion.Z += (rand.NextDouble() - 0.5) / 20.0;
            child.Pos.SetFrom(child.ServerPos);
        }

        private static void ModifyChildAttributes(Entity child, AnimalState motherAnimalState, double birthTotalDays, float hoursPerDay)
        {
            child.Attributes.SetString("origin", "reproduction");
            child.WatchedAttributes.SetInt("generation", motherAnimalState.Generation + 1);

            AnimalState childAnimalState = GetBehavior<AnimalState>(child, nameof(ChildSpawner));
            childAnimalState.BirthTotalDays = birthTotalDays;

            //Grab or create the tree used by EntityBehaviorGrow
            //and make it use the historical birthtime instead of always having birth time be now
            ITreeAttribute growTree = child.WatchedAttributes.GetTreeAttribute("grow") ?? new TreeAttribute();
            
            double birthHours = birthTotalDays * hoursPerDay;
            growTree.SetDouble("timeSpawned", birthHours);
            child.WatchedAttributes.SetAttribute("grow", growTree);
        }

        //TODO
        //need to eventually add support for uneven sex distribution
        private static string DetermineGender(Random rand)
        {
            //Roll a random integer between 0 and 1. If 0, gender is female. If 1, gender is male.
            if (GenerateRandomIntFromMaxInclusive(rand, 1) == 0) return "female";
            else return "male";
        }
    }
}