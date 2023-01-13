using RtsEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine.Player
{
    public class CameraController : MonoBehaviour
    {
        // Скорость движения камеры
        public float movementSpeed = 10.0f;
        // Скорость поворота камеры
        public float rotationSpeed = 10.0f;
        // Ограничение угла наклона камеры
        public float minAngle = 10.0f;
        public float maxAngle = 80.0f;

        private void Update()
        {
            HandleMovementHorizontal(RTSInputManager.MoveDirection.x);
            HandleMovementVertical(RTSInputManager.MoveDirection.y);

            // HandleRotation();
            // HandleTilt();
        }

        private void HandleMovementHorizontal(float x)
        {
            if (x == 0)
                return;

            transform.position += new Vector3(x, 0, 0) * movementSpeed * Time.deltaTime;
        }

        private void HandleMovementVertical(float z)
        {
            if (z == 0)
                return;

            transform.position += new Vector3(0, 0, z) * movementSpeed * Time.deltaTime;
        }


        private void HandleRotation()
        {
            // Поворот камеры
            if (!Input.GetKey(KeyCode.LeftControl))
                return;

            float y = Input.GetAxis("Mouse X");

            if (y == 0)
                return;

            transform.Rotate(0, y * rotationSpeed, 0);
        }

        private void HandleTilt()
        {
            // Наклон камеры
            float mouseInput = Input.GetAxis("Mouse Y");

            if (mouseInput == 0)
                return;

            float angle = mouseInput * rotationSpeed;
            angle = Mathf.Clamp(angle, -maxAngle, -minAngle);
            Camera.main.transform.localEulerAngles += new Vector3(-angle, 0, 0);
        }
    }

}