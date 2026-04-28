using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Helpers.StateMachine
{
    public class StateMachine<TStateEnum>
    {
        private readonly System.Func<TStateEnum> loadState;
        private readonly System.Action<TStateEnum> persistState;
        private readonly System.Func<TStateEnum, IState<TStateEnum>> stateResolver;
        private readonly System.Func<TStateEnum, TStateEnum, bool> transitionValidator;
        private readonly ILogger logger;

        public TStateEnum CurrentState { get; private set; }
        private IState<TStateEnum> currentStateInstance;

        public StateMachine(
            System.Func<TStateEnum> loadState,
            System.Action<TStateEnum> persistState,
            System.Func<TStateEnum, IState<TStateEnum>> stateResolver,
            System.Func<TStateEnum, TStateEnum, bool> transitionValidator,
            ILogger logger)
        {
            this.loadState = loadState;
            this.persistState = persistState;
            this.stateResolver = stateResolver;   
            this.transitionValidator = transitionValidator;
            this.logger = logger;

            CurrentState = loadState();
            currentStateInstance = stateResolver(CurrentState);
        }

        public void UpdateState(double totalDays, EnumMonth month)
        {
            for (int i = 0; i < 10; i++)
            {
                var (nextState, transitionDays, transitionMonth) = currentStateInstance.UpdateState(totalDays, month);

                if (nextState!.Equals(CurrentState)) break;

                else TransitionState(nextState, transitionDays, transitionMonth);
            }
        }

        private void TransitionState(TStateEnum newState, double transitionDays, EnumMonth transitionMonth)
        {
            if (!transitionValidator(CurrentState, newState))
            {
                logger.Error($"Invalid transition {CurrentState} → {newState}");
                return;
            }

            currentStateInstance.ExitState();

            CurrentState = newState;
            persistState(CurrentState);

            currentStateInstance = stateResolver(CurrentState);
            currentStateInstance.EnterState(transitionDays, transitionMonth);
        }
    }
}