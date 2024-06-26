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
}
