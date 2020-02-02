using UnityEngine;

namespace DNA
{
    public class CameraControl : MonoBehaviour
    {
        private float _speed = 4f;
        private float _scrollSpeed = 0.5f;
        private float _scrollValue = 0;
        private float _minScrollBorder = -5f;
        private float _maxScrollBorder = 2f;

        private Vector3 _cameraScrollPosition;

        private void Start()
        {
            _cameraScrollPosition = transform.position;
        }

        private void Update()
        {
            RotateAroundHuman();
            Zoom();
        }

        private void RotateAroundHuman()
        {
            if (Input.GetMouseButton(2))
            {
                transform.RotateAround(Human.HumanTransform.position, transform.right, -Input.GetAxis("Mouse Y") * _speed);
                transform.RotateAround(Human.HumanTransform.position, transform.up, Input.GetAxis("Mouse X") * _speed);
                transform.LookAt(Human.HumanTransform.position);
            }
        }

        private void Zoom()
        {
            _scrollValue += Input.mouseScrollDelta.y * _scrollSpeed;
            if (_scrollValue >= _minScrollBorder && _scrollValue <= _maxScrollBorder)
            {
                transform.position += transform.forward * Input.mouseScrollDelta.y * _scrollSpeed;
            }
            else _scrollValue = Mathf.Clamp(_scrollValue, _minScrollBorder, _maxScrollBorder);
        }
    }
}