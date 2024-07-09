using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 상태 인터페이스 정의
public interface IState
{
    void EnterState(); // 상태 진입 시 호출되는 메서드
    void ExitState(); // 상태 종료 시 호출되는 메서드
    void ExecuteOnUpdate(); // 상태 업데이트 시 호출되는 메서드
    void OnInputCallback(InputAction.CallbackContext context); // 입력 콜백 메서드
    void OnAnimationComplete(string animationName); // 애니메이션 완료 시 호출되는 메서드
}

public abstract class StateBase : IState
{
    public virtual void EnterState() { } // 가상 진입 메서드
    public virtual void ExitState() { } // 가상 종료 메서드
    public virtual void ExecuteOnUpdate() { } // 가상 업데이트 메서드
    public virtual void OnInputCallback(InputAction.CallbackContext context) { } // 가상 입력 콜백 메서드
    public virtual void OnAnimationComplete(string animationName) { } // 가상 애니메이션 완료 메서드
}

// Idle 상태 클래스
public class IdleState : StateBase
{
    private readonly PlayerController _player;

    public IdleState(PlayerController player)
    {
        _player = player;
    }

    // Idle 상태 진입 시 호출
    public override void EnterState()
    {
        _player.BindInputCallback(true, OnInputCallback);
    }

    // Idle 상태 종료 시 호출
    public override void ExitState()
    {
        _player.BindInputCallback(false, OnInputCallback);
    }

    // 입력 콜백 처리
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
        if (context.action.name == "Guard")
        {
            _player.ChangeState(new GuardState(_player));
        }
        if (context.action.name == "Skill1")
        {
            _player.ChangeState(new Skill1State(_player));
        }
    }
}

// Move 상태 클래스
public class MoveState : StateBase
{
    private readonly PlayerController _player;

    public MoveState(PlayerController player)
    {
        _player = player;
    }

    // Move 상태 진입 시 호출
    public override void EnterState()
    {

        _player.Animator_Player.SetBool("Moving", true);
        _player.BindInputCallback(true, OnInputCallback);
    }

    // Move 상태 종료 시 호출
    public override void ExitState()
    {
        _player.Animator_Player.SetBool("Moving", false);
        _player.BindInputCallback(false, OnInputCallback);

    }

    // Move 상태 갱신 시 호출
    public override void ExecuteOnUpdate()
    {
        Vector2 moveInput = _player.GetMoveInput(); // 이동 입력 받기
        _player.Move(); // 이동 처리

        if (moveInput == Vector2.zero) // 이동 입력이 없으면 Idle 상태로 전환
        {
            _player.ChangeState(new IdleState(_player));
        }
    }

    // 입력 콜백 처리
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

// Dodge 상태 클래스
public class DodgeState : StateBase
{
    private readonly PlayerController _player;

    public DodgeState(PlayerController player)
    {
        _player = player;
    }

    // Dodge 상태 진입 시 호출
    public override void EnterState()
    {
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
        _player.GuardPose.SetActive(true);
        _player.GuardEffect.Play();
        _player.BindInputCallback(true, OnInputCallback);
        //_player.Animator_Player.SetTrigger("Parry");
        _player.Parry();
        _player.Animator_Player.SetBool("Guard", true);
    }

    public override void ExitState()
    {
        _player.BindInputCallback(false, OnInputCallback);
        _player.EndGuard();
        _player.Animator_Player.SetBool("Guard", false);
        _player.GuardEffect.Stop();
        _player.GuardPose.SetActive(false);
        
    }
    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        Debug.LogWarning($"Canceled!!!!! ==================={context.canceled}");


        if(context.canceled)
        {
            _player.ChangeState(new IdleState(_player));
        }
    }
    public override void OnAnimationComplete(string animationName)
    {
        //_player.Animator_Player.SetTrigger("Stop");
        
        _player.Guard();
    }
}
// Item 상태 클래스
public class ItemState : StateBase
{
    private readonly PlayerController _player;

    public ItemState(PlayerController player)
    {
        _player = player;
    }

    // Item 상태 진입 시 호출
    public override void EnterState()
    {
        _player.Animator_Player.SetTrigger("Use");
    }

    // Item 상태 갱신 시 호출
    public override void ExecuteOnUpdate()
    {
        
            _player.ChangeState(new IdleState(_player));
    }
}

// Atk1 상태 클래스 (연속 공격의 첫 번째 단계)
public class Atk1State : StateBase
{
    private readonly PlayerController _player;
    private bool _isCombo; // 연속 공격 여부 플래그

    public Atk1State(PlayerController player)
    {
        _player = player;
        
    }

    // Atk1 상태 진입 시 호출
    public override void EnterState()
    {
        _isCombo = false; // 연속 공격 초기화
        _player.Animator_Player.SetTrigger("Atk1");
        _player.Atk1.Play();
        _player.BindInputCallback(true, OnInputCallback);
        _player.AtkCollider.SetActive(true);
    }

    // Atk1 상태 종료 시 호출
    public override void ExitState()
    {
        _player.AtkCollider.SetActive(false);
        _player.BindInputCallback(false, OnInputCallback);
        _isCombo = false;
    }

    // Atk1 상태 갱신 시 호출
    public override void ExecuteOnUpdate()
    {
    }

    // 입력 콜백 처리
    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        if (context.action.name == "Atk")
        {
            _isCombo = true; // 연속 공격 활성화
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

// Atk2 상태 클래스 (연속 공격의 두 번째 단계)
public class Atk2State : StateBase
{
    private readonly PlayerController _player;
    private bool _isCombo; // 연속 공격 여부 플래그

    public Atk2State(PlayerController player)
    {
        _player = player;
    }

    // Atk2 상태 진입 시 호출
    public override void EnterState()
    {
        _isCombo = false; // 연속 공격 초기화
        _player.Animator_Player.SetTrigger("Atk2");
        _player.Atk2.Play();
        _player.AtkCollider.SetActive(true);
        _player.BindInputCallback(true, OnInputCallback);
    }

    // Atk2 상태 종료 시 호출
    public override void ExitState()
    {
        _player.AtkCollider.SetActive(false);
        _player.BindInputCallback(false, OnInputCallback);
        _isCombo = false;
    }

    // Atk2 상태 갱신 시 호출
    public override void ExecuteOnUpdate()
    {
    }

    // 입력 콜백 처리
    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        if (context.action.name == "Atk")
        {
            _isCombo = true; // 연속 공격 활성화
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

// Atk3 상태 클래스 (연속 공격의 세 번째 단계)
public class Atk3State : StateBase
{
    private readonly PlayerController _player;

    public Atk3State(PlayerController player)
    {
        _player = player;
    }

    // Atk3 상태 진입 시 호출
    public override void EnterState()
    {
        _player.Animator_Player.SetTrigger("Atk3");
        _player.Atk3.Play();
        _player.Atk4.Play();
        _player.AtkCollider.SetActive(true);
    }

    // Atk3 상태 종료 시 호출
    public override void ExitState()
    {
        _player.AtkCollider.SetActive(false);
    }

    // Atk3 상태 갱신 시 호출
    public override void ExecuteOnUpdate()
    {
        
    }

    // 입력 콜백 처리
    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        // Atk3 상태에서는 추가 공격 입력을 받지 않음
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
        //_player.Jump();
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
