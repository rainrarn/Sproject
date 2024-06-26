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

    public override void EnterState()
    {
        _player.BindInputCallback(true, OnInputCallback);
    }

    public override void ExitState()
    {
        _player.BindInputCallback(false, OnInputCallback);
    }

    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        if(context.action.name == "Atk")
        {
            _player.ChangeState(new AtkState(_player));
        }
        if(context.action.name == "Move")
        {
            _player.ChangeState(new MoveState(_player));
        }
    }
}

public class MoveState : StateBase
{
    private readonly PlayerView _player;
    public MoveState(PlayerView player)
    {
        _player = player;
    }

    public override void EnterState()
    {
        _player.Animator_Player.SetTrigger("Move");
    }
    
}
public class AtkState : StateBase
{
    private readonly PlayerView _player;
    public AtkState(PlayerView player)
    {
        _player = player;
    }

    public override void EnterState()
    {
        _player.Animator_Player.SetTrigger("Atk");
    }

    public override void ExecuteOnUpdate()
    {
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if(animInfo.normalizedTime>1)
        {
            _player.ChangeState(new IdleState(_player));
        }
    }
}