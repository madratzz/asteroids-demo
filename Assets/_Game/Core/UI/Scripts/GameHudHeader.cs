using DG.Tweening;

namespace ProjectCore.Hud
{
    public class GameHudHeader : GameHudBar
    {
        public override Tween Show() => Animate(true);

        public override Tween Hide() => Animate(false);

        private Tween Animate(bool show)
        {
            float offScreen = GameHudConfig.HeaderOffScreenPosition;
            return show ? AnimateY(offScreen, 0) : AnimateY(0, offScreen);
        }
    }
}