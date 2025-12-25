using ProjectGame.Features.Player.Configs;
using ProjectGame.Features.Player.Interfaces;
using UnityEngine;

namespace ProjectGame.Features.Player.Components
{
    [RequireComponent(typeof(Rigidbody2D))]
    public class PlayerMotor : MonoBehaviour
    {
        [SerializeField] private PlayerSettingsSO Settings;
        
        private Rigidbody2D _rb;
        private IPlayerInput _input;

        public void Initialize(IPlayerInput input, PlayerSettingsSO settings)
        {
            _input = input;
            if (settings != null) Settings = settings;
        }

        private void Awake()
        {
            _rb = GetComponent<Rigidbody2D>();
            _rb.gravityScale = 0; // Space Physics
        }

        private void FixedUpdate()
        {
            if (_input == null || Settings == null) return;

            HandleMovement();
            HandleRotation();
        }

        private void HandleMovement()
        {
            _rb.linearDamping = Settings.DragFactor;

            if (_input.IsThrusting)
            {
                // Force applied in the "Up" direction of the ship
                _rb.AddForce(transform.up * Settings.ThrustForce);
            }
        }

        private void HandleRotation()
        {
            // Negative sign because 'A' (-1) should rotate Left (Positive Z rotation)
            float rotateAmount = -_input.RotationInput * Settings.RotationSpeed * Time.fixedDeltaTime;
            _rb.MoveRotation(_rb.rotation + rotateAmount);
        }
    }
}