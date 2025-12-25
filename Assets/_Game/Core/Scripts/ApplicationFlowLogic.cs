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
            // --- MAIN MENU CONTEXT ---
            Add(FlowContext.MainMenu, UICloseReasons.Game,     FlowIntent.GoToGame);

            // --- LEVEL FAIL CONTEXT ---
            Add(FlowContext.LevelFail, UICloseReasons.Home,      FlowIntent.GoToMainMenu);
            Add(FlowContext.LevelFail, UICloseReasons.Game,      FlowIntent.GoToGame);
            Add(FlowContext.LevelFail, UICloseReasons.Revive,    FlowIntent.ResumePrevious);
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
            return _strategies.GetValueOrDefault((context, reason), FlowIntent.DefaultToMainMenu);
        }
    }
    
    // The "What" (The Output of the Brain)
    public enum FlowIntent
    {
        None = 0,               // Default/Uninitialized catch
        DefaultToMainMenu = 1,  // Fallback
    
        // Navigation (100 range)
        GoToMainMenu   = 100,
        GoToGame       = 101,
        GoToLevelFail  = 102,
    
        // Logic Actions (200 range)
        ResumePrevious = 200
    }

    // The "Where" (The Context of the Event)
    public enum FlowContext
    {
        None      = 0,
        MainMenu  = 1,
        LevelFail = 2
    }
}