using UnityEngine;
using ProjectGame.Features.Enemies.Logic;
using UnityEngine.Serialization;

namespace ProjectGame.Features.Enemies
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [Header("Pools")]
        [SerializeField] private AsteroidPool LargePool;
        [SerializeField] private AsteroidPool MediumPool;
        [SerializeField] private AsteroidPool SmallPool;

        
        [Header("Wave Config")]
        [SerializeField] private float BaseSpeed = 1.0f;
        [SerializeField] private float SpawnBuffer = 2.0f;

        private int _activeAsteroidCount = 0;
        private bool _isSpawning;
        
        private readonly WaveLogic _waveLogic = new WaveLogic();
        private readonly AsteroidSpawnerLogic _spawnLogic = new AsteroidSpawnerLogic();
        
        private Camera _cam;
        
        private float _camHeight;
        private float _camWidth;

        private void OnEnable()
        {
            // Reset logic when Game State starts
            _waveLogic.Reset();
            _activeAsteroidCount = 0;
            StartNextWave();
        }
        
        private void Awake()
        {
            _cam = Camera.main;
            CalculateScreenBounds();
        }

        private void CalculateScreenBounds()
        {
            if (_cam == null) return;
            _camHeight = _cam.orthographicSize;
            _camWidth = _camHeight * _cam.aspect;
        }

        private void Update()
        {
            if (_isSpawning || _activeAsteroidCount > 0) return;
            if (!LargePool.IsReady) return;
            
            Debug.Log($"Wave {_waveLogic.CurrentWave} Complete. Starting Next...");
            StartNextWave();
        }
        
        private void StartNextWave()
        {
            _isSpawning = true;
            
            int wave = _waveLogic.NextWave();
            int count = _waveLogic.CalculateAsteroidCount(wave);
            float speed = _waveLogic.CalculateWaveSpeed(wave, BaseSpeed);

            for (int i = 0; i < count; i++)
            {
                SpawnAsteroid(AsteroidSize.Large, GetRandomEdgePosition(), speed);
            }

            _isSpawning = false;
        }

        private void SpawnAsteroid(AsteroidSize size, Vector2 position, float speed)
        {
            // 1. Select Pool
            AsteroidPool pool = size switch
            {
                AsteroidSize.Large => LargePool,
                AsteroidSize.Medium => MediumPool,
                AsteroidSize.Small => SmallPool,
                _ => LargePool
            };

            if (pool == null || !pool.IsReady) return;

            // 2. Spawn & Init
            Asteroid asteroid = pool.Get();
            
            // Pass 'HandleSplit' so the asteroid can tell us when it died
            asteroid.Initialize(position, speed, pool.Release, HandleSplit);

            _activeAsteroidCount++;
        }
        
        private void HandleSplit(AsteroidSize size, Vector3 position)
        {
            _activeAsteroidCount--; // Parent died

            // Determine Children
            AsteroidSize? childSize = null;
            if (size == AsteroidSize.Large) childSize = AsteroidSize.Medium;
            else if (size == AsteroidSize.Medium) childSize = AsteroidSize.Small;

            // Spawn Children
            if (childSize.HasValue)
            {
                float speed = _waveLogic.CalculateWaveSpeed(_waveLogic.CurrentWave, BaseSpeed);
                // Spawn 2 children
                SpawnAsteroid(childSize.Value, position, speed);
                SpawnAsteroid(childSize.Value, position, speed);
            }
        }
        
        private Vector2 GetRandomEdgePosition()
        {
            return _spawnLogic.GetRandomSpawnPosition(_camHeight, _camWidth, SpawnBuffer);
        }
    }
}