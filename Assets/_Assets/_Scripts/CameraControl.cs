using UnityEngine;

namespace DNA
{
    public class CameraControl : MonoBehaviour
    {
        private float _speed = 4f;
        private float _scrollSpeed = 0.75f;
        private void Update()
        {
            if (Input.GetMouseButton(0))
            {
                transform.RotateAround(Human.Instance.transform.position, transform.right, -Input.GetAxis("Mouse Y") * _speed);
                transform.RotateAround(Human.Instance.transform.position, transform.up, Input.GetAxis("Mouse X") * _speed);
            }

            transform.position += transform.forward * Input.mouseScrollDelta.y * _scrollSpeed;
        }
    }
}