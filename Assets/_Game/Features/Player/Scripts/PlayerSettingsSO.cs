using UnityEngine;

namespace ProjectGame.Features.Player.Configs
{
    [CreateAssetMenu(fileName = "PlayerSettings", menuName = "ProjectGame/Player/Settings")]
    public class PlayerSettingsSO : ScriptableObject
    {
        [Header("Movement")]
        public float ThrustForce = 10f;
        public float RotationSpeed = 180f;
        public float DragFactor = 1f; // Linear Drag

        [Header("Shooting")]
        public float FireRate = 0.25f;
        public float BulletSpeed = 20f;
    }
}