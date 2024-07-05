using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class MonsterController : MonoBehaviour
{
    public Transform _player;
    public float meleeRange;
    public float castRange;
    public float chaseRange;
    public float stopRange;

    public float moveSpeed = 1.0f;
    public float rotationSpeed = 5.0f;


    public Animator Animator_Monster;
    private M_State _curmstate;

    [SerializeField] Text Text_Temporarl;


    public GameObject M_AttackCollider;
    private void Start()
    {
        
    }
    private void Update()
    {
        _curmstate?.M_ExecuteOnUpdate();
    }
    public void M_ChangeState(M_State m_State)
    {
        _curmstate?.M_ExitState();
        _curmstate = m_State;
        Text_Temporarl.text = _curmstate.ToString();
        _curmstate.M_EnterState();
    }
    public void M_OnAnimationEnd()
    {
        M_ChangeState(new M_IdleState(this));
    }

    //몬스터 패턴
    private float CheckDistance()
    {
        return Vector3.Distance(transform.position, _player.position);
    }

    //공격방식 결정
    public void AttackKind()
    {
        int rand = Random.Range(0, 1);

        if(rand == 0)
        {
            AttackMelee();
        }
        else
        {
            AttackCast();
        }
    }

    //근접공격시
    public void AttackMelee()
    {
        float d = CheckDistance();

        if(d<=meleeRange)
        {

        }
        else
        {

        }
    }

    //원거리공격시
    public void AttackCast()
    {
        float d = CheckDistance();

        if (d <= castRange)
        {

        }
        else
        {

        }
    }

    //플레이어에 다가가기
    public void MoveToPlayer()
    {
        float d = CheckDistance();
        LookPlayer();
        if (meleeRange > d)
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    //플레이어에 멀어지기
    public void MoveAwayPlayer()
    {
        float d= CheckDistance();
        LookPlayer();

        if (castRange <d)
        { 
            Vector3 direction = (transform.position - _player.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    //플레이어 바라보기
    private void LookPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        if (angle > 0)
        {
            Animator_Monster.SetTrigger("TurnRight");
        }
        else
        {
            Animator_Monster.SetTrigger("TurnLeft");
        }
    }

    //플레이어 앞/뒤 감지
    private bool IsPlayerFront()
    {
        Vector3 directionToPlayer= (_player.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

        return dotProduct > 0;
    }
}
