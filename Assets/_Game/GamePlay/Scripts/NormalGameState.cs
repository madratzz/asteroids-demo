using System.Collections;
using ProjectCore.Events;
using ProjectCore.Utilities;
using ProjectCore.Variables;
using ProjectGame.Features.Enemies;
using ProjectGame.Features.Enemies.Logic;
using ProjectGame.Features.Waves;
using ProjectGame.Features.Waves.Logic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VContainer;
using VContainer.Unity;

namespace ProjectCore.GamePlay
{
    [CreateAssetMenu(fileName = "NormalGameState", menuName = "ProjectCore/State Machine/States/Normal Game State")]
    public class NormalGameState : GameState
    {
        //GameStateEnvironment
        [Header("Scene Assets")]
        [SerializeField] private AssetReferenceGameObject LevelEnvironmentReference;
        
        [Header("Gameplay Actors")]
        [SerializeField] private AssetReferenceGameObject PlayerShipReference;
        [SerializeField] private AssetReferenceGameObject AsteroidSpawnerReference;
        
        [Header("Game State Data")]
        [SerializeField] private Int CurrentPlayerScore;
        [SerializeField] private Int PlayerLives;
        
        [SerializeField] private GameEvent GotoLevelFail;
        
        private LifetimeScope _gameplayScope;
        
        private AsyncOperationHandle<GameObject> _playerHandle;
        private GameObject _playerInstance;
        
        private AsyncOperationHandle<GameObject> _asteroidSpawnerHandle;
        private WaveSpawner _waveSpawnerInstance;
        
        public override IEnumerator Execute()
        {
            if (CurrentPlayerScore == null)
            {
                Debug.LogError("[NormalGameState] Critical: CurrentPlayerScore not found!");
                yield break;
            } 
            
            CurrentPlayerScore.SetValue(0);
            
            //Run Base (Loads HUD)
            yield return base.Execute();

            CreateGameplayScope();
            
            //Load the Gameplay Objects
            yield return InstantiateLevelObject();
            yield return InstantiatePlayerRoutine();
            
            
            //yield return InstantiateAsteroidSpawnerRoutine();
            yield return InstantiateAsteroidSpawnerViaVContainer();
            
            //Start the Game Flow
            GameStateStart.Invoke();
            LogLevelStartedEvent();
        }
        
        public override IEnumerator Tick()
        {
            yield return base.Tick();

            if (PlayerLives<=0)
            {
                GotoLevelFail.Invoke();
            }
        }
        
        protected override void ResetState()
        {
            base.ResetState();
            
            if (_gameplayScope != null)
            {
                _gameplayScope.Dispose();
                _gameplayScope = null;
            }
            
            if (_playerHandle.IsValid())
                Addressables.ReleaseInstance(_playerHandle);
            _playerInstance = null;
            
            if (_waveSpawnerInstance != null)
            {
                Destroy(_waveSpawnerInstance.gameObject);
                _waveSpawnerInstance = null;
            }
            
            if (_asteroidSpawnerHandle.IsValid())
                Addressables.ReleaseInstance(_asteroidSpawnerHandle);
            _waveSpawnerInstance = null;
        }
        
        private void CreateGameplayScope()
        {
            // Find the Root Scope (The bridge between SO and VContainer)
            var rootScope = LifetimeScope.Find<RootLifetimeScope>();
            
            if (rootScope == null)
            {
                Debug.LogError("[NormalGameState] Critical: RootLifetimeScope not found in scene!");
                return;
            }

            // Create a Child Scope for this Level
            _gameplayScope = rootScope.CreateChild(builder =>
            {
                // Register Scoped Logic (Created fresh for this game session)
                builder.Register<WaveLogic>(Lifetime.Scoped).As<IWaveLogic>();
                builder.Register<WaveSpawnerLogic>(Lifetime.Scoped);
                
                // Note: Configs (WaveSettings/AsteroidSettings) are inherited from Root automatically!
            });
        }
        
        private IEnumerator InstantiateAsteroidSpawnerViaVContainer()
        {
            // Load the PREFAB (Data), don't instantiate it yet
            _asteroidSpawnerHandle = Addressables.LoadAssetAsync<GameObject>(AsteroidSpawnerReference);
            
            while (!_asteroidSpawnerHandle.IsDone) yield return null;

            if (_asteroidSpawnerHandle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject prefab = _asteroidSpawnerHandle.Result;

                // Let VContainer Instantiate it
                // This automatically finds the [Inject] method on AsteroidSpawner
                // and fills it with the Logic we registered in CreateGameplayScope()
                _waveSpawnerInstance = _gameplayScope.Container.Instantiate(prefab)
                    .GetComponent<WaveSpawner>();
            }
            else
            {
                Debug.LogError("Failed to load Asteroid Spawner Prefab!");
            }
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
        
        
    }
}