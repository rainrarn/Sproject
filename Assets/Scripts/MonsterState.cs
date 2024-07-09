using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���� �������̽� ����
public interface M_State
{
    void M_EnterState(); // ���� ���� �� ȣ��Ǵ� �޼���
    void M_ExitState(); // ���� ���� �� ȣ��Ǵ� �޼���
    void M_ExecuteOnUpdate(); // ���� ������Ʈ �� ȣ��Ǵ� �޼���
    void M_Act(string action); // �ൿ �ݹ� �޼���
    void M_OnAnimationComplete(string animationName); // �ִϸ��̼� �Ϸ� �� ȣ��Ǵ� �޼���
}

public abstract class M_StateBase : M_State
{
    public virtual void M_EnterState() { } // ���� ���� �޼���
    public virtual void M_ExitState() { } // ���� ���� �޼���
    public virtual void M_ExecuteOnUpdate() { } // ���� ������Ʈ �޼���
    public virtual void M_Act(string action) { } // ���� �ൿ �ݹ� �޼���
    public virtual void M_OnAnimationComplete(string animationName) { } // ���� �ִϸ��̼� �Ϸ� �޼���
}

// Idle ���� Ŭ����
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

//�̵�
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

//�÷��̾� �ٶ󺸱�
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

//�÷��̾�� �ٰ�����
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

//�÷��̾� �־�����
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
//��������1
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

//���Ÿ�����1
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

