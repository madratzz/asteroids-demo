
namespace ProjectGame.Features.Weapons.Logic
{
    public class WeaponLogic
    {
        private float _nextFireTime;
        
        public bool ShouldFire(bool isFiringInput, float currentTime, float fireRate)
        {
            if (!isFiringInput) return false;
            if (currentTime < _nextFireTime) return false;
            
            _nextFireTime = currentTime + fireRate;
            return true;
        }
        
        public void Reset() => _nextFireTime = 0;
    }
}