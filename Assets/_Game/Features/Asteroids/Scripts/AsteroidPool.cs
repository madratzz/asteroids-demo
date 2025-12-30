using ProjectGame.Core.Pooling;

namespace ProjectGame.Features.Enemies
{
    // Inherits all the Addressables and Container logic!
    public class AsteroidPool : ObjectPoolBase<Asteroid>, IPool<Asteroid>
    {
    }
}