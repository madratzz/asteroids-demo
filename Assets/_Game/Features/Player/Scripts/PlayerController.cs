using System.Collections;
using ProjectCore.Variables;
using ProjectGame.Core.Interfaces;
using ProjectGame.Features.Player.Components;
using ProjectGame.Features.Player.Configs;
using ProjectGame.Features.Weapons;
using UnityEngine;

namespace ProjectGame.Features.Player.Controllers
{
    public class PlayerController : MonoBehaviour, IDamageable
    {
        [Header("Configuration")]
        [SerializeField] private PlayerSettingsSO Settings;
        [SerializeField] private float RespawnDelay = 2.0f;
        
        [SerializeField] private Int PlayerLives;

        [Header("Components")]
        [SerializeField] private PlayerMotor Motor;
        [SerializeField] private InputSystemReader InputReader;
        [SerializeField] private WeaponController WeaponSystem;
        [SerializeField] private GameObject VisualsRoot;
        
        private bool _isRespawning = false;

        private void Awake()
        {
            if (Motor == null) Motor = GetComponent<PlayerMotor>();
            if (InputReader == null) InputReader = GetComponent<InputSystemReader>();
            if (WeaponSystem == null) WeaponSystem = GetComponent<WeaponController>();
            
            Motor.Initialize(InputReader, Settings);
            WeaponSystem.Initialize(InputReader, Settings);
            
            PlayerLives.ResetToDefaultValue();
        }

        public void TakeDamage(int amount)
        {
            if (_isRespawning) return;
            
            StartCoroutine(RespawnRoutine(amount));
        }
        
        private IEnumerator RespawnRoutine(int amount)
        {
            _isRespawning = true;
            
            SetPlayerActive(false);
            
            yield return new WaitForSeconds(RespawnDelay);

            if (PlayerLives == null) yield break;
            
            PlayerLives.ApplyChange(-amount);
            Debug.Log($"Lives Remaining: {PlayerLives.GetValue()}");
            
            if (PlayerLives > 0)
            {
                Respawn();
            }
            else
            {
                SetPlayerActive(false);
            }
        }
        
        private void Respawn()
        {
            ResetPlayerPosition();
            ResetPhysics();
            
            SetPlayerActive(true);
            
            _isRespawning = false;
        }

        private void ResetPlayerPosition()
        {
            transform.position = Vector3.zero;
            transform.rotation = Quaternion.identity;
        }

        private void ResetPhysics()
        {
            if (!TryGetComponent(out Rigidbody2D rb)) return;
            
            rb.linearVelocity = Vector2.zero;
            rb.angularVelocity = 0f;
        }

        private void SetPlayerActive(bool isActive)
        {
            if (VisualsRoot) VisualsRoot.SetActive(isActive);
            if (Motor) Motor.enabled = isActive;
            if (WeaponSystem) WeaponSystem.enabled = isActive;
        }
    }
}