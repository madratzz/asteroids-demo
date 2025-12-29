using System;
using ProjectCore.Events;
using ProjectCore.Variables;
using ProjectGame.Core.Interfaces;
using UnityEngine;
using Random = UnityEngine.Random;

namespace ProjectGame.Features.Enemies
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(ScreenWrap.ScreenWrap))]
    public class Asteroid : MonoBehaviour, IDamageable
    {
        [Header("Config")]
        [SerializeField] private AsteroidSize Size;
        
        [Header("Architecture Dependencies")]
        [SerializeField] private Int CurrentPlayerScore; 
        [SerializeField] private GameEvent EnemyDestroyed;
        
        private int _asteroidScoreValue;
        private Rigidbody2D _rb;
        private Action<Asteroid> _returnToPool;
        private Action<AsteroidSize, Vector3> _splitAction;

        private const string PlayerTag = "Player";

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            
            // Optimization: Asteroids usually just spin and drift
            _rb.linearDamping = 0; 
            _rb.angularDamping = 0;
        }
        
        public void Initialize(Vector2 position, AsteroidSize size, 
            AsteroidSettingsSO settings, float baseSpeed, Action<Asteroid> returnAction, 
            Action<AsteroidSize, Vector3> splitAction )
        {
            Size = size;
            _asteroidScoreValue = settings.GetScore(size);
            _returnToPool = returnAction;
            _splitAction = splitAction;
            transform.position = position;

            RandomizeAsteroidRotation();
            float speedMultiplier = CalculateSpeedMultiplier(settings);
            SetRandomMovement(settings, baseSpeed, speedMultiplier);
        }
        
        public void TakeDamage(int amount)
        {
            Die();
        }

        private void Die()
        {
            if (CurrentPlayerScore != null) CurrentPlayerScore.ApplyChange(_asteroidScoreValue);
            if (EnemyDestroyed != null) EnemyDestroyed.Invoke();
            
            _splitAction?.Invoke(Size, transform.position);
            Release();
        }

        public void Release()
        {
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0;
            _returnToPool?.Invoke(this);
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.attachedRigidbody.TryGetComponent(out IDamageable damageable)) return;
           
            if (!other.CompareTag(PlayerTag)) return;
            
            damageable.TakeDamage(1); 
            Die();
        }
        
        
        private float CalculateSpeedMultiplier(AsteroidSettingsSO settings)
        {
            // Small asteroids move faster than large ones
            float speedMultiplier = (Size == AsteroidSize.Small) 
                ? settings.SmallSpeedMultiplier 
                : settings.NormalSpeedMultiplier;
            return speedMultiplier;
        }

        private void RandomizeAsteroidRotation()
        {
            // Randomize Rotation
            float randomAngle = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0, 0, randomAngle);
        }

        private void SetRandomMovement(AsteroidSettingsSO settings, float baseSpeed, float speedMultiplier)
        {
            // Randomize Direction
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            _rb.linearVelocity = randomDir * (baseSpeed * speedMultiplier);
            _rb.angularVelocity = Random.Range(settings.MinSpin, settings.MaxSpin); // Spin
        }
    }
}