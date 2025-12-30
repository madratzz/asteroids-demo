namespace ProjectGame.Features.Weapons.Logic
{
    public class ProjectileLogic
    {
        private float _spawnTime;
        private float _lifetime;

        public void Initialize(float startTime, float duration)
        {
            _spawnTime = startTime;
            _lifetime = duration;
        }

        public bool IsExpired(float currentTime)
        {
            return currentTime >= _spawnTime + _lifetime;
        }
    }
}