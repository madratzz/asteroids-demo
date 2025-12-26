namespace ProjectGame.Features.Weapons.Interfaces
{
    public interface IProjectilePool
    {
        bool IsReady { get; }
        Projectile Get();
        void Release(Projectile item);
    }
}