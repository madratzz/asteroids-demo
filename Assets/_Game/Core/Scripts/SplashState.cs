using System;
using System.Collections;
using ProjectCore.Events;
using ProjectCore.StateMachine;
using ProjectCore.Utilities;
using ProjectCore.Variables;
using ProjectGame.Core.Interfaces;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using VContainer;
using VContainer.Unity;

namespace ProjectCore
{
    [CreateAssetMenu(fileName = "SplashState", menuName = "ProjectCore/State Machine/States/Splash State")]
    public class SplashState : State
    {
        [Header("Assets (Addressables)")]
        [SerializeField] private AssetReferenceGameObject ApplicationFlowControllerReference;

        [Header("Configuration")] 
        [SerializeField] private int SceneIndex;
        [SerializeField] private float TimeoutDuration = 3f;

        [Header("Variables & Events")] 
        [SerializeField] private Float SceneLoadingProgress;
        [SerializeField] private GameEvent HideLoadingView;
        
        [NonSerialized] private AsyncOperation _sceneLoadingOperation;

        private ApplicationFlowController _flowControllerInstance;
        
        private const float SplashDelay = 0.5f;

        public override IEnumerator Init(IState listener)
        {
            yield return base.Init(listener);

            SceneLoadingProgress.SetValue(0);
        }

        public override IEnumerator Execute()
        {
            yield return base.Execute();
            
            yield return InstantiateApplicationFlowController();
            
            
            if (_flowControllerInstance == null)
            {
                Debug.LogError("[SplashState] Critical: Core systems failed to load.");
                yield break; 
            }

            //Start Loading the Game Scene
            yield return GameSceneLoading();
        }

        private object InstantiateApplicationFlowController()
        {
            return AddressablesHelper.InstantiateGameObject(
                ApplicationFlowControllerReference,
                (controller, handle) =>
                {
                    _flowControllerInstance = controller
                        .GetComponent<ApplicationFlowController>();
                    
                    var container = LifetimeScope.Find<RootLifetimeScope>().Container;
                    container.InjectGameObject(controller);
                }
            );
        }

        private IEnumerator GameSceneLoading()
        {
            // Artificial delay want the splash to linger
            yield return new WaitForSeconds(SplashDelay);

            _sceneLoadingOperation = SceneManager.LoadSceneAsync(SceneIndex, LoadSceneMode.Additive);
            if (_sceneLoadingOperation != null)
                _sceneLoadingOperation.allowSceneActivation = false; // Hold until we say go
        }

        public override IEnumerator Tick()
        {
            yield return base.Tick();

            //Monitor Scene Loading
            if (_sceneLoadingOperation != null)
            {
                // Scene loads to 0.9 then waits for activation
                SceneLoadingProgress.SetValue(_sceneLoadingOperation.progress / 0.9f);
            }

            //Wait for Scene Ready + SDKs + Minimum Time
            float timeStarted = Time.time;

            while (true)
            {
                bool isTimeOut = (Time.time - timeStarted) > TimeoutDuration;
                bool isSceneReady = _sceneLoadingOperation != null && _sceneLoadingOperation.progress >= 0.9f;

                // Exit condition: Timeout (maximum wait) OR Scene ready
                if (isTimeOut || isSceneReady)
                {
                    break;
                }

                yield return null;
            }

            SceneLoadingProgress.SetValue(1.0f);

            //Activate Scene
            if (_sceneLoadingOperation != null)
            {
                _sceneLoadingOperation.allowSceneActivation = true;
                yield return new WaitUntil(() => _sceneLoadingOperation.isDone);

                Scene scene = SceneManager.GetSceneByBuildIndex(SceneIndex);
                if (scene.IsValid())
                {
                    SceneManager.SetActiveScene(scene);
                }
            }

            //Cleanup & Boot
            HideLoadingView.Invoke();

            if (_flowControllerInstance != null)
            {
                _flowControllerInstance.Boot();
            }
        }
    }
}