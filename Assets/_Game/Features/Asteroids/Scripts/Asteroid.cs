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
        [SerializeField] private int ScoreValue = 100;
        
        [Header("Architecture Dependencies")]
        [SerializeField] private Int CurrentPlayerScore; 
        [SerializeField] private GameEvent EnemyDestroyed;
        
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
        
        public void Initialize(Vector2 position, float speed, Action<Asteroid> returnAction, 
            Action<AsteroidSize, Vector3> splitAction)
        {
            _returnToPool = returnAction;
            _splitAction = splitAction;
            
            transform.position = position;

            // Randomize Rotation
            float randomAngle = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0, 0, randomAngle);

            // Randomize Direction
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            
            // Small asteroids move faster than large ones
            float speedMultiplier = (Size == AsteroidSize.Small) ? 1.5f : 1.0f;
            
            
            _rb.linearVelocity = randomDir * (speed * speedMultiplier);
            _rb.angularVelocity = Random.Range(-50f, 50f); // Spin
        }
        
        private void OnTriggerEnter2D(Collider2D other)
        {
            if (!other.attachedRigidbody.TryGetComponent(out IDamageable damageable)) return;
           
            if (!other.CompareTag(PlayerTag)) return;
            
            damageable.TakeDamage(1); 
            Die();
        }
        
        public void TakeDamage(int amount)
        {
            Die();
        }

        private void Die()
        {
            if (CurrentPlayerScore != null) CurrentPlayerScore.ApplyChange(ScoreValue);
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
    }
}