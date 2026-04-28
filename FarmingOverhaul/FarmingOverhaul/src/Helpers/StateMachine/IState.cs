using Vintagestory.API.Common;

namespace FarmingOverhaul.src.Helpers.StateMachine
{
    public interface IState<TStateEnum>
    {
        TStateEnum State { get; }

        void EnterState(double transitionDays, EnumMonth transitionMonth);
        void ExitState();

        (TStateEnum nextState, double transitionDays, EnumMonth transitionMonth) UpdateState(double totalDays, EnumMonth month);
    }
}