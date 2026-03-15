using System;
using Vintagestory.API.Common.Entities;

namespace FarmingOverhaul
{
    public static class HelperFunctions
    {
        public static int GenerateRandomInt(int min, int max)
        {
            return Random.Shared.Next(min, max + 1);
        }

        public static string GetSpeciesStringLowerFromEntity(Entity entity)
        {
            return entity.Code.Path.Split('-')[0].ToLower();
        }
    }
}