namespace ProjectCore
{
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
}