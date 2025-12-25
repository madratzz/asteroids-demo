using System.Collections;
using ProjectCore.Utilities;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

namespace ProjectCore.GamePlay
{
    [CreateAssetMenu(fileName = "NormalGameState", menuName = "ProjectCore/State Machine/States/Normal Game State")]
    public class NormalGameState : GameState
    {
        //GameStateEnvironment
        [Header("NormalGameState Assets (Addressables)")]
        [SerializeField] private AssetReferenceGameObject LevelEnvironmentReference;
        [SerializeField] private AssetReferenceGameObject PlayerShipReference;
        
        private AsyncOperationHandle<GameObject> _playerHandle;
        private GameObject _playerInstance;
        
        public override IEnumerator Execute()
        {
            //Run Base (Loads HUD)
            yield return base.Execute();
            
            //Load the Gameplay Objects
            yield return InstantiateLevelObject();
            
            yield return InstantiatePlayerRoutine();

            //Start the Game Flow
            GameStateStart.Invoke();
            LogLevelStartedEvent();
            
        }

        private IEnumerator InstantiateLevelObject()
        {
            return AddressablesHelper.InstantiateGameObject(
                LevelEnvironmentReference,
                (levelObj, handle) =>
                {
                    _levelObject = levelObj; // Stored in Base Class
                    _levelHandle = handle;   // Stored in Base Class
                },
                ()=> Debug.LogWarning("Failed to load level"), 
                this
            );
        }
        
        private IEnumerator InstantiatePlayerRoutine()
        {
            Vector3 spawnPos = Vector3.zero;
            Quaternion spawnRot = Quaternion.identity;
            
            yield return AddressablesHelper.InstantiateGameObject(
                PlayerShipReference,
                (playerObj, handle) =>
                {
                    _playerInstance = playerObj;
                    _playerHandle = handle;
                    
                    _playerInstance.transform.SetPositionAndRotation(spawnPos, spawnRot);
                },
                () => Debug.LogError("Failed to load Player Ship!"),
                this
            );
        }

        private void LogLevelStartedEvent()
        {
            Debug.Log($"[NormalGameState] Level Started.");
        }
        
        protected override void ResetState()
        {
            base.ResetState();
            
            if (_playerHandle.IsValid())
            {
                Addressables.ReleaseInstance(_playerHandle);
            }
            _playerInstance = null;
        }
    }
}