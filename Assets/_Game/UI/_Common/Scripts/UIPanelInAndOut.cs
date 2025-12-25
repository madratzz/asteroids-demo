using System.Collections;
using UnityEngine;
using DG.Tweening;
using ExtensionMethods;

namespace ProjectCore.UI
{
    public class UIPanelInAndOut : UIView
    {
        public UIConfig UIPanelConfig;
        public RectTransform UIPanel;
        /*[Required] public UISounds UISounds;*/

        protected float _exitTime;
        private Tween _panelTranslationTween;

        private void Awake()
        {
            UIPanel.SetAnchoredPositionX(UIPanelConfig.GetOffScreenOffset());
        }

        public override IEnumerator Show(bool isResumed)
        {
            if (isResumed && UIPanelConfig != null)
            {
                UIPanel.SetAnchoredPositionX(UIPanelConfig.GetOffScreenOffset());
            }

            UIPanelEaseIn();

            if (UIPanelConfig == null) yield break;

            _exitTime = UIPanelConfig.EaseOutTime + 0.1f;
            yield return new WaitForSeconds(UIPanelConfig.EaseInTime);
        }

        protected virtual void UIPanelEaseIn()
        {
            _panelTranslationTween?.Kill();
            if (UIPanelConfig != null)
            {
                _panelTranslationTween = UIPanel.DOAnchorPosX(0, UIPanelConfig.EaseInTime)
                    .SetEase(UIPanelConfig.EasingIn);
            }
        }

        public override IEnumerator Hide(bool shouldDestroy)
        {
            // if (UISounds != null) UISounds.PlayCloseViewSound();

            UIPanelEaseOut(shouldDestroy);
            yield return new WaitForSeconds(_exitTime);
        }

        protected virtual void UIPanelEaseOut(bool shouldDestroy)
        {
            _panelTranslationTween?.Kill();

            if (UIPanelConfig == null) return;

            float targetX = -UIPanelConfig.GetOffScreenOffset();
            _panelTranslationTween = UIPanel.DOAnchorPosX(targetX, UIPanelConfig.EaseOutTime)
                .SetEase(UIPanelConfig.EasingOut);
        }
    }
}
