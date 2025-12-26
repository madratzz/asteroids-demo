using NUnit.Framework;
using ProjectGame.Features.Weapons.Logic;

namespace ProjectGame.Tests.EditMode
{
    public class ProjectileLogicTests
    {
        private ProjectileLogic _logic;
        private const float SPAWN_TIME = 100f;
        private const float LIFETIME = 2.0f;

        [SetUp]
        public void Setup()
        {
            _logic = new ProjectileLogic();
            _logic.Initialize(SPAWN_TIME, LIFETIME);
        }

        
        [Test]
        public void IsExpired_ReturnsFalse_Before_Lifetime_Elapsed()
        {
            // Act: Check at 1.9 seconds alive
            bool result = _logic.IsExpired(SPAWN_TIME + 1.9f);

            // Assert
            Assert.IsFalse(result);
        }

        
        // Verifies the exact boundary condition (>= vs >). 
        [Test]
        public void IsExpired_ReturnsTrue_Exactly_At_Lifetime_Limit()
        {
            // Act: Check exactly at 2.0 seconds alive
            bool result = _logic.IsExpired(SPAWN_TIME + LIFETIME);

            // Assert
            Assert.IsTrue(result, "Should expire exactly at the limit");
        }

        [Test]
        public void IsExpired_ReturnsTrue_Well_After_Limit()
        {
            // Act
            bool result = _logic.IsExpired(SPAWN_TIME + LIFETIME + 5.0f);

            // Assert
            Assert.IsTrue(result);
        }
    }
}