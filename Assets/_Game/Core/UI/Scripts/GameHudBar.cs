using CustomUtilities.Attributes;
using UnityEngine;
using DG.Tweening;

namespace ProjectCore.Hud
{
    public class GameHudBar : MonoBehaviour
    {
        [SerializeField] protected RectTransform RectTransform;
        [SerializeField] protected GameHudConfig GameHudConfig;
        

        private Tween _tween;

        protected virtual void Awake()
        {
            if (RectTransform == null) RectTransform = GetComponent<RectTransform>();
        }
        
        [Button]
        public virtual Tween Show()
        {
            return null;
        }

        [Button]
        public virtual Tween Hide()
        {
            return null;
        }

        protected virtual Tween AnimateY(float startingY, float destinationY)
        {
            return AnimateAxis(
                startingY, 
                destinationY, 
                isYAxis: true
            );
        }
        
        protected virtual Tween AnimateX(float startingX, float destinationX)
        {
            return AnimateAxis(
                startingX, 
                destinationX, 
                isYAxis: false
            );
        }

        private Tween AnimateAxis(float startValue, float endValue, bool isYAxis)
        {
            KillTween();

            // Set starting position
            RectTransform.anchoredPosition = GetPosition(startValue, isYAxis);

            // Animate to destination
            _tween = isYAxis 
                ? RectTransform.DOAnchorPosY(endValue, GameHudConfig.AnimationDuration) 
                : RectTransform.DOAnchorPosX(endValue, GameHudConfig.AnimationDuration);

            // Set final position and cleanup
            Vector2 endPos = GetPosition(endValue, isYAxis);

            _tween.OnKill(() => RectTransform.anchoredPosition = endPos)
                  .OnComplete(() => _tween = null);

            return _tween;
        }

        private Vector2 GetPosition(float value, bool isYAxis)
        {
            Vector2 pos = RectTransform.anchoredPosition;
            if (isYAxis) pos.y = value;
            else pos.x = value;
            return pos;
        }

        protected virtual void KillTween()
        {
            _tween?.Kill();
            _tween = null;
        }
    }
}