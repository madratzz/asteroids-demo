using UnityEngine;

namespace ProjectGame.Features.Player.Interfaces
{
    public interface IPlayerInput
    {
        float RotationInput { get; } // -1 (Left) to 1 (Right)
        bool IsThrusting { get; }    // True if pressing Forward
        bool IsFiring { get; }       // True if pressing Fire
    }
}