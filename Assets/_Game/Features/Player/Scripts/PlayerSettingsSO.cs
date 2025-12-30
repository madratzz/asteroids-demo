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
        public float BulletLifetime = 1f;
        public int Damage = 1; //Not Used but wanted to Extract in SO
        
        [Header("Repawn")]
        public float RespawnDelay = 2f;
    }
}