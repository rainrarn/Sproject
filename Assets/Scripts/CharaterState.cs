using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

// 상태 인터페이스 정의
public interface IState
{
    void EnterState(); // 상태 진입 시 호출되는 메소드
    void ExitState(); // 상태 종료 시 호출되는 메소드
    void ExecuteOnUpdate(); // 상태 갱신 시 호출되는 메소드
    void OnInputCallback(InputAction.CallbackContext context); // 입력 콜백 메소드
}

// 상태 기본 클래스
public class StateBase : IState
{
    public virtual void EnterState() { } // 가상 진입 메소드
    public virtual void ExitState() { } // 가상 종료 메소드
    public virtual void ExecuteOnUpdate() { } // 가상 갱신 메소드
    public virtual void OnInputCallback(InputAction.CallbackContext context) { } // 가상 입력 콜백 메소드
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
        _player.Move(); // 이동 처리 (주석 처리됨)

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
        _player.Animator_Player.SetTrigger("Dodge");
    }

    // Dodge 상태 갱신 시 호출
    public override void ExecuteOnUpdate()
    {
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime > 1) // 애니메이션이 끝나면 Idle 상태로 전환
        {
            _player.ChangeState(new IdleState(_player));
        }
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
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime > 1) // 애니메이션이 끝나면 Idle 상태로 전환
        {
            _player.ChangeState(new IdleState(_player));
        }
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
        _player.BindInputCallback(true, OnInputCallback);
    }

    // Atk1 상태 종료 시 호출
    public override void ExitState()
    {
        _player.BindInputCallback(false, OnInputCallback);
        _isCombo = false;
    }

    // Atk1 상태 갱신 시 호출
    public override void ExecuteOnUpdate()
    {
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime > 0.7f && _isCombo)
        {
            _player.ChangeState(new Atk2State(_player));
        }
        else if (animInfo.normalizedTime >= 1) // 애니메이션이 끝나면 Idle 상태로 전환
        {
            _player.Animator_Player.SetTrigger("Stop");
            _player.ChangeState(new IdleState(_player));
        }
    }

    // 입력 콜백 처리
    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        if (context.action.name == "Atk")
        {
            _isCombo = true; // 연속 공격 활성화
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
        Debug.LogWarning("어택2들어옴");

        _player.BindInputCallback(true, OnInputCallback);
    }

    // Atk2 상태 종료 시 호출
    public override void ExitState()
    {
        _player.BindInputCallback(false, OnInputCallback);
        _isCombo = false;
    }

    // Atk2 상태 갱신 시 호출
    public override void ExecuteOnUpdate()
    {
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime > 0.7f && _isCombo)
        {
            _player.ChangeState(new Atk3State(_player));
        }
        else if (animInfo.normalizedTime >= 1) // 애니메이션이 끝나면 Idle 상태로 전환
        {
            _player.Animator_Player.SetTrigger("Stop");
            _player.ChangeState(new IdleState(_player));
        }
    }

    // 입력 콜백 처리
    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        if (context.action.name == "Atk")
        {
            _isCombo = true; // 연속 공격 활성화
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
    }

    // Atk3 상태 종료 시 호출
    public override void ExitState()
    {
        
    }

    // Atk3 상태 갱신 시 호출
    public override void ExecuteOnUpdate()
    {
        var animInfo = _player.Animator_Player.GetCurrentAnimatorStateInfo(0);
        if (animInfo.normalizedTime >= 1) // 애니메이션이 끝나면 Idle 상태로 전환
        {
            _player.Animator_Player.SetTrigger("Stop");
            _player.ChangeState(new IdleState(_player));
        }
    }

    // 입력 콜백 처리
    public override void OnInputCallback(InputAction.CallbackContext context)
    {
        // Atk3 상태에서는 추가 공격 입력을 받지 않음
    }
}
