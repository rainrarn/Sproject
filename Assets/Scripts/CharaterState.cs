using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// ���� �������̽� ����
public interface IState
{
    void EnterState(); // ���� ���� �� ȣ��Ǵ� �޼���
    void ExitState(); // ���� ���� �� ȣ��Ǵ� �޼���
    void ExecuteOnUpdate(); // ���� ������Ʈ �� ȣ��Ǵ� �޼���
    void OnInputCallback(InputAction.CallbackContext context); // �Է� �ݹ� �޼���
    void OnAnimationComplete(string animationName); // �ִϸ��̼� �Ϸ� �� ȣ��Ǵ� �޼���
}

public abstract class StateBase : IState
{
    public virtual void EnterState() { } // ���� ���� �޼���
    public virtual void ExitState() { } // ���� ���� �޼���
    public virtual void ExecuteOnUpdate() { } // ���� ������Ʈ �޼���
    public virtual void OnInputCallback(InputAction.CallbackContext context) { } // ���� �Է� �ݹ� �޼���
    public virtual void OnAnimationComplete(string animationName) { } // ���� �ִϸ��̼� �Ϸ� �޼���
}

// Idle ���� Ŭ����
public class IdleState : StateBase
{
    private readonly PlayerController _player;

    public IdleState(PlayerController player)
    {
        _player = player;
    }

    // Idle ���� ���� �� ȣ��
    public override void EnterState()
    {
        _player.BindInputCallback(true, OnInputCallback);
    }

    // Idle ���� ���� �� ȣ��
    public override void ExitState()
    {
        _player.BindInputCallback(false, OnInputCallback);
    }

    // �Է� �ݹ� ó��
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

// Move ���� Ŭ����
public class MoveState : StateBase
{
    private readonly PlayerController _player;

    public MoveState(PlayerController player)
    {
        _player = player;
    }

    // Move ���� ���� �� ȣ��
    public override void EnterState()
    {

        _player.Animator_Player.SetBool("Moving", true);
        _player.BindInputCallback(true, OnInputCallback);
    }

    // Move ���� ���� �� ȣ��
    public override void ExitState()
    {
        _player.Animator_Player.SetBool("Moving", false);
        _player.BindInputCallback(false, OnInputCallback);

    }

    // Move ���� ���� �� ȣ��
    public override void ExecuteOnUpdate()
    {
        Vector2 moveInput = _player.GetMoveInput(); // �̵� �Է� �ޱ�
        _player.Move(); // �̵� ó�� (�ּ� ó����)

        if (moveInput == Vector2.zero) // �̵� �Է��� ������ Idle ���·� ��ȯ
        {
            _player.ChangeState(new IdleState(_player));
        }
    }

    // �Է� �ݹ� ó��
    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        if (context.action.name == "Atk")
        {
            _player.Animator_Player.SetBool("Moving", false);
            _player.ChangeState(new Atk1State(_player));
        }
    }
}

// Dodge ���� Ŭ����
public class DodgeState : StateBase
{
    private readonly PlayerController _player;

    public DodgeState(PlayerController player)
    {
        _player = player;
    }

    // Dodge ���� ���� �� ȣ��
    public override void EnterState()
    {
        _player.Animator_Player.SetTrigger("Dodge");
    }

    // Dodge ���� ���� �� ȣ��
    public override void ExecuteOnUpdate()
    {
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime > 1) // �ִϸ��̼��� ������ Idle ���·� ��ȯ
        {
            _player.ChangeState(new IdleState(_player));
        }
    }
}

// Item ���� Ŭ����
public class ItemState : StateBase
{
    private readonly PlayerController _player;

    public ItemState(PlayerController player)
    {
        _player = player;
    }

    // Item ���� ���� �� ȣ��
    public override void EnterState()
    {
        _player.Animator_Player.SetTrigger("Use");
    }

    // Item ���� ���� �� ȣ��
    public override void ExecuteOnUpdate()
    {
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime > 1) // �ִϸ��̼��� ������ Idle ���·� ��ȯ
        {
            _player.ChangeState(new IdleState(_player));
        }
    }
}

// Atk1 ���� Ŭ���� (���� ������ ù ��° �ܰ�)
public class Atk1State : StateBase
{
    private readonly PlayerController _player;
    private bool _isCombo; // ���� ���� ���� �÷���

    public Atk1State(PlayerController player)
    {
        _player = player;
        
    }

    // Atk1 ���� ���� �� ȣ��
    public override void EnterState()
    {
        _isCombo = false; // ���� ���� �ʱ�ȭ
        _player.Animator_Player.SetTrigger("Atk1");
        _player.Atk1.Play();
        _player.BindInputCallback(true, OnInputCallback);
    }

    // Atk1 ���� ���� �� ȣ��
    public override void ExitState()
    {
        _player.BindInputCallback(false, OnInputCallback);
        _isCombo = false;
    }

    // Atk1 ���� ���� �� ȣ��
    public override void ExecuteOnUpdate()
    {
        //var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        //if (animInfo.normalizedTime > 0.5f && _isCombo)
        //{
        //    _player.ChangeState(new Atk2State(_player));
        //}
        //else if (animInfo.normalizedTime >= 1) // �ִϸ��̼��� ������ Idle ���·� ��ȯ
        //{
        //    _player.Animator_Player.SetTrigger("Stop");
        //    _player.ChangeState(new IdleState(_player));
        //}
        
    }

    // �Է� �ݹ� ó��
    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        if (context.action.name == "Atk")
        {
            _isCombo = true; // ���� ���� Ȱ��ȭ
        }
    }
    public override void OnAnimationComplete(string animationName)
    {
        //if (animationName == "Atk1")
        //{
        //    // ���� ���·� ��ȯ
        //    _player.ChangeState(new Atk2State(_player));
        //}
        if(_isCombo)
        {
            _player.ChangeState(new Atk2State(_player));
        }
        else
        {
            _player.Animator_Player.SetTrigger("Stop");
            _player.ChangeState(new IdleState(_player));
        }
    }
}

// Atk2 ���� Ŭ���� (���� ������ �� ��° �ܰ�)
public class Atk2State : StateBase
{
    private readonly PlayerController _player;
    private bool _isCombo; // ���� ���� ���� �÷���

    public Atk2State(PlayerController player)
    {
        _player = player;
    }

    // Atk2 ���� ���� �� ȣ��
    public override void EnterState()
    {
        _isCombo = false; // ���� ���� �ʱ�ȭ
        _player.Animator_Player.SetTrigger("Atk2");
        _player.Atk2.Play();

        _player.BindInputCallback(true, OnInputCallback);
    }

    // Atk2 ���� ���� �� ȣ��
    public override void ExitState()
    {
        _player.BindInputCallback(false, OnInputCallback);
        _isCombo = false;
    }

    // Atk2 ���� ���� �� ȣ��
    public override void ExecuteOnUpdate()
    {
        //var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        //if (animInfo.normalizedTime > 0.5f && _isCombo)
        //{
        //    _player.ChangeState(new Atk3State(_player));
        //}
        //else if (animInfo.normalizedTime >= 1) // �ִϸ��̼��� ������ Idle ���·� ��ȯ
        //{
        //    _player.Animator_Player.SetTrigger("Stop");
        //    _player.ChangeState(new IdleState(_player));
        //}
    }

    // �Է� �ݹ� ó��
    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        if (context.action.name == "Atk")
        {
            _isCombo = true; // ���� ���� Ȱ��ȭ
        }
    }
    public override void OnAnimationComplete(string animationName)
    {
        //if (animationName == "Atk1")
        //{
        //    // ���� ���·� ��ȯ
        //    _player.ChangeState(new Atk2State(_player));
        //}
        if (_isCombo)
        {
            _player.ChangeState(new Atk3State(_player));
        }
        else
        {
            _player.Animator_Player.SetTrigger("Stop");
            _player.ChangeState(new IdleState(_player));
        }
    }
}

// Atk3 ���� Ŭ���� (���� ������ �� ��° �ܰ�)
public class Atk3State : StateBase
{
    private readonly PlayerController _player;

    public Atk3State(PlayerController player)
    {
        _player = player;
    }

    // Atk3 ���� ���� �� ȣ��
    public override void EnterState()
    {
        _player.Animator_Player.SetTrigger("Atk3");
        _player.Atk3.Play();
        _player.Atk4.Play();
    }

    // Atk3 ���� ���� �� ȣ��
    public override void ExitState()
    {
        
    }

    // Atk3 ���� ���� �� ȣ��
    public override void ExecuteOnUpdate()
    {
        //var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        //if (animInfo.normalizedTime >= 1) // �ִϸ��̼��� ������ Idle ���·� ��ȯ
        //{
        //    _player.Animator_Player.SetTrigger("Stop");
        //    _player.ChangeState(new IdleState(_player));
        //}
    }

    // �Է� �ݹ� ó��
    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        // Atk3 ���¿����� �߰� ���� �Է��� ���� ����
    }
    public override void OnAnimationComplete(string animationName)
    {
        _player.Animator_Player.SetTrigger("Stop");
        _player.ChangeState(new IdleState(_player));
    }
}
