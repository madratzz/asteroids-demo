using NUnit.Framework;
using ProjectGame.Features.Weapons.Logic;

namespace ProjectGame.Tests.EditMode
{
    public class WeaponLogicTests
    {
        private WeaponLogic _logic;
        private const float FIRE_RATE = 1.0f;

        [SetUp]
        public void Setup()
        {
            _logic = new WeaponLogic();
        }
        
        [Test]
        public void ShouldFire_ReturnsFalse_When_InputIsFalse()
        {
            // Arrange
            float time = 10f;
            
            // Act
            bool result = _logic.ShouldFire(isFiringInput: false, currentTime: time, fireRate: FIRE_RATE);

            // Assert
            Assert.IsFalse(result);
        }
        
        [Test]
        public void ShouldFire_ReturnsTrue_When_InputIsTrue_And_FirstShot()
        {
            // Act
            bool result = _logic.ShouldFire(true, 10f, FIRE_RATE);

            // Assert
            Assert.IsTrue(result);
        }

        
        [Test]
        public void ShouldFire_RespectsCooldown_Sequence()
        {
            // Fire First Shot (Should Succeed)
            _logic.ShouldFire(true, 10f, FIRE_RATE);

            // Try firing 0.1s later (Should Fail)
            bool rapidFireAttempt = _logic.ShouldFire(true, 10.1f, FIRE_RATE);
            Assert.IsFalse(rapidFireAttempt, "Allowed firing inside cooldown window");

            // Try firing exactly at cooldown duration (Should Succeed)
            // This also covers Regression Protection for "Off-by-one" errors
            bool validShot = _logic.ShouldFire(true, 11.0f, FIRE_RATE);
            Assert.IsTrue(validShot, "Failed to fire exactly when cooldown expired");
        }
        
        // Using TestCase to blast multiple scenarios at the logic efficiently
        [TestCase(0f, ExpectedResult = true)]  // Fire Rate 0 (Machine Gun)
        [TestCase(10f, ExpectedResult = false)] // Fire Rate 10 (Slow) - checked immediately after shot
        public bool ShouldFire_Handles_Different_FireRates(float fireRate)
        {
            // Fire once
            _logic.ShouldFire(true, 0f, fireRate);
            
            // Check immediately after (at 0.1s)
            // If fireRate is 0, we expect TRUE. If 10, FALSE.
            return _logic.ShouldFire(true, 0.1f, fireRate);
        }
    }
}