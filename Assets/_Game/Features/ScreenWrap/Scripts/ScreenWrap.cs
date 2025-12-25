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

        private void RecalculateBounds()
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
            Vector3 currentPos = transform.position;
            
            float newX = CalculateWrappedPosition(currentPos.x, _leftConstraint, _rightConstraint);
            float newY = CalculateWrappedPosition(currentPos.y, _bottomConstraint, _topConstraint);
            
            //Mathf.Approximately for safe float comparison
            if (!Mathf.Approximately(currentPos.x, newX) || !Mathf.Approximately(currentPos.y, newY))
            {
                transform.position = new Vector3(newX, newY, currentPos.z);
            }
        }
        
        private float CalculateWrappedPosition(float current, float min, float max)
        {
            if (current > max + Buffer) return min - Buffer;
            if (current < min - Buffer) return max + Buffer;
            return current;
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