using FarmingOverhaul.src.Helpers.StateMachine;
using FarmingOverhaul.src.Systems.Breeding.Enums;
using System;
using System.Collections.Generic;
using Vintagestory.API.Common;
using Vintagestory.API.Util;
using static FarmingOverhaul.src.HelperFunctions;

namespace FarmingOverhaul.src.Systems.Breeding.States
{
    public class IdleState(TreeAccessor treeAccessor, EnumMonth[] breedingSeason, int daysPerMonth) : IState<FemaleReproductionState>
    {
        private readonly TreeAccessor treeAccessor = treeAccessor;
        private readonly EnumMonth[] breedingSeason = breedingSeason;
        private readonly int daysPerMonth = daysPerMonth;
        public FemaleReproductionState State => FemaleReproductionState.Idle;
        private readonly string prefix = nameof(IdleState);

        readonly Dictionary<EnumMonth, int> MonthStartDayDict = CreateMonthStartDayDict(daysPerMonth);

        private double DayEnteredState
        {
            get => treeAccessor.GetDoubleFromTree(prefix + nameof(DayEnteredState));
            set => treeAccessor.SetDoubleInTree(prefix + nameof(DayEnteredState), value);
        }
        private int MonthEnteredStateInt
        {
            get => treeAccessor.GetIntFromTree(prefix + nameof(MonthEnteredStateInt));
            set => treeAccessor.SetIntInTree(prefix + nameof(MonthEnteredStateInt), value);
        }

        public void EnterState(double transitionDays, EnumMonth transitionMonth)
        {
            DayEnteredState = transitionDays;
            MonthEnteredStateInt = (int)transitionMonth;
        }

        public void ExitState()
        {
        }

        public (FemaleReproductionState nextState, double transitionDays, EnumMonth transitionMonth) UpdateState(double totalDays, EnumMonth month)
        {
            //If the month we entered the state in was during breeding season, then we don't need to check if breeding season has occurred since we know we are currently in it
            //therefor, return estrus, the day we entered idle, and the month we entered idle
            if (IsBreedingSeason(breedingSeason, (EnumMonth)MonthEnteredStateInt))
            {
                return (FemaleReproductionState.Estrus, DayEnteredState, (EnumMonth)MonthEnteredStateInt);
            }

            //Check if estrus SHOULD have been triggered between the month we entered and the current month. This is necessary in case the animal entered idle during a non-breeding season month, but breeding season has since started
            //therefor, return estrus, the day and month that the breeding season was supposed to occur.
            double dayBreedingSeasonOccured = WhenDidBreedingSeasonOccur(breedingSeason, MonthStartDayDict, DayEnteredState, MonthEnteredStateInt, month, daysPerMonth);
           
            if (dayBreedingSeasonOccured != -1)
            {
                return (FemaleReproductionState.Estrus, dayBreedingSeasonOccured, GetMonthFromDay(dayBreedingSeasonOccured, daysPerMonth));
            }

            //If we get here, then breeding season has not started since we entered the state
            //therefor, return the most current time which is the called time
            return (FemaleReproductionState.Idle, totalDays, month);
        }

        private static double WhenDidBreedingSeasonOccur(
            EnumMonth[] breedingSeason, 
            Dictionary<EnumMonth, int> monthStartDayDict,
            double dayEnteredState,
            int monthEnteredState, 
            EnumMonth currentMonth, 
            int daysPerMonth
            )
        {
            //Start checking from the month after the month we entered the state, since if the month we entered was in breeding season, then we would have already returned from the UpdateState function
            //if we enter in january/month0, start checking at february/month1. 
            //Feb year0, month1 modulo 12 = 1;
            //Feb year1, month13 modulo 12 = 1;
            int monthToCheckIndex = (monthEnteredState + 1) % 12;
            
            while (true)
            {
                EnumMonth monthToCheck = (EnumMonth)monthToCheckIndex;
                if (IsBreedingSeason(breedingSeason, monthToCheck))
                {
                    int daysPerYear = daysPerMonth * 12;
                    int yearsCompleted = GetYearsCompletedFromDay(dayEnteredState, daysPerYear);
                    int daysToAdd = yearsCompleted * daysPerYear;
                    int startDayOfMonth = monthStartDayDict[monthToCheck];

                    //if monthToCheck is less than the month we entered the state,
                    //we are checking if a month NEXT YEAR relative to the historical start time is the breeding season.
                    //because of this, we need to add an additional year
                    if (monthToCheckIndex < monthEnteredState )
                    {
                        return daysToAdd + daysPerYear + startDayOfMonth;

                    }
                    else
                    {
                        return daysToAdd + startDayOfMonth;
                    }
                }

                if (monthToCheck == currentMonth) 
                    break;

                else monthToCheckIndex = (monthToCheckIndex + 1) % 12;
            }

            return -1;
        }

        private static bool IsBreedingSeason(EnumMonth[] breedingSeason, EnumMonth currentMonth)
        {
            return breedingSeason?.Contains(currentMonth) == true;
        }
    }
}