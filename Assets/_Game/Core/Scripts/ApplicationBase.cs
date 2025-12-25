using System;
using ProjectCore.Events;
using ProjectCore.StateMachine;
using ProjectCore.Variables;
using UnityEngine;

namespace ProjectCore
{
    public class ApplicationBase : MonoBehaviour
    {
        [SerializeField] private int IOSTargetFrameRate = 60;
        [SerializeField] private int AndroidTargetFrameRate = 60;

        [SerializeField] private FiniteStateMachine ApplicationStateMachine;
        [SerializeField] private TimeMachine.TimeMachine ApplicationTimeMachine;

        [SerializeField] private GameEvent AppPaused;
        [SerializeField] private GameEvent AppResumed;
        [SerializeField] private DBInt AppPausedTime;


        private Coroutine _stateMachineRoutine;
        private Coroutine _timeMachineRoutine;
        private bool _appPaused = false;

        #region Life Cycle

        // Start is called before the first frame update
        private void Start()
        {
            Application.targetFrameRate = Application.platform switch
            {
                RuntimePlatform.Android => AndroidTargetFrameRate,
                RuntimePlatform.IPhonePlayer => IOSTargetFrameRate,
                _ => Application.targetFrameRate
            };

            Screen.sleepTimeout = SleepTimeout.NeverSleep;

            _timeMachineRoutine = StartCoroutine(ApplicationTimeMachine.Tick());
            _stateMachineRoutine = StartCoroutine(ApplicationStateMachine.Tick());
            
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
