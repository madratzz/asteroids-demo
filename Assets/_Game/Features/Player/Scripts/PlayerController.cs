using ProjectGame.Features.Player.Components;
using ProjectGame.Features.Player.Configs;
using ProjectGame.Features.Weapons;
using UnityEngine;

namespace ProjectGame.Features.Player.Controllers
{
    public class PlayerController : MonoBehaviour
    {
        [Header("Configuration")]
        [SerializeField] private PlayerSettingsSO Settings;

        [Header("Components")]
        [SerializeField] private PlayerMotor Motor;
        [SerializeField] private InputSystemReader InputReader;
        [SerializeField] private WeaponController WeaponSystem;

        private void Awake()
        {
            if (Motor == null) Motor = GetComponent<PlayerMotor>();
            if (InputReader == null) InputReader = GetComponent<InputSystemReader>();
            if (WeaponSystem == null) WeaponSystem = GetComponent<WeaponController>();
            
            Motor.Initialize(InputReader, Settings);
            WeaponSystem.Initialize(InputReader, Settings);
        }
    }
}