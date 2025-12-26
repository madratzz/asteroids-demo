using NUnit.Framework;
using UnityEngine;
using ProjectGame.Features.ScreenWrap;

namespace ProjectGame.Tests.EditMode
{
    public class ScreenWrapLogicTests
    {
        private ScreenWrapLogic _logic;
        private const float LEFT = -10f;
        private const float RIGHT = 10f;
        private const float TOP = 5f;
        private const float BOTTOM = -5f;
        private const float BUFFER = 1f;

        [SetUp]
        public void Setup()
        {
            _logic = new ScreenWrapLogic();
            _logic.SetBounds(LEFT, RIGHT, TOP, BOTTOM);
        }

        [Test]
        public void Position_Inside_Bounds_Should_Not_Change()
        {
            Vector3 input = Vector3.zero;
            Vector3 result = _logic.CalculateWrappedPosition(input, BUFFER);
            
            Assert.AreEqual(input, result);
        }

        [Test]
        public void Position_Past_Right_Buffer_Should_Teleport_Left()
        {
            float overshoot = 0.1f;
            // Right Edge (10) + Buffer (1) + Overshoot (0.1) = 11.1
            Vector3 input = new Vector3(RIGHT + BUFFER + overshoot, 0, 0);
            
            Vector3 result = _logic.CalculateWrappedPosition(input, BUFFER);

            // Expect: Left Edge (-10) - Buffer (1) + Overshoot (0.1) = -10.9
            float expectedX = (LEFT - BUFFER) + overshoot;
            
            Assert.AreEqual(expectedX, result.x, 0.001f);
        }

        [Test]
        public void Position_Past_Bottom_Buffer_Should_Teleport_Top()
        {
            float overshoot = 0.1f; // We went 0.1 units "too far down"
            
            // Bottom Edge (-5) - Buffer (1) - Overshoot (0.1) = -6.1
            Vector3 input = new Vector3(0, BOTTOM - BUFFER - overshoot, 0);
            
            Vector3 result = _logic.CalculateWrappedPosition(input, BUFFER);
            
            // Expect: Top Edge (5) + Buffer (1) - Overshoot (0.1) = 5.9
            // Note: Since we went DOWN, the wrap brings us to the TOP, preserving the downward motion
            float expectedY = (TOP + BUFFER) - overshoot;
            
            Assert.AreEqual(expectedY, result.y, 0.001f);
        }

        [Test]
        public void Diagonal_Exit_Should_Wrap_Both_Axes()
        {
            float overshoot = 0.1f;
            Vector3 input = new Vector3(RIGHT + BUFFER + overshoot, TOP + BUFFER + overshoot, 0);
            
            Vector3 result = _logic.CalculateWrappedPosition(input, BUFFER);
            
            Assert.Less(result.x, 0); 
            Assert.Less(result.y, 0);
        }
        
        [Test]
        public void HighSpeed_Movement_Should_Preserve_Momentum()
        {
            // Move 2.0 units past the edge
            float overshoot = 2.0f;
            Vector3 input = new Vector3(RIGHT + BUFFER + overshoot, 0, 0);
            
            Vector3 result = _logic.CalculateWrappedPosition(input, BUFFER);

            // It should wrap and land 2.0 units inside the other side
            float expectedX = (LEFT - BUFFER) + overshoot;
            
            Assert.AreEqual(expectedX, result.x, 0.001f);
        }
    }
}