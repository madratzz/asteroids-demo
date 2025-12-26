using System;
using UnityEngine;

namespace ProjectGame.Features.Weapons
{
    [RequireComponent(typeof(Rigidbody2D))]
    [RequireComponent(typeof(ScreenWrap.ScreenWrap))] // Bullets wrap around the screen!
    public class Projectile : MonoBehaviour
    {
        private Rigidbody2D _rb;
        private float _lifetime;
        private float _spawnTime;
        
        // The callback to return this specific bullet to the pool
        private Action<Projectile> _returnToPool;

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0; // Ensure no gravity affects the bullet
        }

        public void Initialize(Vector2 direction, float speed, float lifetime, Action<Projectile> returnAction)
        {
            _returnToPool = returnAction;
            _lifetime = lifetime;
            _spawnTime = Time.time;

            // Reset Physics (Crucial when reusing objects)
            _rb.linearVelocity = Vector2.zero; // Unity 6 API
            _rb.angularVelocity = 0f;

            // Set Position/Rotation logic is handled by the Spawner, 
            // but we align "Up" to the direction here
            transform.up = direction;

            // Fire!
            _rb.AddForce(direction * speed, ForceMode2D.Impulse);
        }

        private void Update()
        {
            // Self-Cleanup: If I live too long, return to pool
            if (Time.time >= _spawnTime + _lifetime)
            {
                Release();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            // TODO: Add damage logic here later (IDamageable)
            
            // For now, just disappear if we hit something that isn't the player
            // (We will add Layer checks later to prevent shooting yourself)
            Release();
        }

        private void Release()
        {
            // Reset velocity strictly to prevent "drifting" when respawned next time
            _rb.linearVelocity = Vector2.zero;
            
            // Call the pool to take me back
            _returnToPool?.Invoke(this); 
        }
    }
}