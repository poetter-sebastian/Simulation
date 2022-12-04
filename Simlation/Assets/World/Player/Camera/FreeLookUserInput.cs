using System;
using Cinemachine;
using Game.Utility;
using UnityEngine;
using UnityEngine.InputSystem;
using Utility;
using static UnityEngine.Camera;
using Cursor = UnityEngine.Cursor;
using UnityEngine.EventSystems;
using UnityEngineInternal;
using World.Agents;
using World.Environment;

namespace Player.Camera
{
    [RequireComponent(typeof(CinemachineFreeLook))]
    public class FreeLookUserInput : MonoBehaviour, ILog
    {
        public Transform target = default;
        public EventSystem UIEvents;

        [Header("Functions")]
        public bool borderScroll;
        public bool orbitY = true;
        public bool blockWorldInteractions = false;

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

        public event EventHandler<GenEventArgs<bool>> CallW;
        public event EventHandler<GenEventArgs<bool>> CallA;
        public event EventHandler<GenEventArgs<bool>> CallS;
        public event EventHandler<GenEventArgs<bool>> CallD;
        public event EventHandler<GenEventArgs<bool>> CallRotation;
        public event EventHandler<GenEventArgs<TreeAgent>> TreeWasHit;
        public event EventHandler DoCheating;
        
        private Vector3 mousePosition;
        private Transform relativeTransform;
        private bool freeLookActive = false;
        private bool newFreeLookActive;
        private bool hasFocus = true;
        private bool inWindow = true;
        private bool inGUI = false;

        private InputProvider input;

        private void OnApplicationFocus(bool changedFocus)
        {
            hasFocus = changedFocus;
        }

        public string LN()
        {
            return "Camera controller";
        }

        public void OnUIToggle(object s, GenEventArgs<bool> e)
        {
            inGUI = e.Value;
        }
        
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
            input.OnMiddleClick.performed += OnMiddleClick;
            input.OnRightClick.performed += OnRightClick;
            input.OnCheating.performed += OnCheating;
        }

        private void OnDisable()
        {
            input.FasterMovement -= FasterMovement;
            input.OnLeftClick.performed -= OnLeftClick;
            input.OnMiddleClick.performed -= OnMiddleClick;
            input.OnRightClick.performed -= OnRightClick;
        }

        private void Update()
        {
            OnWindowPosition();

            if (inWindow)
            {
                newFreeLookActive = input.Rotation();
                if (!inGUI)
                {
                    HandleMovementInput();
                }
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
            if (!blockWorldInteractions)
            {
                foreach (var hit in Physics.RaycastAll(ray, 600f))
                {
                    var selectedComponent = hit.rigidbody.GetComponent<IMouseListener>();
                    //collider was hit and it's a world object and the function is called
                    var tree = hit.rigidbody.GetComponent<TreeAgent>();
                    if (tree != null)
                    {
                        TreeWasHit?.Invoke(this, new GenEventArgs<TreeAgent>(tree));
                    }
                    selectedComponent?.MouseClick();
                }
            }
            else
            {
                foreach (var hit in Physics.RaycastAll(ray, 600f))
                {
                    var selectedComponent = hit.rigidbody.GetComponent<WorldController>();
                    //collider was hit and it's the world object
                    selectedComponent?.player.SpawnObjOnWorld(hit.point);
                }
                blockWorldInteractions = false;
            }
        }

        private void OnMiddleClick(InputAction.CallbackContext e)
        {
            CallRotation?.Invoke(this, new GenEventArgs<bool>(true));
        }
        
        private void OnRightClick(InputAction.CallbackContext e)
        {
            ILog.L(LN, "Right clicked!");
            CallRotation?.Invoke(this, new GenEventArgs<bool>(true));
        }

        private void OnCheating(InputAction.CallbackContext e)
        {
            if (!inGUI)
            {
                DoCheating?.Invoke(this, EventArgs.Empty);
            }
        }

        public void OnESC()
        {
            Application.Quit();
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
                CallW?.Invoke(this, new GenEventArgs<bool>(true));
                moveDirection += relativeTransform.forward;
            }

            if (input.Left() || (input.PointerPosition().x <= borderThickness && borderScroll))
            {
                CallA?.Invoke(this, new GenEventArgs<bool>(true));
                moveDirection += -relativeTransform.right;
            }

            if (input.Down() || (input.PointerPosition().y <= borderThickness && borderScroll))
            {
                CallS?.Invoke(this, new GenEventArgs<bool>(true));
                moveDirection += -relativeTransform.forward;
            }

            if (input.Right() || (input.PointerPosition().x >= Screen.width - borderThickness && borderScroll))
            {
                CallD?.Invoke(this, new GenEventArgs<bool>(true));
                moveDirection += relativeTransform.right;
            }
            moveDirection.y = 0f;

            var movement = moveDirection.normalized * (movementSpeed * Time.deltaTime);
            
            target.position += movement;

            if (moveDirection != Vector3.zero)
            {
                target.rotation = Quaternion.RotateTowards(target.rotation, Quaternion.LookRotation(moveDirection), 0 * Time.deltaTime);
            }
        }

        private void OnWindowPosition()
        {
            var screenRect = new Rect(0, 0, Screen.width, Screen.height);
            inWindow = screenRect.Contains(input.PointerPosition()) && hasFocus;
        }
    }
}
