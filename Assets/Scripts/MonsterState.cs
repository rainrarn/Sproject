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
        else if(action =="CastAtk1")
        {
            _monster.M_ChangeState(new M_CastAtk1State(_monster));
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

//�߰�
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

