using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace FarmingOverhaul.src
{
    public static class HelperFunctions
    {
        /// Returns a random integer between min and max, inclusive.
        public static int GenerateRandomIntFromMinMaxInclusive(Random random, int min, int max)
        {
            return random.Next(min, max + 1);
        }

        /// Returns a random integer between 0 and max, inclusive.
        public static int GenerateRandomIntFromMaxInclusive(Random random, int max)
        {
            return random.Next(max + 1);

        }

        /// Returns a random double between min and max, inclusive.
        public static double GenerateRandomDouble(Random random, double min, double max)
        {
            return (max - min) * random.NextDouble() + min;
        }

        //Returns a random number based on the normal distribution with the given mean and standard deviation,
        //but if the number is outside of the given range, it will keep generating until it gets one that is within the range.
        //The standard deviation is calculated based on the range so that 99.7% of the numbers will be within the range.
        public static double SampleNormalDistributionInRange(Random random, double min, double max)
        {
            double val;
            double standardDev = (max - min) / 6;
            double mean = (max + min) / 2;

            do { val = SampleGaussian(random, mean, standardDev); } 
            while (val < min || val > max);

            return val;
        }
               
        public static double SampleNormalDistributionInRange(Random random, double min, double max, double mean, double stdDev)
        {
            double val;

            do { val = SampleGaussian(random, mean, stdDev); }
            while (val < min || val > max);
            return val;
        }

        /// <summary>
        /// This method uses the Box-Muller transform to generate values from a standard normal
        /// distribution, which are then scaled and shifted to match the specified parameters.</summary>
        /// <param name="random">The random number generator to use for producing the sample. Cannot be null.</param>
        /// <param name="mean">The mean, or expected value, of the normal distribution. Determines the center of the distribution.</param>
        /// <param name="stdDev">The standard deviation of the normal distribution. Must be non-negative. Determines the spread of the
        /// distribution.</param>
        /// <returns>A double-precision floating-point number sampled from the normal distribution defined by the specified mean
        /// and standard deviation.</returns>
        public static double SampleGaussian(Random random, double mean, double stdDev)
        {
            return mean + stdDev * SampleStandardNormalDistribution(random);
        }

        /// <summary>
        /// Generates a random value sampled from the standard normal distribution.
        /// </summary>
        /// <remarks>This method uses the Box-Muller transform to convert uniformly distributed random
        /// numbers into a normally distributed value.
        /// <param name="random">The random number generator used to produce the sample. Must not be null.</param>
        /// <returns>A double-precision floating-point number representing a random sample from the standard normal distribution,
        /// with a mean of 0 and a standard deviation of 1.</returns>
        public static double SampleStandardNormalDistribution(Random random)
        {
            double var1 = 1.0 - random.NextDouble();
            double var2 = 1.0 - random.NextDouble();

            return Math.Sqrt(-2.0 * Math.Log(var1)) * Math.Cos(2.0 * Math.PI * var2);
        }

        public static double GetGaussianWeight(double value, double min, double max, double stdDevSpread = 6)
        {
            double mean = (max + min) / 2;
            double stdDev = (max - min) / stdDevSpread;

            if (stdDev <= 0) return value == mean ? 1.0 : 0.0;

            double stdDevsFromCenter = (value - mean) / stdDev;

            return Math.Exp(-0.5 * stdDevsFromCenter * stdDevsFromCenter);
        }

        //Returns the species of the entity as a lowercase string.
        public static string GetSpeciesStringLowerFromEntity(Entity entity)
        {
            return entity.Code.Path.Split('-')[0].ToLower();
        }

        public static bool HasTimeFinished(double totalDays, double start, double length)
        {
            return totalDays >= start + length;
        }

        public static void AddIfNotNull<T>(List<T> list, T? item)
        {
            if (item != null) list.Add(item);
        }

        public static string? ValidateRange(double min, double max, string field)
        {
            if (min > max)
            {
                return ($"{field} min value is greater than max value. Must be less than or equal to.");
            }
            else return null;
        }

        public static T GetBehavior<T>(Entity entity, string caller) where T : EntityBehavior
        {
            var behavior = entity.GetBehavior<T>();
            if (behavior == null)
            {
                throw new InvalidOperationException($"{caller} requires {typeof(T).Name} but it is missing on {entity.Code}");
            }
            return behavior;
        }


        //Calculate the month based on the total days and the number of days per month. This assumes that the first day of the year is day 0 and corresponds to the first month
        public static EnumMonth GetMonthFromDay(double day, int daysPerMonth)
        {
            int DaysPerYear = daysPerMonth * 12;

            //Normalize the day to a value between 0 and DaysPerYear - 1
            int normalizedDay = (int)day % DaysPerYear;

            // Calculate the month index by dividing the normalized day by the number of days per month
            int monthIndex = (normalizedDay / daysPerMonth);
            return (EnumMonth)monthIndex;
        }

        public static int GetYearsCompletedFromDay(double startDay, int daysPerYear)
        {
            return (int)(startDay - (startDay % daysPerYear));
        }

        public static Dictionary<EnumMonth, int> CreateMonthStartDayDict(int daysPerMonth)
        {
            Dictionary<EnumMonth, int> monthStartDayDict = [];
            int startingTotalDays = 0;
            int totalDays = startingTotalDays;
            foreach (EnumMonth month in Enum.GetValues<EnumMonth>())
            {
                monthStartDayDict.Add(month, totalDays);
                totalDays += daysPerMonth;
            }
            return monthStartDayDict;
        }
    }
}