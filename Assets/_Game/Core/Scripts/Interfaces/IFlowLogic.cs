using ProjectCore;
using ProjectCore.UI;

namespace ProjectGame.Core.Interfaces
{
    public interface IFlowLogic
    {
        FlowIntent GetDecision(FlowContext context, UICloseReasons reason);
    }
}