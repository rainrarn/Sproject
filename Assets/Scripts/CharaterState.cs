using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
public interface IState
{
    void EnterState();
    void ExitState();
    void ExecuteOnUpdate();
    void OnInputCallback(InputAction.CallbackContext context);
}

public class StateBase : IState
{
    public virtual void EnterState() { }
    public virtual void ExitState() { }
    public virtual void ExecuteOnUpdate() { }
    public virtual void OnInputCallback(InputAction.CallbackContext context) { }
}

public class IdleState : StateBase
{
    private readonly PlayerView _player;
    public IdleState(PlayerView player)
    {
        _player = player;
    }

    
}