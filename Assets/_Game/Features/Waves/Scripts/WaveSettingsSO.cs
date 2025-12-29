using UnityEngine;

namespace ProjectGame.Features.Waves
{
    [CreateAssetMenu(fileName = "WaveSettings", menuName = "ProjectGame/Configs/Wave Settings")]
    public class WaveSettingsSO : ScriptableObject
    {
        [Header("Progression")] public int BaseAsteroidCount = 2;
        public float SpeedIncrementPerWave = 0.5f;

        [Header("Spawning")] public float SpawnBuffer = 2.0f; // Distance outside screen to spawn
    }
}