using System;
namespace FarmingOverhaul
{
    public static class HelperFunctions
    {
        public static int GenerateRandomInt(int min, int max)
        {
            return Random.Shared.Next(min, max + 1);
        }
    }
}