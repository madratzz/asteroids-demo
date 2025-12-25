using System.Collections;
using UnityEngine;

using ProjectCore.StateMachine;

using ProjectCore.UI;

namespace ProjectCore.UI
{
    [CreateAssetMenu(fileName = "ViewTransition", menuName = "ProjectCore/State Machine/Transitions/View Transitions")]
    public class UIViewTransition : Transition
    {
        // Runtime data passed from the Manager
        [System.NonSerialized] public UICloseReasons CloseReason; //Default or Runtime Close Reason
        [System.NonSerialized] public Camera Camera;

        public override IEnumerator Execute()
        { 
            // Pass the runtime data to the specific UI State
            if (ToState is UIViewState targetViewState)
            {
                targetViewState.ViewSourceCloseReason = CloseReason;
                targetViewState.RenderCamera = Camera;
            }
            
            yield return base.Execute();

            CloseReason = default;
            Camera = null;
        }
    }
}