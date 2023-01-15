using RTSEngine.Events;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RTSEngine
{
    public class RTSInput : MonoBehaviour
    {
        public static PlayerInput Input { get; private set; }

        private void OnEnable()
        {
            Input.Enable();
        }

        private void OnDisable()
        {
            Input.Disable();
        }

        private void Awake()
        {
            Input = new PlayerInput();

            Input.Player.MouseClick.performed += context => InputEvents.CallOnMouseClick();
            Input.Player.MousePass.performed += context => InputEvents.CallOnMouseClickPass();
            Input.Player.MouseRightClick.performed += context => InputEvents.CallOnMouseRightClick();
        }
    }

    public static class RTSInputManager
    {
        public static Vector2 MoveDirection
        {
            get
            {
                return RTSInput.Input.Player.Move.ReadValue<Vector2>();
            }
        }
    }
}
