using FarmingOverhaul.src.Behaviors;
using System;
using Vintagestory.API.Common.Entities;
using Vintagestory.API.Datastructures;
using static FarmingOverhaul.src.HelperFunctions;

namespace FarmingOverhaul.src.Systems.Breeding.Behaviors
{
    public abstract class BaseBreedingBehavior(Entity entity) : BaseBehavior(entity)
    {
        public const string BaseBreedingBehaviorKey = "fobasebreedingbehavior";
        protected AnimalState animalState;
        private WeightBehavior weightBehavior;

        private long updateReproductionTickListenerId = 0;
        private int updateReproductionTimerMs;


        public override void Initialize(EntityProperties properties, JsonObject attributes)
        {
            base.Initialize(properties, attributes);
            SetRequiredBehaviors();

            updateReproductionTimerMs = CalculateReproductionTimerMs(Calendar.SpeedOfTime, Calendar.CalendarSpeedMul);
            //For testing, set update timer to 15 seconds
            updateReproductionTimerMs = 15000;
        }

        protected virtual void UpdateReproduction(float deltaTime)
        {
            //If animal is dead, stop all ticking and exit
            if (!entity.Alive) { DisableTickListeners(); Logger.Error("entity is dead"); return; }
        }

        public override void EnableTickListeners()
        {
            if (updateReproductionTickListenerId != 0) return;

            updateReproductionTickListenerId = Api.Event.RegisterGameTickListener(UpdateReproduction, updateReproductionTimerMs);
        }

        public override void DisableTickListeners()
        {
            if (updateReproductionTickListenerId == 0) return;

            Api.Event.UnregisterGameTickListener(updateReproductionTickListenerId);
            updateReproductionTickListenerId = 0;
        }

        public override void SetRequiredBehaviors()
        {
            animalState = GetBehavior<AnimalState>(entity, nameof(BaseBreedingBehavior));
            weightBehavior = GetBehavior<WeightBehavior>(entity, nameof(BaseBreedingBehavior));
        }

        /*Calculate how often the reproduction function should be called based on the game speed.
        *speedOfTime is a physics affecting multipler for how fast an IRL second is for an in game second. Default is 60. So one irl second is one in game minute. 
        One irl minute is one in game hour. 24 irl minutes is one in game day.

        calSpeedMul is a non physics affecting multiplier for how fast an IRL seconds is for an in game second. Defualt is 0.5. So days last 50% longer.
        When combined with speedOfTime, this results in 48 irl minutes per in game day.*/
        private static int CalculateReproductionTimerMs(float speedOfTime, float calSpeedMul)
        {
            const int secondsPerMinute = 60;
            const int millisecondsPerSecond = 1000;
            const double desiredInGameMinutesBetweenUpdate = 30;

            double desiredInGameSecondsBetweenUpdate = desiredInGameMinutesBetweenUpdate * secondsPerMinute;

            float calendarMultiplier = speedOfTime * calSpeedMul;
            if (calendarMultiplier <= 0) throw new ArgumentException($"Either SpeedOfTime or CalendarSpeedMul are 0. Must be greater than 0.");

            double timerInSec = desiredInGameSecondsBetweenUpdate / calendarMultiplier;

            return (int)Math.Round(timerInSec * millisecondsPerSecond);
        }
    }
}
