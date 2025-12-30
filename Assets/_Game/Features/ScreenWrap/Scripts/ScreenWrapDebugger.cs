using UnityEngine;
namespace ProjectGame.Features.ScreenWrap
{
    public class ScreenWrapDebugger : MonoBehaviour
    {
        [SerializeField] private bool ShowGizmos = true;
        
        // Internal state for drawing
        private Vector3 _center;
        private float _width;
        private float _height;
        private float _buffer;
        private bool _isInitialized;

        public void UpdateBounds(Vector3 center, float width, float height, float buffer)
        {
            _center = center;
            _width = width;
            _height = height;
            _buffer = buffer;
            _isInitialized = true;
        }

        private void OnDrawGizmos()
        {
            if (!ShowGizmos || !_isInitialized) return;

            // Draw Screen Edge (Yellow)
            Gizmos.color = Color.yellow;
            Gizmos.DrawWireCube(_center, new Vector3(_width, _height, 1));

            // Draw Buffer Edge (Red)
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(_center, new Vector3(_width + _buffer * 2, _height + _buffer * 2, 1));
        }
    }
}