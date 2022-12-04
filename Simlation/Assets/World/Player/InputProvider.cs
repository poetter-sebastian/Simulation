using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Player
{
    public class InputProvider
    {
        private readonly ComputerActions input = new ();

        public InputProvider()
        {
            input.Enable();
        }

        public InputAction OnLeftClick => input.Player.MainAction;
        
        public InputAction OnMiddleClick => input.Player.MiddleMouse;

        public InputAction OnRightClick => input.Player.RightClick;

        public InputAction OnCheating => input.Player.Cheating;

        public bool Top()
        {
            return input.Player.Forward.ReadValue<float>() > 0.1f;
        }
        
        public bool Down()
        {
            return input.Player.Backward.ReadValue<float>() > 0.1f;
        }

        public bool Right()
        {
            return input.Player.Right.ReadValue<float>() > 0.1f;
        }
        
        public bool Left()
        {
            return input.Player.Left.ReadValue<float>() > 0.1f;
        }

        public Vector2 PointerPosition()
        {
            return input.Player.Position.ReadValue<Vector2>();
        }

        public bool Rotation()
        {
            try
            {
                return input.Player.Rotate.ReadValue<float>() > 0.1f;
            }
            catch (Exception)
            {
                var pad = input.Player.Rotate.ReadValue<Vector2>();
                return pad.x > 0.1f || pad.y > 0.1f;
            }
        }

        public float Rotate(string axis)
        {
            return axis == "X" ? input.Player.Rotation.ReadValue<Vector2>().x : input.Player.Rotation.ReadValue<Vector2>().y;
        }

        public float Scroll()
        {
            return input.Player.Scroll.ReadValue<Vector2>().y;
        }
        
        public event Action<InputAction.CallbackContext> FasterMovement
        {
            add
            {
                input.Player.FastMove.performed += value;
                input.Player.FastMove.canceled += value;
            }
            remove
            {
                input.Player.FastMove.performed -= value;
                input.Player.FastMove.canceled -= value;
            }
        }

        public event Action<InputAction.CallbackContext> ToMenu
        {
            add => input.Player.Menu.performed += value;
            remove => input.Player.Menu.performed -= value;
        }
    }   
}
