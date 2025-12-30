using ProjectCore.Events;
using ProjectCore.StateMachine;
using ProjectGame.Features.Enemies;
using ProjectGame.Features.Waves;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ProjectCore
{
    // This runs automatically at game start if set up in Resources
    public class RootLifetimeScope : LifetimeScope
    {
        [SerializeField] private FiniteStateMachine ApplicationStateMachine;
        [SerializeField] private GameEvent TimeMachineTick;
        
        [Header("Configs")]
        [SerializeField] private WaveSettingsSO WaveSettings;
        [SerializeField] private AsteroidSettingsSO AsteroidSettings;
        
        
        protected override void Configure(IContainerBuilder builder)
        {
            // Register the Logic here so it's available globally
            // Lifetime.Singleton = Global and Only One Instance
            builder.Register<ApplicationFlowLogic>(Lifetime.Singleton).AsImplementedInterfaces();
            //StateMachine
            builder.RegisterInstance(ApplicationStateMachine).AsSelf();
            
            //Register TimeMachineTick GameEvent with a key
            builder.RegisterInstance(TimeMachineTick).AsSelf().Keyed("TimeMachineTick");
            
            //Configs
            builder.RegisterInstance(WaveSettings).AsSelf();
            builder.RegisterInstance(AsteroidSettings).AsSelf();
        }
    }
}