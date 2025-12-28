using ProjectCore.StateMachine;
using ProjectGame.Core.Interfaces;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ProjectCore
{
    // This runs automatically at game start if set up in Resources
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private FiniteStateMachine ApplicationStateMachine;
        protected override void Configure(IContainerBuilder builder)
        {
            // Register the Logic here so it's available globally
            // Lifetime.Singleton = Global and Only One Instance
            builder.Register<ApplicationFlowLogic>(Lifetime.Singleton).AsImplementedInterfaces();
            
            //StateMachine
            builder.RegisterInstance(ApplicationStateMachine).AsSelf();
        }
    }
}