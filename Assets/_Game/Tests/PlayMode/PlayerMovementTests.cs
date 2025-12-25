using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ProjectGame.Features.Player.Components;
using ProjectGame.Features.Player.Configs;
using ProjectGame.Features.Player.Interfaces;

namespace ProjectGame.Tests.PlayMode
{
    public class PlayerMovementTests
    {
        // --------------------------------------------------------
        // 1. THE MOCK (Our "Fake" Controller)
        // --------------------------------------------------------
        public class MockPlayerInput : IPlayerInput
        {
            public float RotationInput { get; set; }
            public bool IsThrusting { get; set; }
            public bool IsFiring { get; set; }
        }

        // --------------------------------------------------------
        // 2. TEST SETUP
        // --------------------------------------------------------
        private GameObject _playerObj;
        private PlayerMotor _motor;
        private Rigidbody2D _rb;
        private MockPlayerInput _mockInput;
        private PlayerSettingsSO _testSettings;

        [SetUp]
        public void Setup()
        {
            // Create the GameObject wrapper
            _playerObj = new GameObject("TestPlayer");
            
            // Add Physics (Required by Motor)
            _rb = _playerObj.AddComponent<Rigidbody2D>();
            _rb.gravityScale = 0;
            _rb.linearDamping = 0; // Set drag to 0 for easier velocity math
            
            // Add the Component we are testing
            _motor = _playerObj.AddComponent<PlayerMotor>();

            // Create dependencies
            _mockInput = new MockPlayerInput();
            _testSettings = ScriptableObject.CreateInstance<PlayerSettingsSO>();
            
            // Configure predictable settings for testing
            _testSettings.ThrustForce = 10f;
            _testSettings.RotationSpeed = 90f; // 90 degrees per second
            _testSettings.DragFactor = 0f;

            // INJECT dependencies (The power of your Architecture!)
            _motor.Initialize(_mockInput, _testSettings);
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(_playerObj);
            Object.Destroy(_testSettings);
        }

        // --------------------------------------------------------
        // 3. THE TEST CASES
        // --------------------------------------------------------

        [UnityTest]
        public IEnumerator Player_Should_Move_Forward_When_Thrusting()
        {
            // Arrange
            _mockInput.IsThrusting = true;
            Vector3 startPos = _playerObj.transform.position;

            // Act: Wait for Physics Engine to tick (approx 0.02s)
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate(); // Wait a few frames for velocity to accumulate

            // Assert
            // 1. Check Velocity
            Assert.Greater(_rb.linearVelocity.y, 0.1f, "Ship did not acquire upward velocity.");
            
            // 2. Check Position
            Assert.Greater(_playerObj.transform.position.y, startPos.y, "Ship did not move physically.");
        }

        [UnityTest]
        public IEnumerator Player_Should_Rotate_Left_When_Input_Is_Negative()
        {
            // Arrange
            _mockInput.RotationInput = -1f; // "Left" or "A" key usually rotates CCW (Positive Z)
            float startRotation = _rb.rotation;

            // Act
            // Rotate for 1 second implies 90 degrees change based on our settings
            float duration = 0.5f;
            float elapsed = 0f;
            
            while (elapsed < duration)
            {
                elapsed += Time.fixedDeltaTime;
                yield return new WaitForFixedUpdate();
            }

            // Assert
            // Note: Rotation in 2D is Z-axis. 
            // Turning LEFT usually increases rotation angle (0 -> 45 -> 90)
            Assert.Greater(_rb.rotation, startRotation, "Ship did not rotate Counter-Clockwise (Left).");
            
            // Optional: Verify approximate angle (0.5s * 90deg/s = 45deg)
            // We use a tolerance (delta) because physics isn't perfectly precise
            Assert.AreEqual(45f, _rb.rotation, 2.0f, "Rotation amount was incorrect.");
        }

        [UnityTest]
        public IEnumerator Player_Should_Stop_Rotating_When_Input_Zero()
        {
            // Arrange
            _mockInput.RotationInput = -1f;
            yield return new WaitForFixedUpdate(); // Start rotating
            
            float rotationWhileMoving = _rb.rotation;
            
            // Act: Stop Input
            _mockInput.RotationInput = 0f;
            yield return new WaitForFixedUpdate();
            yield return new WaitForFixedUpdate();

            // Assert
            Assert.AreEqual(rotationWhileMoving, _rb.rotation, 0.1f, "Ship continued rotating after input stopped.");
        }

        [UnityTest]
        public IEnumerator Player_Should_Apply_Drag_From_Settings()
        {
            // Arrange: Enable Drag in settings
            _testSettings.DragFactor = 5f;
            _motor.Initialize(_mockInput, _testSettings); // Re-inject to ensure drag is read
            
            // Get moving first
            _rb.linearVelocity = new Vector2(0, 10f);
            
            // Act
            yield return new WaitForFixedUpdate();

            // Assert: Velocity should decrease without input
            Assert.Less(_rb.linearVelocity.y, 10f, "Drag was not applied to the Rigidbody.");
        }
    }
}