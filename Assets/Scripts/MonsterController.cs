using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
public class MonsterController : MonoBehaviour
{
    public Transform _player;
    public float meleeRange;
    public float castRange;
    public float chaseRange;
    public float stopRange;

    public float moveSpeed = 1.0f;
    public float backSpeed = 5.0f;
    public float rotationSpeed = 100.0f;

    private bool isRotating = false;

    public Animator Animator_Monster;
    private M_State _curmstate;

    [SerializeField] Text Text_Temporarl;

    public ParticleSystem Breath;
    [SerializeField] Transform BreathPose;
    public GameObject MeleeAtk1Collider;
    public float playDuration = 5.0f; // ��ƼŬ�� ����� �ð�
    public float BeforePlay = 2.0f;

    public bool isPatternRunning = false;
    private void Start()
    {
        meleeRange = 5.0f;
        castRange = 10.0f;

        M_ChangeState(new M_IdleState(this));

        
    }
    private void Update()
    {
        _curmstate?.M_ExecuteOnUpdate();

        if(Input.GetKeyDown(KeyCode.Q))
        {
            //_curmstate.M_Act("Chase");
            AttackMelee();
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            //AttackMelee();
            _curmstate.M_Act("CastAtk1");
        }
        if (Input.GetKeyDown(KeyCode.R))
        {
            StartCoroutine(RandomPatternEveryHour());
        }
        if (Input.GetKeyDown(KeyCode.T))
        {
            StopCoroutine(RandomPatternEveryHour());
        }

        if(MonsterStatManager.M_instance._hp<=0)
        {
            _curmstate.M_Act("Dead");
        }

    }
    IEnumerator RandomPatternEveryHour()
    {
        while (true)
        {

            yield return new WaitForSeconds(5);
            if (isPatternRunning)
            {
                continue;
            }

            // �������� ���� ����
            int randomPattern = Random.Range(0, 2); // 0 �Ǵ� 1

            if (randomPattern == 0)
            {
                AttackMelee();
            }
            else
            {
                _curmstate.M_Act("CastAtk1");
            }
        }
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

    //���� ����
    public float CheckDistance()
    {
        return Vector3.Distance(transform.position, _player.position);
    }

    //���ݹ�� ����
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

    //�������ݽ�
    public void AttackMelee()
    {
        float d = CheckDistance();

        if(d > meleeRange)
        {
            _curmstate.M_Act("Chase");
            //M_ChangeState(new M_ChaseState(this));
        }
        else
        {
            M_ChangeState(new M_MeleeAtk1State(this));
        }
    }

    //���Ÿ����ݽ�
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

    //�÷��̾ �ٰ�����
    public void MoveToPlayer()
    {
        float d = CheckDistance();
        LookPlayer();
        if (meleeRange < d)
        {
            Vector3 direction = (_player.position - transform.position).normalized;
            transform.position += direction * moveSpeed * Time.deltaTime;
        }
    }

    //�÷��̾ �־�����
    public void MoveAwayPlayer()
    {
        float d= CheckDistance();
        LookPlayer();

        if (castRange <d)
        { 
            Vector3 direction = (transform.position - _player.position).normalized;
            transform.position += direction * backSpeed * Time.deltaTime*5;
        }
    }

    //�÷��̾� �ٶ󺸱�
    public void LookPlayer()
    {
        Vector3 direction = (_player.position - transform.position).normalized;
        Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z));

        float angle = Vector3.SignedAngle(transform.forward, direction, Vector3.up);


        transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime*10);
    }

    //�÷��̾� ��/�� ����
    private bool IsPlayerFront()
    {
        Vector3 directionToPlayer= (_player.position - transform.position).normalized;
        float dotProduct = Vector3.Dot(transform.forward, directionToPlayer);

        return dotProduct > 0;
    }

    public void MeleeAtk1()
    {

    }
    public float rotationDuration = 1f; // ȸ�� ���� �ð�
    public void LookAtPlayer()
    {
        if (!isRotating)
        {
            isRotating = true;
            Vector3 direction = (_player.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0, direction.z)); // Y�� ȸ���� ���

            transform.DORotateQuaternion(lookRotation, rotationDuration).OnComplete(() =>
            {
                isRotating = false;
            });
        }
    }
    public void M_OnAnimationComplete(string animationName)
    {
        _curmstate.M_OnAnimationComplete(animationName);
    }

    public void BeforeBreath()
    {
        StartCoroutine(BPlayParticle());
    }
    IEnumerator BPlayParticle()
    {
        yield return new WaitForSeconds(BeforePlay); // ������ �ð� ���� ���
        Breath.Play(); // ��ƼŬ ���
        
        
    }
    public void EndBreath()
    {
        StartCoroutine(PlayParticle());
    }
    IEnumerator PlayParticle()
    {

        //Breath.Play(); // ��ƼŬ ���
        yield return new WaitForSeconds(playDuration); // ������ �ð� ���� ���
        Breath.Stop(); // ��ƼŬ ����
    }
}
