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
        else if(action =="CastAtk1")
        {
            _monster.M_ChangeState(new M_CastAtk1State(_monster));
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

//추격
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
        _monster.M_AttackCollider.SetActive(true);
    }
    public override void M_ExitState()
    {
        _monster.M_AttackCollider.SetActive(false);
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
        _monster.Animator_Monster.SetTrigger("CastAtk1State");
        _monster.M_AttackCollider.SetActive(true);
    }
    public override void M_ExitState()
    {
        _monster.M_AttackCollider.SetActive(false);
    }
    public override void M_OnAnimationComplete(string animationName)
    {
        _monster.Animator_Monster.SetTrigger("Stop");
        _monster.M_ChangeState(new M_IdleState(_monster));
    }
}

