using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

namespace ProjectCore.Hud
{
    public class GameHud : MonoBehaviour
    {
        [SerializeField] protected List<GameHudBar> GameHudBars;
        
        public virtual Tween Show() => AnimateBars(true);

        public virtual Tween Hide() => AnimateBars(false);

        protected virtual Tween AnimateBars(bool show)
        {
            Sequence sequence = DOTween.Sequence();
            
            foreach (GameHudBar gameHudBar in GameHudBars)
            {
                if (gameHudBar != null)
                {
                    sequence.Join(show ? gameHudBar.Show() : gameHudBar.Hide());
                }
            }

            return sequence;
        }
    }
}