using System.Collections.Generic;
using UnityEngine;
using ProjectGame.Features.Enemies;

namespace ProjectGame.Features.Enemies
{
    [CreateAssetMenu(fileName = "AsteroidSettings", menuName = "ProjectGame/Configs/Asteroid Settings")]
    public class AsteroidSettingsSO : ScriptableObject
    {
        [Header("Movement")] public float BaseSpeed = 1.0f;
        public float SmallSpeedMultiplier = 1.5f;
        public float NormalSpeedMultiplier = 1.0f;

        [Header("Physics")] public float MinSpin = -50f;
        public float MaxSpin = 50f;

        [Header("Scoring")] public int ScoreLarge = 50;
        public int ScoreMedium = 100;
        public int ScoreSmall = 200;

        [Header("Split Logic")] [SerializeField]
        private List<SplitRule> SplitRules;

        public int GetScore(AsteroidSize size)
        {
            return size switch
            {
                AsteroidSize.Large => ScoreLarge,
                AsteroidSize.Medium => ScoreMedium,
                AsteroidSize.Small => ScoreSmall,
                _ => 0
            };
        }

        public bool TryGetSplitRule(AsteroidSize size, out AsteroidSize childSize, out int count)
        {
            // Simple linear search is fine for 3-4 items. 
            // For larger lists, convert to Dictionary in OnEnable().
            foreach (SplitRule rule in SplitRules)
            {
                if (rule.Source != size) continue;

                childSize = rule.Child;
                count = rule.Count;
                return true;
            }

            childSize = default;
            count = 0;
            return false;
        }


        [System.Serializable]
        private struct SplitRule
        {
            public AsteroidSize Source;
            public AsteroidSize Child;
            [Min(1)] public int Count;
        }
    }
}