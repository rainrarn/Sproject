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

    private bool isRotating = false;

    public Animator Animator_Monster;
    private M_State _curmstate;

    [SerializeField] Text Text_Temporarl;


    public GameObject MeleeAtk1Collider;


    private void Start()
    {
        meleeRange = 5.0f;
        castRange = 20.0f;

        M_ChangeState(new M_IdleState(this));
    }
    private void Update()
    {
        _curmstate?.M_ExecuteOnUpdate();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            _curmstate.M_Act("MeleeAtk1");
        }
      
            LookPlayer();




    }
    public void M_ChangeState(M_State m_State)
    {
        _curmstate?.M_ExitState();
        _curmstate = m_State;
        _curmstate.M_EnterState();
    }
    public void M_OnAnimationEnd()
    {
        M_ChangeState(new M_IdleState(this));
    }

    //몬스터 패턴
    public float CheckDistance()
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

        if(d > meleeRange)
        {
            M_ChangeState(new M_ChaseState(this));
        }
        else
        {
            M_ChangeState(new M_MeleeAtk1State(this));
        }
    }

    //원거리공격시
    public void AttackCast()
    {
        float d = CheckDistance();

        if (d < castRange)
        {
            M_ChangeState(new M_BackState(this));
        }
        else
        {
            M_ChangeState(new M_CastAtk1State(this));
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
    public void LookPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);


        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*10);
    }

    //플레이어 앞/뒤 감지
    private bool IsPlayerFront()
    {
        Vector3 directionToPlayer= (_player.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

        return dotProduct > 0;
    }

    public void MeleeAtk1()
    {

    }
    private void LookAtPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Y축 회전만 고려

        // 현재 회전 상태와 목표 회전 상태의 차이를 계산
        float angleDifference = Quaternion.Angle(transform.rotation, lookRotation);

        // 회전 방향을 구분하여 애니메이션 트리거 설정
        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);
        if (!isRotating && angleDifference > 0.1f)
        {
            isRotating = true;
            Animator_Monster.SetBool("IsRotating", true);
            if (angle > 0)
            {
                Animator_Monster.SetBool("TurnRight", true);
                Animator_Monster.SetBool("TurnLeft", false);
            }
            else
            {
                Animator_Monster.SetBool("TurnRight", false);
                Animator_Monster.SetBool("TurnLeft", true);
            }
        }

        // 회전 완료 시 애니메이션 트리거 해제
        if (isRotating && angleDifference <= 0.1f)
        {
            isRotating = false;
            Animator_Monster.SetBool("IsRotating", false);
            Animator_Monster.SetBool("TurnRight", false);
            Animator_Monster.SetBool("TurnLeft", false);
        }

        // 부드럽게 회전
        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * rotationSpeed);
    }

}
