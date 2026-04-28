using FarmingOverhaul.src.Behaviors;
using FarmingOverhaul.src.Constants.AnimalConstants;
using FarmingOverhaul.src.Helpers.StateMachine;
using FarmingOverhaul.src.Systems.Breeding.Enums;
using System;
using System.Collections.Generic;
using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Systems.Breeding.States.Managers
{
    public class FemaleReproductionStateManager
    {
        private readonly StateMachine<FemaleReproductionState> stateMachine;
        private readonly Dictionary<FemaleReproductionState, IState<FemaleReproductionState>> states = [];
        private readonly string currentStateKey = "currentstate";

        public FemaleReproductionState CurrentState => stateMachine.CurrentState;

        private readonly TreeAccessor treeAccessor;
        private readonly ILogger logger;

        public event Action<int, double>? OnBirth;

        public FemaleReproductionStateManager(Random random, AnimalState animalState, TreeAccessor treeAccessor, ILogger logger, int daysPerMonth)
        {        
            this.treeAccessor = treeAccessor;
            this.logger = logger;
            AnimalConstants constants = animalState.Constants;

            states.Add(FemaleReproductionState.Idle, new IdleState(treeAccessor, constants.Estrus.BreedingSeason, daysPerMonth));
            states.Add(FemaleReproductionState.Estrus, new EstrusState(treeAccessor, animalState, random));
            states.Add(FemaleReproductionState.Pregnant, new PregnancyState(treeAccessor, constants.Pregnancy, random, daysPerMonth));
            states.Add(FemaleReproductionState.Cooldown, new CooldownState(treeAccessor, random, constants.Breeding.MinDaysBeforeBreedAgainFemale, constants.Breeding.MaxDaysBeforeBreedAgainFemale, daysPerMonth));

            stateMachine = new StateMachine<FemaleReproductionState>(
                loadState: () => (FemaleReproductionState)treeAccessor.GetIntFromTree(currentStateKey),
                persistState: (newState) => treeAccessor.SetIntInTree(currentStateKey, (int)newState),
                stateResolver: (currentState) => states[currentState],
                transitionValidator: IsTransitionValid,
                logger
                );

            var pregnancy = (PregnancyState)states[FemaleReproductionState.Pregnant];
            pregnancy.OnBirth += (fetusAmount, birthTotalDays) => OnBirth?.Invoke(fetusAmount, birthTotalDays);
        }
                
        public void Update(double totalDays, EnumMonth month)
        {
            stateMachine.UpdateState(totalDays, month);
        }

        private static bool IsTransitionValid(FemaleReproductionState oldState, FemaleReproductionState newState)
        {
            if (oldState == newState) 
            {
                return false; 
            }

            return (oldState, newState) switch
            {
                (FemaleReproductionState.Idle, FemaleReproductionState.Estrus) => true,
                (FemaleReproductionState.Estrus, FemaleReproductionState.Idle) => true,
                (FemaleReproductionState.Estrus, FemaleReproductionState.Pregnant) => true,
                (FemaleReproductionState.Pregnant, FemaleReproductionState.Cooldown) => true,
                (FemaleReproductionState.Cooldown, FemaleReproductionState.Idle) => true,                
                _ => false
            };
        }
    }
}