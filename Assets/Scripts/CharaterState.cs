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
        switch (context.action.name)
        {
            case "Atk":
                _player.ChangeState(new Atk1State(_player));
                break;
            case "Move":
                _player.ChangeState(new MoveState(_player));
                break;
            case "Dodge":
                _player.ChangeState(new DodgeState(_player));
                break;
            case "Item":
                _player.ChangeState(new ItemState(_player));
                break;
            case "Guard":
                _player.ChangeState(new GuardState(_player));
                break;
            case "Skill1":
                if (PlayerStatManager.instance._cristalcount > 0)
                {
                    _player.ChangeState(new Skill1State(_player));
                }
                break;
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
        _player.Move(); // �̵� ó��

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
        else if (context.action.name == "Dodge")
        {
            _player.Animator_Player.SetBool("Moving", false);
            _player.ChangeState(new DodgeState(_player));
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
        PlayerStatManager.instance.SpendStamina(20);
        _player.Dodge();
        _player.PlayerCollider.SetActive(false);
        _player.Animator_Player.SetTrigger("Dodge");

    }

 

    public override void OnAnimationComplete(string animationName)
    {
        _player.Animator_Player.SetTrigger("Stop");
        _player.PlayerCollider.SetActive(true);
        _player.ChangeState(new IdleState(_player));
    }
}
public class ParryState : StateBase
{
    private readonly PlayerController _player;

    public ParryState(PlayerController player)
    {
        _player = player;
    }

    public override void EnterState()
    {
        PlayerStatManager.instance.SpendStamina(20);
        _player.Animator_Player.SetTrigger("Parry");
        _player.Parry();
    }
    public override void ExitState()
    {
        _player.Animator_Player.SetTrigger("Stop");
    }

    public override void ExecuteOnUpdate()
    {

    }
    public override void OnAnimationComplete(string animationName)
    {
        _player.ChangeState(new GuardState(_player));
    }
}
public class GuardState : StateBase
{
    private readonly PlayerController _player;

    public GuardState(PlayerController player)
    {
        _player = player;
    }

    public override void EnterState()
    {
        PlayerStatManager.instance.SpendStamina(20);
        _player.GuardPose.SetActive(true);
        _player.GuardEffect.Play();
        _player.Parry();
        _player.Animator_Player.SetBool("Guard", true);
    }

    public override void ExitState()
    {
        _player.EndGuard();
        _player.Animator_Player.SetBool("Guard", false);
        _player.GuardEffect.Stop();
        _player.GuardPose.SetActive(false);
        
    }

    public override void OnAnimationComplete(string animationName)
    {
        _player.ChangeState(new IdleState(_player));
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
        _player.Animator_Player.SetTrigger("Drink");
        PlayerStatManager.instance.UsePotion();

    }
    public override void ExitState()
    {
        _player.Animator_Player.SetTrigger("Stop");
    }

    public override void OnAnimationComplete(string animationName)
    {
            
            _player.ChangeState(new IdleState(_player));

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
        _player.Atk1Collider.SetActive(true);
        PlayerStatManager.instance.SpendStamina(10);
    }

    // Atk1 ���� ���� �� ȣ��
    public override void ExitState()
    {
        _player.Atk1Collider.SetActive(false);
        _player.BindInputCallback(false, OnInputCallback);
        _isCombo = false;
    }

    // Atk1 ���� ���� �� ȣ��
    public override void ExecuteOnUpdate()
    {
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
        _player.Atk2Collider.SetActive(true);
        _player.BindInputCallback(true, OnInputCallback);
        PlayerStatManager.instance.SpendStamina(10);
    }

    // Atk2 ���� ���� �� ȣ��
    public override void ExitState()
    {
        _player.Atk2Collider.SetActive(false);
        _player.BindInputCallback(false, OnInputCallback);
        _isCombo = false;
    }

    // Atk2 ���� ���� �� ȣ��
    public override void ExecuteOnUpdate()
    {
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
        PlayerStatManager.instance.SpendStamina(10);
        _player.Animator_Player.SetTrigger("Atk3");
        _player.Atk3.Play();
        _player.Atk4.Play();
        _player.Atk3Collider.SetActive(true);
    }

    // Atk3 ���� ���� �� ȣ��
    public override void ExitState()
    {
        _player.Atk3Collider.SetActive(false);
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

public class Skill1State : StateBase
{
    private readonly PlayerController _player;

    public Skill1State(PlayerController player)
    {
        _player = player;

    }

    public override void EnterState()
    {
        PlayerStatManager.instance.MinusCristal();
        PlayerStatManager.instance.SpendStamina(30);
        _player.Jump();
        _player.Animator_Player.SetTrigger("Skill1");
        _player.Skill1Effect();
        
        //_player.Skill1Collider1.SetActive(true);
    }
    public override void ExitState()
    {
        //_player.Skill1Collider1.SetActive(false);
        _player.Animator_Player.SetTrigger("Stop");
    }
    public override void ExecuteOnUpdate()
    {
    }


    public override void OnAnimationComplete(string animationName)
    {
        _player.StopSkill1();
        _player.Animator_Player.SetTrigger("Stop");
        _player.ChangeState(new IdleState(_player));
    }
}

public class HitState : StateBase
{
    private readonly PlayerController _player;

    public HitState(PlayerController player)
    {
        _player = player;
    }
    public override void EnterState()
    {
        _player.PlayerCollider.SetActive(false);
        _player.Animator_Player.SetBool("Hit",true);
    }
    public override void ExitState()
    {
        _player.Animator_Player.SetBool("Hit", false);
    }
    public override void OnAnimationComplete(string animationName)
    {
        _player.ChangeState(new RiseState(_player));
    }
}
public class RiseState : StateBase
{
    private readonly PlayerController _player;

    public RiseState(PlayerController player)
    {
        _player = player;
    }
    public override void EnterState()
    {
        _player.Animator_Player.SetBool("Rise",true);
    }
    public override void ExitState()
    {
        _player.Animator_Player.SetBool("Rise", false);
    }
    public override void OnAnimationComplete(string animationName)
    {
        _player.ChangeState(new IdleState(_player));
    }
}

public class DeadState : StateBase
{
    private readonly PlayerController _player;

    public DeadState(PlayerController player)
    {
        _player = player;
    }
    public override void EnterState()
    {
        _player.Animator_Player.SetBool("Dead",true);
        _player.DeathDelay();
    }
    public override void OnAnimationComplete(string animationName)
    {

    }
}

enum StateName
{

}