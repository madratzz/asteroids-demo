using ProjectGame.Core.Pooling; // Assuming you created the Generic Base
using ProjectGame.Features.Enemies;

namespace ProjectGame.Features.Enemies
{
    // Inherits all the Addressables and Container logic!
    public class AsteroidPool : ObjectPoolBase<Asteroid>
    {
    }
}