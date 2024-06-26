using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerView : MonoBehaviour
{
    [SerializeField] TextMesh TextMesh_Name;

    public TextMesh TextMesh_Level;
    public Animator Animator_Player;

    private IState _curState;

    private Action<InputAction.CallbackContext> _inputCallback;

    private Vector2 _moveInput;

    private void Start()
    {
        ChangeState(new IdleState(this));
    }

    public void ChangeState(IState newState)
    {
        _curState?.ExitState();
        _curState = newState;
        _curState.EnterState();
    }

    public void BindInputCallback(bool isBind, Action<InputAction.CallbackContext> callback)
    {
        if(isBind)
            _inputCallback += callback;
        else
            _inputCallback -= callback;
    }

    public void OnActionInput(InputAction.CallbackContext context)
    {
        _inputCallback?.Invoke(context);
    }
    public Vector2 GetMoveInput()
    {
        return _moveInput;
    }

    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
    }

    public void Move(Vector2 moveInput)
    {
        
    }
}
