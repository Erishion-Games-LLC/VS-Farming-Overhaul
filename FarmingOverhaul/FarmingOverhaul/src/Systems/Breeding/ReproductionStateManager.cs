using FarmingOverhaul.src.Behaviors;
using FarmingOverhaul.src.Systems.Breeding.States;
using System;
using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Systems.Breeding
{
    public class ReproductionStateManager
    {
        public ReproductionState CurrentState
        {
            get => (ReproductionState)treeAccessor.GetIntFromTree(nameof(CurrentState));
            private set => treeAccessor.SetIntInTree(nameof(CurrentState), (int)value);
        }
        protected IReproState CurrentStateInstance;

        private IdleState Idle { get; }
        private EstrusState Estrus { get; }
        private PregnancyState Pregnancy { get; }
        private CooldownState Cooldown { get; }

        private readonly TreeAccessor treeAccessor;
        private readonly ILogger logger;

        public event Action<int>? OnBirth;

        public ReproductionStateManager(Random random, AnimalState animalState, TreeAccessor treeAccessor, ILogger logger)
        {
            this.treeAccessor = treeAccessor;
            this.logger = logger;
            AnimalConstants constants = animalState.Constants;

            Idle = new IdleState(animalState);
            Estrus = new EstrusState(treeAccessor, animalState, random);
            Pregnancy = new PregnancyState(treeAccessor, animalState, random);
            Cooldown = new CooldownState(treeAccessor, random, constants.MinDaysBeforeBreedAgainFemale, constants.MaxDaysBeforeBreedAgainFemale);
            CurrentStateInstance = GetStateInstance(CurrentState);

            Pregnancy.OnBirth += (fetusAmount) => OnBirth?.Invoke(fetusAmount);
        }

        public void Update(double totalDays, EnumMonth month)
        {
            ReproductionState nextStateEnum = CurrentStateInstance.UpdateState(totalDays, month);
            if (nextStateEnum != CurrentState)
            {
                TransitionState(nextStateEnum, totalDays);
            }
        }

        private IReproState GetStateInstance(ReproductionState state)
        {
            return state switch
            {
                ReproductionState.Idle => Idle,
                ReproductionState.Estrus => Estrus,
                ReproductionState.Pregnant => Pregnancy,
                ReproductionState.Cooldown => Cooldown,
                _ => throw new ArgumentException($"Invalid reproduction state: {state}")
            };
        }

        private void TransitionState(ReproductionState newState, double totalDays)
        {
            if (!IsTransitionValid(CurrentState, newState))
            {                
                logger.Error($"Invalid state transition. Cannot transition from {CurrentState} to {newState}.");
                return;
            }

            CurrentStateInstance.ExitState();
            CurrentState = newState;
            CurrentStateInstance.EnterState(totalDays);
        }
        private static bool IsTransitionValid(ReproductionState oldState, ReproductionState newState)
        {
            if (oldState == newState) 
            {
                return false; 
            }

            return (oldState, newState) switch
            {
                (ReproductionState.Idle, ReproductionState.Estrus) => true,
                (ReproductionState.Estrus, ReproductionState.Idle) => true,
                (ReproductionState.Estrus, ReproductionState.Pregnant) => true,
                (ReproductionState.Pregnant, ReproductionState.Cooldown) => true,
                (ReproductionState.Cooldown, ReproductionState.Idle) => true,                
                _ => false
            };
        }
    }
}