using ProjectGame.Features.Waves.Logic;

namespace ProjectGame.Features.Waves
{
    public class WaveLogic : IWaveLogic
    {
        private int _currentWave = 0;
        public int CurrentWave => _currentWave;
     
        private readonly WaveSettingsSO _settings;

        public WaveLogic(WaveSettingsSO settings)
        {
            _settings = settings;
        }

        // Formula: Wave 1 = 3 asteroids, Wave 2 = 4, etc.
        public int CalculateAsteroidCount(int waveIndex)
        {
            return _settings.BaseAsteroidCount + waveIndex;
        }

        // Formula: Speed increases by 0.5f per wave
        public float CalculateWaveSpeed(int waveIndex, float baseSpeed)
        {
            return baseSpeed + (waveIndex * _settings.SpeedIncrementPerWave);
        }

        public int NextWave()
        {
            _currentWave++;
            return _currentWave;
        }
        
        public void Reset() => _currentWave = 0;
    }
}