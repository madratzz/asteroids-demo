using UnityEngine;

namespace ProjectGame.Features.ScreenWrap
{
    public class ScreenWrap : MonoBehaviour
    {
        [Header("Settings")]
        [SerializeField] private float Buffer = 0.5f; 
        
        private readonly ScreenWrapLogic _logic = new ScreenWrapLogic();
        
        private ScreenWrapDebugger _debugger;
        private Camera _cam;
        

        private void Awake()
        {
            _cam = Camera.main;
            _debugger = GetComponentInChildren<ScreenWrapDebugger>();
            RecalculateBounds();
        }

        private void RecalculateBounds()
        {
            if (_cam == null) _cam = Camera.main;
            if (_cam == null) return;

            float distFromCam = Mathf.Abs(_cam.transform.position.z - transform.position.z);
            Vector2 bottomLeft = _cam.ViewportToWorldPoint(new Vector3(0, 0, distFromCam));
            Vector2 topRight = _cam.ViewportToWorldPoint(new Vector3(1, 1, distFromCam));

            // Update the Logic (The Brain)
            _logic.SetBounds(bottomLeft.x, topRight.x, topRight.y, bottomLeft.y);
            
            UpdateDebuggerBounds(topRight, bottomLeft);
        }

        private void Update()
        {
            // Get current state
            Vector3 currentPos = transform.position;

            // Ask the brain for the new position
            Vector3 newPos = _logic.CalculateWrappedPosition(currentPos, Buffer);

            // Apply ONLY if different (Optimization)
            if (currentPos != newPos)
            {
                transform.position = newPos;
            }
        }
        
        private void UpdateDebuggerBounds(Vector2 topRight, Vector2 bottomLeft)
        {
            if (_debugger == null) return;
            
            float width = topRight.x - bottomLeft.x;
            float height = topRight.y - bottomLeft.y;

            // Calculate Center: (Min + Max) / 2
            Vector3 center = new Vector3(
                (bottomLeft.x + topRight.x) / 2,
                (bottomLeft.y + topRight.y) / 2,
                transform.position.z
            );

            _debugger.UpdateBounds(center, width, height, Buffer);
        }
    }
}