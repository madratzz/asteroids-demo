using UnityEngine;

namespace ProjectGame.Features.Enemies.Logic
{
    public class WaveLogic
    {
        private int _currentWave = 0;
        public int CurrentWave => _currentWave;

        // Formula: Wave 1 = 3 asteroids, Wave 2 = 4, etc.
        public int CalculateAsteroidCount(int waveIndex)
        {
            return 2 + waveIndex;
        }

        // Formula: Speed increases by 0.5f per wave
        public float CalculateWaveSpeed(int waveIndex, float baseSpeed)
        {
            return baseSpeed + (waveIndex * 0.5f);
        }

        public int NextWave()
        {
            _currentWave++;
            return _currentWave;
        }
        
        public void Reset() => _currentWave = 0;
    }
}