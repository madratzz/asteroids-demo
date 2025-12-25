using ProjectGame.Features.Player.Components;
using ProjectGame.Features.Player.Configs;
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

        private void Awake()
        {
            if (Motor == null) Motor = GetComponent<PlayerMotor>();
            if (InputReader == null) InputReader = GetComponent<InputSystemReader>();
            
            Motor.Initialize(InputReader, Settings);
        }
    }
}