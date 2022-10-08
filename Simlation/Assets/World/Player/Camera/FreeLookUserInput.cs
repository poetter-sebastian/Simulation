using System;
using Cinemachine;
using Game.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using static UnityEngine.Camera;
using Cursor = UnityEngine.Cursor;

namespace Player.Camera
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class FreeLookUserInput : MonoBehaviour, ILog
    {
        public Transform target = default;
        
        [Header("Functions")]
        public bool borderScroll;
        public bool orbitY = true;

        [Header("Scrolling")]
        public float scrollSpeed = 0.5f;

        [Header("Movement")]
        public float normalSpeed = 50;
        public float fastSpeed = 80;
        public float movementTime = 2;
        public float movementSpeed = 50;

        [Header("Restriction")]
        public float minZoom = 80f;
        public float maxZoom = 260f;
        public float borderThickness = 12.5f;
        public float cameraDistance = 10f;
        public float orbitFOV = 75;

        [Header("Changeable")]
        public Vector2Int mapLimit;

        private Vector3 mousePosition;
        private Transform relativeTransform;
        private bool freeLookActive = false;
        private bool newFreeLookActive;
        private bool hasFocus = true;
        private bool inWindow = true;

        private InputProvider input;
        
        private void Awake()
        {
            if (main is { }) relativeTransform = main.transform;
            CinemachineCore.GetInputAxis = GetInputAxis;
            movementSpeed = normalSpeed;
        }

        private void OnEnable()
        {
            input = new InputProvider();
            input.FasterMovement += FasterMovement;
            input.OnLeftClick.performed += OnLeftClick;
        }

        private void OnDisable()
        {
            input.FasterMovement -= FasterMovement;
            input.OnLeftClick.performed -= OnLeftClick;
        }

        private void Update()
        {
            OnWindowPosition();
            if (inWindow)
            {
                newFreeLookActive = input.Rotation();
                HandleMovementInput();
            }
            switch (freeLookActive)
            {
                case false when newFreeLookActive:
                    freeLookActive = true;
                    Cursor.visible = false;
                    Cursor.lockState = CursorLockMode.Locked;
                    mousePosition = input.PointerPosition();
                    break;
                case true when !newFreeLookActive:
                    freeLookActive = false;
                    Cursor.visible = true;
                    Cursor.lockState = CursorLockMode.None;
                    break;
            }
        }

        private void OnMiddleClickHold()
        {
            freeLookActive = true;
            Cursor.visible = false;
            Cursor.lockState = CursorLockMode.Locked;
            mousePosition = input.PointerPosition();
        }
        
        private void OnMiddleCLickLeft()
        {
            freeLookActive = false;
            Cursor.visible = true;
            Cursor.lockState = CursorLockMode.None;
        }

        private void OnLeftClick(InputAction.CallbackContext e)
        {
            ILog.L(LN, "LeftMouseClicked");
            var ray = main.ScreenPointToRay(input.PointerPosition());
            if (Physics.Raycast(ray, out var hit))
            {
                var selectedComponent = hit.rigidbody.GetComponent<IMouseListener>();
                if (selectedComponent == null) return;
                //collider was hit and it's a world object and the function is called
                selectedComponent.MouseClick();
            }
        }
        
        private void OnRightClick()
        {
            
        }

        public void OnESC()
        {
            
        }
        
        private float GetInputAxis(string axisName)
        {
            if(inWindow)
            {
                if(freeLookActive)
                {
                    return input.Rotate(axisName);
                }
                if (axisName == "Y")
                {
                    return input.Scroll() * - scrollSpeed;
                }
            }
            return 0;
        }

        private void FasterMovement(InputAction.CallbackContext e)
        {
            movementSpeed = e.performed ? fastSpeed : normalSpeed;
        }

        private void HandleMovementInput()
        {
            var moveDirection = Vector3.zero;

            if (input.Top() || (input.PointerPosition().y >= Screen.height - borderThickness && borderScroll))
            {
                moveDirection += relativeTransform.forward;
            }

            if (input.Left() || (input.PointerPosition().x <= borderThickness && borderScroll))
            {
                moveDirection += -relativeTransform.right;
            }

            if (input.Down() || (input.PointerPosition().y <= borderThickness && borderScroll))
            {
                moveDirection += -relativeTransform.forward;
            }

            if (input.Right() || (input.PointerPosition().x >= Screen.width - borderThickness && borderScroll))
            {
                moveDirection += relativeTransform.right;
            }
            moveDirection.y = 0f;

            var movement = moveDirection.normalized * (movementSpeed * Time.deltaTime);
            
            target.position += movement;

            if (moveDirection != Vector3.zero)
                target.rotation = Quaternion.RotateTowards(target.rotation, Quaternion.LookRotation(moveDirection), 0 * Time.deltaTime);
        }

        private void OnWindowPosition()
        {
            var screenRect = new Rect(0, 0, Screen.width, Screen.height);
            inWindow = screenRect.Contains(input.PointerPosition()) && hasFocus;
        }

        private void OnApplicationFocus(bool changedFocus)
        {
            hasFocus = changedFocus;
        }

        public string LN()
        {
            return "Camera controller";
        }
    }
}
