using VContainer;
using VContainer.Unity;

namespace ProjectCore
{
    // This runs automatically at game start if set up in Resources
    public class RootLifetimeScope : LifetimeScope
    {
        protected override void Configure(IContainerBuilder builder)
        {
            // Register the Logic here so it's available globally
            // Lifetime.Singleton = Global
            builder.Register<ApplicationFlowLogic>(Lifetime.Singleton);
        }
    }
}