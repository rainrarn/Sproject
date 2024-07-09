using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상태 인터페이스 정의
public interface M_State
{
    void M_EnterState(); // 상태 진입 시 호출되는 메서드
    void M_ExitState(); // 상태 종료 시 호출되는 메서드
    void M_ExecuteOnUpdate(); // 상태 업데이트 시 호출되는 메서드
    void M_Act(string action); // 행동 콜백 메서드
    void M_OnAnimationComplete(string animationName); // 애니메이션 완료 시 호출되는 메서드
}

public abstract class M_StateBase : M_State
{
    public virtual void M_EnterState() { } // 가상 진입 메서드
    public virtual void M_ExitState() { } // 가상 종료 메서드
    public virtual void M_ExecuteOnUpdate() { } // 가상 업데이트 메서드
    public virtual void M_Act(string action) { } // 가상 행동 콜백 메서드
    public virtual void M_OnAnimationComplete(string animationName) { } // 가상 애니메이션 완료 메서드
}

// Idle 상태 클래스
public class M_IdleState : M_StateBase
{
    private readonly MonsterController _monster;

    public M_IdleState(MonsterController monster)
    {
        _monster = monster;
    }

    public override void M_EnterState()
    {
        
    }
    public override void M_ExitState()
    {
        
    }
    
    public override void M_Act(string action)
    {
        if (action == "MeleeAtk1")
        {
            _monster.M_ChangeState(new M_MeleeAtk1State(_monster));
        }
        else if (action == "Move")
        {
            _monster.M_ChangeState(new M_MoveState(_monster));
        }
        else if(action == "CastAtk1")
        {
            _monster.M_ChangeState(new M_CastAtk1State(_monster));
        }
        else if(action == "Look")
        {
            _monster.M_ChangeState(new M_LookState(_monster));
        }
        else if(action == "Chase")
        {
            _monster.M_ChangeState(new M_ChaseState(_monster));
        }
        else if(action == "Back")
        {
            _monster.M_ChangeState(new M_BackState(_monster));
        }
    }
    
        
}

//이동
public class M_MoveState : M_StateBase
{
    private readonly MonsterController _monster;
    public M_MoveState(MonsterController monster)
    {
        _monster = monster;
    }
    public override void M_EnterState()
    {
        _monster.Animator_Monster.SetBool("Move",true);
    }
    public override void M_ExitState()
    {
        _monster.Animator_Monster.SetBool("Move", false);
    }
    public override void M_OnAnimationComplete(string animationName)
    {
        _monster.M_ChangeState(new M_IdleState(_monster));
    }
}

//플레이어 바라보기
public class M_LookState : M_StateBase
{
    private readonly MonsterController _monster;
    public M_LookState(MonsterController monster)
    {
        _monster = monster;
    }
    public override void M_EnterState()
    {
        _monster.Animator_Monster.SetTrigger("Look");
        _monster.LookAtPlayer();
    }
    public override void M_ExitState()
    {
        _monster.Animator_Monster.SetTrigger("StopRot");
    }
    public override void M_OnAnimationComplete(string animationName)
    {
        _monster.M_ChangeState(new M_CastAtk1State(_monster));
    }
}

//플레이어에게 다가가기
public class M_ChaseState : M_StateBase
{
    private readonly MonsterController _monster;
    public M_ChaseState(MonsterController monster)
    {
        _monster = monster;
    }
    public override void M_EnterState()
    {
        _monster.Animator_Monster.SetBool("Chase", true);
    }
    public override void M_ExitState()
    {
        _monster.Animator_Monster.SetBool("Chase", false);
    }
    public override void M_ExecuteOnUpdate()
    {
        _monster.MoveToPlayer();
        if(_monster.CheckDistance() <= _monster.meleeRange)
        {
            _monster.M_ChangeState(new M_MeleeAtk1State(_monster));
        }
    }
}

//플레이어 멀어지기
public class M_BackState : M_StateBase
{
    private readonly MonsterController _monster;
    public M_BackState(MonsterController monster)
    {
        _monster = monster;
    }
    public override void M_EnterState()
    {
        _monster.Animator_Monster.SetBool("Back", true);
    }
    public override void M_ExitState()
    {
        _monster.Animator_Monster.SetBool("Back", false);
    }
    public override void M_ExecuteOnUpdate()
    {
        _monster.MoveAwayPlayer();
        if (_monster.CheckDistance() >= _monster.castRange)
        {
            _monster.M_ChangeState(new M_LookState(_monster));
        }
    }
}
//근접공격1
public class M_MeleeAtk1State : M_StateBase
{
    private readonly MonsterController _monster;
    public M_MeleeAtk1State(MonsterController monster)
    {
        _monster = monster;
    }
    public override void M_EnterState()
    {
        _monster.Animator_Monster.SetTrigger("MeleeAtk1");
        _monster.MeleeAtk1Collider.SetActive(true);
    }
    public override void M_ExitState()
    {
        _monster.MeleeAtk1Collider.SetActive(false);
    }
    public override void M_OnAnimationComplete(string animationName)
    {
        _monster.Animator_Monster.SetTrigger("Stop");
        _monster.M_ChangeState(new M_IdleState(_monster));
    }
}

//원거리공격1
public class M_CastAtk1State : M_StateBase
{
    private readonly MonsterController _monster;
    public M_CastAtk1State(MonsterController monster)
    {
        _monster = monster;
    }
    public override void M_EnterState()
    {
        _monster.Animator_Monster.SetTrigger("CastAtk1");
        _monster.BeforeBreath();


    }
    public override void M_ExitState()
    {
        _monster.EndBreath();
    }
    public override void M_OnAnimationComplete(string animationName)
    {
        _monster.Animator_Monster.SetTrigger("Stop");
        _monster.M_ChangeState(new M_IdleState(_monster));
    }
}

