using System.Collections.Generic;
using ProjectCore.UI;

namespace ProjectCore
{
    public class ApplicationFlowLogic
    {
        // Key: (Context + Reason) -> Value: Intent
        private readonly Dictionary<(FlowContext, UICloseReasons), FlowIntent> _strategies;

        public ApplicationFlowLogic()
        {
            _strategies = new Dictionary<(FlowContext, UICloseReasons), FlowIntent>();
            InitializeStrategies();
        }

        private void InitializeStrategies()
        {
            // --- LEVEL FAIL CONTEXT ---
            Add(FlowContext.LevelFail, UICloseReasons.Game,      FlowIntent.GoToGame);
        }

        // Helper to keep the dictionary cleaner
        private void Add(FlowContext ctx, UICloseReasons reason, FlowIntent intent)
        {
            _strategies[(ctx, reason)] = intent;
        }

        /// <summary>
        /// The Pure Function: Takes inputs, returns decision.
        /// </summary>
        public FlowIntent GetDecision(FlowContext context, UICloseReasons reason)
        {
            return _strategies.GetValueOrDefault((context, reason), FlowIntent.DefaultToGame);
        }
    }
    
    // The "What" (The Output of the Brain)
    public enum FlowIntent
    {
        None = 0,               // Default/Uninitialized catch
        DefaultToGame = 1,  // Fallback
    
        // Navigation (100 range)
        //GoToMainMenu   = 100,
        GoToGame       = 101,
        GoToLevelFail  = 102,
    
        // Logic Actions (200 range)
        ResumePrevious = 200
    }

    // The "Where" (The Context of the Event)
    public enum FlowContext
    {
        None      = 0,
        //MainMenu  = 1,
        LevelFail = 2
    }
}