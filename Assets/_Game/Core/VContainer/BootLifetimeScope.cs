using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ProjectCore
{
    public class BootLifetimeScope : LifetimeScope
    {
        [SerializeField] private ApplicationBase ApplicationBase;

        protected override void Configure(IContainerBuilder builder)
        {
            // Register the specific instance already sitting in your scene
            builder.RegisterComponent(ApplicationBase);
        }
    }
}