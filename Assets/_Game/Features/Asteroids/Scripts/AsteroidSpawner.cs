using UnityEngine;
using ProjectGame.Features.Enemies.Logic;

namespace ProjectGame.Features.Enemies
{
    public class AsteroidSpawner : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private AsteroidPool _pool;

        [Header("Config")]
        [SerializeField] private float _spawnRate = 2.0f;
        [SerializeField] private float _asteroidSpeed = 5.0f;
        [SerializeField] private float _spawnDistanceBuffer = 2.0f;

        private readonly AsteroidSpawnerLogic _logic = new AsteroidSpawnerLogic();
        private Camera _cam;
        private float _camHeight;
        private float _camWidth;

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
            if (_pool == null || !_pool.IsReady) return;
            
            if (_logic.ShouldSpawn(Time.time, _spawnRate))
            {
                Spawn();
            }
        }

        private void Spawn()
        {
            // Ask the Brain: "Where should it go?"
            Vector2 spawnPos = _logic.GetRandomSpawnPosition(_camHeight, _camWidth, _spawnDistanceBuffer);

            // Get from Pool
            Asteroid asteroid = _pool.Get();
            
            // Initialize
            asteroid.Initialize(spawnPos, _asteroidSpeed, _pool.Release);
        }
    }
}