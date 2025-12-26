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
        [SerializeField] private int ScoreValue = 100;
        
        [Header("Architecture Dependencies")]
        [SerializeField] private Int CurrentPlayerScore; 
        [SerializeField] private GameEvent EnemyDestroyed;
        
        private Rigidbody2D _rb;
        private Action<Asteroid> _returnToPool;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            
            // Optimization: Asteroids usually just spin and drift
            _rb.linearDamping = 0; 
            _rb.angularDamping = 0;
        }

        public void Initialize(Vector2 position, float speed, Action<Asteroid> returnAction)
        {
            _returnToPool = returnAction;
            transform.position = position;

            // Randomize Rotation
            float randomAngle = Random.Range(0f, 360f);
            transform.rotation = Quaternion.Euler(0, 0, randomAngle);

            // Randomize Direction (Any random direction)
            Vector2 randomDir = Random.insideUnitCircle.normalized;
            
            // Physics
            _rb.linearVelocity = randomDir * speed;
            _rb.angularVelocity = Random.Range(-50f, 50f); // Spin
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: If hit by bullet -> Die and Score
            // TODO: If hit by player -> Game Over
        }
        
        public void TakeDamage(int amount)
        {
            Die();
        }

        private void Die()
        {
            if (CurrentPlayerScore != null)
            {
                CurrentPlayerScore.ApplyChange(ScoreValue); 
            }
            
            if (EnemyDestroyed != null)
            {
                EnemyDestroyed.Invoke();
            }
            
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