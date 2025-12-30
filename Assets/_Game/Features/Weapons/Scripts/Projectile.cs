using System;
using ProjectGame.Core.Interfaces;
using UnityEngine;
using ProjectGame.Features.Weapons.Logic;

namespace ProjectGame.Features.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(ScreenWrap.ScreenWrap))] // Bullets wrap around the screen!
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private readonly ProjectileLogic _logic = new ProjectileLogic();
        
        private Action<Projectile> _returnToPool;
        
        private int _damageValue;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0; // Ensure no gravity affects the bullet
        }

        public void Initialize(Vector2 direction, float speed, float lifetime, int damage, Action<Projectile> returnAction)
        {
            _returnToPool = returnAction;
            _damageValue = damage;
            
            _logic.Initialize(Time.time, lifetime);
            
            // Reset Physics
            _rb.linearVelocity = Vector2.zero;
            _rb.angularVelocity = 0f;
            
            transform.up = direction;
            
            _rb.AddForce(direction * speed, ForceMode2D.Impulse);
        }

        private void Update()
        {
            if (_logic.IsExpired(Time.time))
            {
                Release();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.attachedRigidbody == null) return;
            if (!other.attachedRigidbody.TryGetComponent<IDamageable>(out var damageable)) return;
            
            damageable.TakeDamage(_damageValue);
            Release();
        }

        private void Release()
        {
            // Prevent "drifting" when respawned next time
            _rb.linearVelocity = Vector2.zero;
            _returnToPool?.Invoke(this); 
        }
    }
}