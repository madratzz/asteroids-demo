using UnityEngine;

namespace ProjectGame.Features.ScreenWrap
{
    public class ScreenWrapLogic
    {
        private float _leftConstraint;
        private float _rightConstraint;
        private float _topConstraint;
        private float _bottomConstraint;
        
        private float _width; // Cache the dimensions
        private float _height; // Cache the dimensions

        public void SetBounds(float left, float right, float top, float bottom)
        {
            _leftConstraint = left;
            _rightConstraint = right;
            _topConstraint = top;
            _bottomConstraint = bottom;
            
            // Calculate total width/height once
            // Note: These will be used to calculate the 'Range' including buffers later
            _width = _rightConstraint - _leftConstraint;
            _height = _topConstraint - _bottomConstraint;
        }

        public Vector3 CalculateWrappedPosition(Vector3 currentPos, float buffer)
        {
            // Calculate the full range including the buffer on both sides
            // The "World" is bigger than the screen by exactly 2x Buffer
            float rangeX = _width + (buffer * 2);
            float rangeY = _height + (buffer * 2);

            // Define the "Start" point (0,0) (Left/Bottom minus buffer)
            float startX = _leftConstraint - buffer;
            float startY = _bottomConstraint - buffer;
            
            // Mathf.Repeat creates a loop: 0 -> range -> 0
            // We shift the input by startX so 0 aligns with the left-most buffer edge
            float wrappedX = startX + Mathf.Repeat(currentPos.x - startX, rangeX);
            float wrappedY = startY + Mathf.Repeat(currentPos.y - startY, rangeY);

            return new Vector3(wrappedX, wrappedY, currentPos.z);
        }
    }
}