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

        public static double GenerateRandomDouble(double min, double max)
        {
            return (max - min) * Random.Shared.NextDouble() + min;
        }

        public static double GetDoubleFromNormalDistributionClamped(double min, double max)
        {
            double val;
            double standardDev = (max - min) / 6;
            double mean = (max - min) / 2;

            do { val = GenerateNormalDistribution(mean, standardDev); } 
            while (val < min || val > max);

            return val;
        }

        private static double GenerateNormalDistribution(double mean, double standardDev)
        {
            double var1 = 1.0 - Random.Shared.NextDouble();
            double var2 = 1.0 - Random.Shared.NextDouble();

            double randStdNormal =
                Math.Sqrt(-2.0 * Math.Log(var1)) *
                Math.Sin(2.0 * Math.PI * var2);
            return mean + standardDev * randStdNormal;
        }

        public static string GetSpeciesStringLowerFromEntity(Entity entity)
        {
            return entity.Code.Path.Split('-')[0].ToLower();
        }
    }
}