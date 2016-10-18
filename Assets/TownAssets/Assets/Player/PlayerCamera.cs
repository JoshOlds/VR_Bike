using UnityEngine;

namespace Waypoints.Assets
{
    public class PlayerCamera
    {
        public Vector2 MouseSensitivity { get; set; }
        public Vector3 MousePosition { get; private set; }

        public bool UseRelativeMovement = false;
        
        private readonly Bike _bike;
        private readonly Camera _camera;
        private const float Height = .86f;
        private const float Distance = .54f;
        private const float Damping = 10f;

        private const float RotationSpeed = 10;

        

        public PlayerCamera(Bike bike, Camera camera)
        {
            _bike = bike;
            _camera = camera;
        }

        public void Update()
        {
             Screen.lockCursor = UseRelativeMovement;

            if (UseRelativeMovement)
            {
                MousePosition += new Vector3(
                    Input.GetAxis("Mouse X") * Time.deltaTime * MouseSensitivity.x,
                    Input.GetAxis("Mouse Y") * Time.deltaTime * MouseSensitivity.y);
            }
            else
            {
                MousePosition = Input.mousePosition;
            }

            var position = _bike.transform.TransformPoint(0, Height, -Distance);
            _camera.transform.position = position;
            _camera.transform.LookAt(_bike.transform.TransformPoint(0, 0, 50), _bike.transform.up);
            UpdateRotation();
        }

        private void UpdateRotation()
        {
            var mouseMovement = (MousePosition - (new Vector3(Screen.width / 2f, Screen.height / 2f))) * .01f;

            if (mouseMovement.sqrMagnitude >= .5)
            {
                _camera.transform.Rotate(new Vector3(-mouseMovement.y, mouseMovement.x, 0) * RotationSpeed);
            }
        }

    }
}