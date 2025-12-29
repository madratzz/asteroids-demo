using UnityEngine;

namespace ProjectGame.Features.Enemies.Logic
{
    public class WaveSpawnerLogic
    {
        private float _nextSpawnTime;

        // Timer Logic
        public bool ShouldSpawn(float currentTime, float spawnRate)
        {
            if (currentTime >= _nextSpawnTime)
            {
                _nextSpawnTime = currentTime + spawnRate;
                return true;
            }
            return false;
        }

        // Position Logic
        // Calculates a random point OUTSIDE the screen bounds so asteroids drift in naturally
        public Vector2 GetRandomSpawnPosition(float camHeight, float camWidth, float buffer = 1.0f)
        {
            // Pick a random edge: 0=Top, 1=Right, 2=Bottom, 3=Left
            int edge = Random.Range(0, 4);
            
            float x = 0;
            float y = 0;

            switch (edge)
            {
                case 0: // Top
                    x = Random.Range(-camWidth, camWidth);
                    y = camHeight + buffer;
                    break;
                case 1: // Right
                    x = camWidth + buffer;
                    y = Random.Range(-camHeight, camHeight);
                    break;
                case 2: // Bottom
                    x = Random.Range(-camWidth, camWidth);
                    y = -camHeight - buffer;
                    break;
                case 3: // Left
                    x = -camWidth - buffer;
                    y = Random.Range(-camHeight, camHeight);
                    break;
            }

            return new Vector2(x, y);
        }
    }
}