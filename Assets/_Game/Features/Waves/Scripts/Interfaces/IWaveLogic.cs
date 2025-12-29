namespace ProjectGame.Features.Waves.Logic
{
    public interface IWaveLogic
    {
        int CurrentWave { get; }
        int CalculateAsteroidCount(int waveIndex);
        float CalculateWaveSpeed(int waveIndex, float baseSpeed);
        int NextWave();
        void Reset();
    }
}