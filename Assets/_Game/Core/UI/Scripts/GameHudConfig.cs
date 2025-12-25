using DG.Tweening;
using UnityEngine;

namespace ProjectCore.Hud
{
    [CreateAssetMenu(fileName = "GameHudConfig", menuName = "ProjectCore/UI/Configs/GameHudConfig")]
    public class GameHudConfig : ScriptableObject
    {
        public float AnimationDuration;
        public Ease ShowEase = Ease.OutBack;
        public Ease HideEase = Ease.InCubic;
    
        public float HeaderOffScreenPosition;
        public float FooterOffScreenPosition;
    }
}