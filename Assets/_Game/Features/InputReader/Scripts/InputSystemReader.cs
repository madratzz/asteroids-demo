using UnityEngine;
using ProjectGame.Features.Player.Interfaces;

namespace ProjectGame.Features.Player.Components
{
    public class InputSystemReader : MonoBehaviour, IPlayerInput
    {
        private GameControls _controls;
        
        // Cache the values
        private Vector2 _moveInput;
        private bool _fireInput;

        public float RotationInput => _moveInput.x;
        // Check if Y is positive
        public bool IsThrusting => _moveInput.y > 0.1f; 
        public bool IsFiring => _fireInput;

        private void Awake()
        {
            _controls = new GameControls();
            
            _controls.Gameplay.Move.performed += ctx => _moveInput = ctx.ReadValue<Vector2>();
            _controls.Gameplay.Move.canceled += ctx => _moveInput = Vector2.zero;

            _controls.Gameplay.Fire.performed += ctx => _fireInput = true;
            _controls.Gameplay.Fire.canceled += ctx => _fireInput = false;
        }

        private void OnEnable() => _controls.Enable();
        private void OnDisable() => _controls.Disable();
    }
}