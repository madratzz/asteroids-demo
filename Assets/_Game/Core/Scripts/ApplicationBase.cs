using System;
using ProjectCore.Events;
using ProjectCore.StateMachine;
using ProjectCore.Variables;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace ProjectCore
{
    public class ApplicationBase : MonoBehaviour
    {
        [SerializeField] private int IOSTargetFrameRate = 60;
        [SerializeField] private int AndroidTargetFrameRate = 60;
        [SerializeField] private TimeMachine.TimeMachine ApplicationTimeMachine;

        [SerializeField] private GameEvent AppPaused;
        [SerializeField] private GameEvent AppResumed;
        [SerializeField] private DBInt AppPausedTime;
        
        private FiniteStateMachine _applicationStateMachine;
        private Coroutine _stateMachineRoutine;
        private Coroutine _timeMachineRoutine;
        private bool _appPaused = false;
        
        [Inject]
        public void Construct(FiniteStateMachine stateMachine)
        {
            _applicationStateMachine = stateMachine;
        }
        
        #region Life Cycle


        private void Start()
        {
            Application.targetFrameRate = Application.platform switch
            {
                RuntimePlatform.Android => AndroidTargetFrameRate,
                RuntimePlatform.IPhonePlayer => IOSTargetFrameRate,
                _ => Application.targetFrameRate
            };

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            if (ApplicationTimeMachine != null) 
                _timeMachineRoutine = StartCoroutine(ApplicationTimeMachine.Tick());
            
            if (_applicationStateMachine != null)
                _stateMachineRoutine = StartCoroutine(_applicationStateMachine.Tick());
            
            // Initialize Firebase and Ads Here
        }

        private void OnApplicationFocus(bool focus)
        {
            if (focus)
            {
                ApplicationResumed();
            }
            else
            {
                ApplicationPaused();
            }
        }

        private void OnApplicationPause(bool paused)
        {
            if (paused)
            {
                ApplicationPaused();
            }
            else
            {
                ApplicationResumed();
            }
        }

        private void OnApplicationQuit()
        {
            ApplicationPaused();
        }

        #endregion

        private void ApplicationPaused()
        {
            if (_appPaused) return;
            
            _appPaused = true;
            AppPausedTime.SetValue((int)DateTimeOffset.Now.ToUnixTimeSeconds());
            AppPaused.Invoke();
        }

        private void ApplicationResumed()
        {
            if (!_appPaused) return;
            
            _appPaused = false;
            AppResumed.Invoke();
        }
    }
}
