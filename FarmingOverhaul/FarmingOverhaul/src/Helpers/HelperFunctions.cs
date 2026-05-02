using FarmingOverhaul.src.Helpers.Validation;
using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Common.Entities;

namespace FarmingOverhaul.src.Helpers
{
    public static class HelperFunctions
    {
        public static bool HasTimeFinished(double totalDays, double start, double length)
        {
            return totalDays >= start + length;
        }

        public static void AddIfNotNull<T>(List<T> list, T? item) where T : class
        {
            if (item != null) list.Add(item);
        }

        //Returns the species of the entity as a lowercase string.
        public static string GetSpeciesStringLowerFromEntity(Entity entity)
        {
            return entity.Code.Path.Split('-')[0].ToLower();
        }

        public static ValidationResult? ValidateRange(double min, double max, string field)
        {
            if (min > max)
            {
                return new ValidationResult(ValidationErrorType.MinGreaterThanMax, field);
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

        //StartPercent MUSTBE less than EndPercent
        public static (double start, double end) CreateSubWindowWithPercent(double parentStart, double parentEnd, double startPercent, double endPercent)
        {
            double duration = parentEnd - parentStart;
            double start = parentStart + (duration * startPercent);
            double end = parentStart + (duration * endPercent);
            if (startPercent > endPercent)
            {
                throw new ArgumentException("Start Percent must be less than or equal to end percent");
            }
            return (start, end);
        }

        //Adds the start and end offset to the beginning
        public static (double start, double end) CreateSubWindowWithOffsetsFromStart(double parentStart, double parentEnd, double offsetStart, double duration)
        {
            double start = parentStart + offsetStart;
            double end = Math.Min(start + duration, parentEnd);
            return (start, end);
        }
    }
}