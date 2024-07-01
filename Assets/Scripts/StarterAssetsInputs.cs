using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace StarterAssets
{
    public class StarterAssetsInputs : MonoBehaviour
    {
        [Header("Character Input Values")]
        public Vector2 move;
        public Vector2 look;

        [Header("Movement Settings")]
        public bool analogMovement;

        [Header("Mouse Cursor Settings")]
        public bool cursorLocked = true;
        public bool cursorInputForLook = true;

        [Header("Unity Events")]
        public UnityEvent<Vector2> OnMoveEvent;
        public UnityEvent<Vector2> OnLookEvent;

        private void Awake()
        {
            if (OnMoveEvent == null) OnMoveEvent = new UnityEvent<Vector2>();
            if (OnLookEvent == null) OnLookEvent = new UnityEvent<Vector2>();
        }

        public void OnMove(InputValue value)
        {
            Vector2 input = value.Get<Vector2>();
            OnMoveEvent.Invoke(input);
            MoveInput(input);
        }

        public void OnLook(InputValue value)
        {
            if (cursorInputForLook)
            {
                Vector2 input = value.Get<Vector2>();
                OnLookEvent.Invoke(input);
                LookInput(input);
            }
        }

        public void MoveInput(Vector2 newMoveDirection)
        {
            move = newMoveDirection;
        }

        public void LookInput(Vector2 newLookDirection)
        {
            look = newLookDirection;
        }

        private void OnApplicationFocus(bool hasFocus)
        {
            SetCursorState(cursorLocked);
        }

        private void SetCursorState(bool newState)
        {
            Cursor.lockState = newState ? CursorLockMode.Locked : CursorLockMode.None;
        }
    }
}
