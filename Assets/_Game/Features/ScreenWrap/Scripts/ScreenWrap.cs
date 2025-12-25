using UnityEngine;

namespace ProjectGame.Features.ScreenWrap
{
    public class ScreenWrap : MonoBehaviour
    {
        [Header("Settings")]
        [Tooltip("The padding beyond the screen edge.")]
        [SerializeField] private float Buffer = 0.5f; 
        
        [SerializeField] private bool DrawBounds = true;

        private Camera _cam;
        private float _leftConstraint;
        private float _rightConstraint;
        private float _bottomConstraint;
        private float _topConstraint;

        private void Awake()
        {
            _cam = Camera.main;
            RecalculateBounds();
        }

        public void RecalculateBounds()
        {
            if (_cam == null) _cam = Camera.main;
            if (_cam == null) return;

            // Calculate absolute distance from Camera to the playing field (Z=0)
            float distFromCam = Mathf.Abs(_cam.transform.position.z - transform.position.z);
            
            // Use that distance for the viewport calculation
            Vector2 bottomLeft = _cam.ViewportToWorldPoint(new Vector3(0, 0, distFromCam));
            Vector2 topRight = _cam.ViewportToWorldPoint(new Vector3(1, 1, distFromCam));

            _leftConstraint = bottomLeft.x;
            _rightConstraint = topRight.x;
            _bottomConstraint = bottomLeft.y;
            _topConstraint = topRight.y;
        }

        private void Update()
        {
            Vector3 pos = transform.position;
            bool hasTeleported = false;

            // Horizontal Wrap
            if (pos.x > _rightConstraint + Buffer)
            {
                pos.x = _leftConstraint - Buffer;
                hasTeleported = true;
            }
            else if (pos.x < _leftConstraint - Buffer)
            {
                pos.x = _rightConstraint + Buffer;
                hasTeleported = true;
            }

            // Vertical Wrap
            if (pos.y > _topConstraint + Buffer)
            {
                pos.y = _bottomConstraint - Buffer;
                hasTeleported = true;
            }
            else if (pos.y < _bottomConstraint - Buffer)
            {
                pos.y = _topConstraint + Buffer;
                hasTeleported = true;
            }

            if (hasTeleported)
            {
                transform.position = pos;
            }
        }
        
        
        private void OnDrawGizmos()
        {
            if (!DrawBounds) return;

            Gizmos.color = Color.yellow;
            // Draw a box representing the calculated screen bounds
            float width = _rightConstraint - _leftConstraint;
            float height = _topConstraint - _bottomConstraint;
            float centerX = (_leftConstraint + _rightConstraint) / 2;
            float centerY = (_topConstraint + _bottomConstraint) / 2;

            Gizmos.DrawWireCube(new Vector3(centerX, centerY, transform.position.z), new Vector3(width, height, 1));
            
            // Draw buffer zone
            Gizmos.color = Color.red;
            Gizmos.DrawWireCube(new Vector3(centerX, centerY, transform.position.z), new Vector3(width + Buffer*2, height + Buffer*2, 1));
        }
    }
}