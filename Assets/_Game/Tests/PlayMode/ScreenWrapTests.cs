using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.TestTools;
using ProjectGame.Features.ScreenWrap;

namespace ProjectGame.Tests.PlayMode
{
    public class ScreenWrapTests
    {
        private GameObject _testObj;
        private ScreenWrap _screenWrap;
        private Camera _cam;

        [SetUp]
        public void Setup()
        {
            //Setup Camera
            GameObject camObj = new GameObject("Main Camera")
            {
                tag = "MainCamera",
                transform =
                {
                    //Move Camera back so there is distance!
                    position = new Vector3(0, 0, -10f)
                }
            };

            _cam = camObj.AddComponent<Camera>();
            _cam.orthographic = true;
            _cam.orthographicSize = 5f;

            //Setup Object at Z=0
            _testObj = new GameObject("WrappingObject")
            {
                transform =
                {
                    position = Vector3.zero // Explicitly ensure Z=0
                }
            };

            // 3. Add Component (Awake runs here, calculating bounds based on Distance=10)
            _screenWrap = _testObj.AddComponent<ScreenWrap>();
        }

        [TearDown]
        public void Teardown()
        {
            Object.Destroy(_testObj);
            if (_cam != null) Object.Destroy(_cam.gameObject);
        }

        [UnityTest]
        public IEnumerator Object_Should_Wrap_Horizontal()
        {
            // Arrange: Calculate the *actual* screen edge at Z=0
            float dist = Mathf.Abs(_cam.transform.position.z - _testObj.transform.position.z);
            float rightEdge = _cam.ViewportToWorldPoint(new Vector3(1, 0, dist)).x;
            
            // Move object past the edge + buffer (assuming buffer is 0.5f)
            _testObj.transform.position = new Vector3(rightEdge + 1.0f, 0, 0); 

            // Act
            yield return null; // Wait for Update frame

            // Assert
            // It should now be on the negative (Left) side
            Assert.Less(_testObj.transform.position.x, 0, "Object did not wrap to the left.");
        }

        [UnityTest]
        public IEnumerator Object_Should_Wrap_Vertical()
        {
            // Arrange
            float dist = Mathf.Abs(_cam.transform.position.z - _testObj.transform.position.z);
            float topEdge = _cam.ViewportToWorldPoint(new Vector3(0, 1, dist)).y;
            
            // Move object past the top edge + buffer
            _testObj.transform.position = new Vector3(0, topEdge + 1.0f, 0); 

            // Act
            yield return null;

            // Assert
            // It should now be on the negative (Bottom) side
            Assert.Less(_testObj.transform.position.y, 0, "Object did not wrap to bottom.");
        }
    }
}