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
        if (context.action.name == "Atk")
        {
            _player.ChangeState(new Atk1State(_player));
        }
        if (context.action.name == "Move")
        {
            _player.ChangeState(new MoveState(_player));
        }
        if (context.action.name == "Dodge")
        {
            _player.ChangeState(new DodgeState(_player));
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
        _player.BindInputCallback(true, OnInputCallback);
    }
    public override void ExitState()
    {
        _player.BindInputCallback(false, OnInputCallback);
    }

    public override void ExecuteOnUpdate()
    {
        Vector2 moveInput = _player.GetMoveInput();
        //_player.Move(moveInput);

        if (moveInput == Vector2.zero)
        {
            _player.ChangeState(new IdleState(_player));
        }
    }
    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        if (context.action.name == "Atk")
        {
            _player.ChangeState(new Atk1State(_player));
        }
        if (context.action.name == "Dodge")
        {
            _player.ChangeState(new DodgeState(_player));
        }
    }
}
public class DodgeState : StateBase
{
    private readonly PlayerView _player;
    public DodgeState(PlayerView player)
    {
        _player = player;
    }

    public override void EnterState()
    {
        _player.Animator_Player.SetTrigger("Dodge");
    }
    public override void ExecuteOnUpdate()
    {
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime > 1)
        {
            _player.ChangeState(new IdleState(_player));
        }
    }
}
public class ItemState : StateBase
{
    private readonly PlayerView _player;
    public ItemState(PlayerView player)
    {
        _player = player;
    }

    public override void EnterState()
    {
        _player.Animator_Player.SetTrigger("Use");
    }

    public override void ExecuteOnUpdate()
    {
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime > 1)
        {
            _player.ChangeState(new IdleState(_player));
        }
    }
}
public class Atk1State : StateBase
{
    private readonly PlayerView _player;
    public Atk1State(PlayerView player)
    {
        _player = player;
    }

    public override void EnterState()
    {
        _player.Animator_Player.SetTrigger("Atk1");
        _player.BindInputCallback(true, OnInputCallback);
    }
    public override void ExitState()
    {
        _player.BindInputCallback(false, OnInputCallback);
    }

    public override void ExecuteOnUpdate()
    {
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime > 1)
        {
            _player.ChangeState(new IdleState(_player));
        }
    }
}

public class Atk2State : StateBase
{
    private readonly PlayerView _player;
    public Atk2State(PlayerView player)
    {
        _player = player;
    }

    public override void EnterState()
    {
        _player.Animator_Player.SetTrigger("Atk2");
    }

    public override void ExecuteOnUpdate()
    {
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime > 1)
        {
            _player.ChangeState(new IdleState(_player));
        }
    }
}
public class Atk3State : StateBase
{
    private readonly PlayerView _player;
    public Atk3State(PlayerView player)
    {
        _player = player;
    }

    public override void EnterState()
    {
        _player.Animator_Player.SetTrigger("Atk3");
    }

    public override void ExecuteOnUpdate()
    {
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime > 1)
        {
            _player.ChangeState(new IdleState(_player));
        }
    }
}