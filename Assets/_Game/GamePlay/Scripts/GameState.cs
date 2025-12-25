using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using ProjectCore.Events;
using ProjectCore.Hud;
using ProjectCore.StateMachine;
using ProjectCore.Utilities;

namespace ProjectCore.GamePlay
{
    public class GameState : State
    {
        [Header("Assets (Addressables)")]
        [SerializeField] protected AssetReferenceGameObject GameHudReference;

        [Header("Events")]
        [SerializeField] protected GameEvent GameStateStart;
        [SerializeField] protected GameEvent GameStateResume;
        [SerializeField] protected GameEvent GameStateExit;
        [SerializeField] protected GameEvent GameStatePause;
        
        [SerializeField] protected GameEvent LevelFail;
        [SerializeField] protected GameEvent LevelComplete;
        [SerializeField] protected GameEvent ShowBannerAd;
        [SerializeField] protected GameEvent HideBannerAd;
        
        // Runtime Handles (To track memory)
        protected AsyncOperationHandle<GameObject> _hudHandle;
        protected AsyncOperationHandle<GameObject> _levelHandle;
        
        [NonSerialized] protected GameHud _gameHud;
        [NonSerialized] protected GameObject _levelObject;
        
        [NonSerialized] protected bool _failReported = false;
        
        public override IEnumerator Init(IState listener)
        {
            yield return base.Init(listener);
            yield return listener.CleanupAllPausedStates(this);
            ResetState();
        }

        public override IEnumerator Execute()
        {
            ShowLevelObjects();
            
            yield return base.Execute();
            
            yield return AddressablesHelper.Instantiate<GameHud>(GameHudReference, (hud, handle) => 
            {
                _gameHud = hud;
                _hudHandle = handle; // Important: Capture handle for cleanup
                _gameHud.Show();
            },
                null,
                this);
        }
        
        public override IEnumerator Resume()
        {
            yield return base.Resume();
            GameStateResume.Invoke();
            ShowLevelObjects();
        }

        public override IEnumerator Pause()
        {
            yield return base.Pause();
            GameStatePause.Invoke();
            HideLevelObjects();
        }

        public override IEnumerator Exit()
        {
            GameStateExit.Invoke();
            
            // Ideally, wait for the HUD to finish its Hide animation callback.
            if (_gameHud != null)
            {
                yield return _gameHud.Hide().WaitForCompletion(); 
            }

            ResetState();
            yield return base.Exit();
        }

        public override IEnumerator Cleanup()
        {
            ResetState();
            yield return base.Cleanup();
        }

        protected virtual void ResetState()
        {
            //Release Addressable Memory
            if (_hudHandle.IsValid())
            {
                Addressables.ReleaseInstance(_hudHandle);
            }
            _gameHud = null;

            if (_levelHandle.IsValid())
            {
                Addressables.ReleaseInstance(_levelHandle);
            }
            _levelObject = null;
        }

        protected virtual void ShowLevelObjects()
        {
            if (_gameHud != null) 
                _gameHud.Show();
            if (_levelObject != null) 
                _levelObject.SetActive(true);
        }

        protected virtual void HideLevelObjects()
        {
            if (_gameHud != null) 
                _gameHud.Hide();
            if (_levelObject != null) 
                _levelObject.SetActive(false);
        }

    }
}