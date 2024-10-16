using System;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Cinemachine;
using UnityEngine.SceneManagement;
public class PlayerController : MonoBehaviour
{
    public Animator Animator_Player;
    private IState _curState;
    private Action<InputAction.CallbackContext> _inputCallback;
    private Vector2 _moveInput;

    [SerializeField] Text Text_TemporalState;

    public float MoveSpeed = 2.0f;
    [Range(0.0f, 0.3f)]
    public float RotationSmoothTime = 0.12f;
    public AudioClip LandingAudioClip;
    public AudioClip[] FootstepAudioClips;
    [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

    public GameObject DeadCamera;
    public CinemachineVirtualCamera DeadCM;

    public GameObject CinemachineCameraTarget;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;
    public bool LockCameraPosition = false;
    public float CameraRotationSpeed = 2.0f;

    public Transform Monster; // ������ ��ġ�� ��Ÿ���� Transform

    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    private float _speed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;

    public float dodgeDistance = 20.0f; // ȸ�� �� �����ϴ� �Ÿ�
    public float dodgeDuration = 0.5f; // ȸ�� �ð�
    public float jumpHeight = 1.0f; // ���� ����
    public float jumpDuration = 1.0f; // ���� �ð�
    private bool isDodging = false;
    private bool isJumping = false;
    private bool isDead = false;
    private CharacterController _controller;

    public ParticleSystem Atk1;
    public ParticleSystem Atk2;
    public ParticleSystem Atk3;
    public ParticleSystem Atk4;

    public ParticleSystem GuardEffect;
    public GameObject GuardPose;

    public ParticleSystem Skill1_1;
    public ParticleSystem Skill1_2;
    public GameObject Skill1_1obj;
    public GameObject Skill1_2obj;

    public ParticleSystem PotionEffect;


    private bool stopParticles = false;
    public GameObject Atk1Collider;
    public GameObject Atk2Collider;
    public GameObject Atk3Collider;
    public GameObject Skill1_1Collider;
    public GameObject Skill1_2Collider;

    public GameObject PlayerCollider;
    public GameObject ParryCollider;
    public GameObject GuardCollider;
    public bool islooked = false;

    public string Atk; // ����� ����Ʈ �±�
    public string Skill1;
    public float effectDuration = 2.0f; // ����Ʈ�� ������ �ð�

    private CharacterController characterController;
    [SerializeField]
    private Canvas _canvas;
    [SerializeField]
    private GameObject LookMark;
    private void Start()
    {
        ChangeState(new IdleState(this));
        characterController = GetComponent<CharacterController>();
        _controller = GetComponent<CharacterController>();
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        _curState?.ExecuteOnUpdate();
        if (isDead == false)
        {
            CameraRotation();
        }
        
        if (Input.GetKeyDown(KeyCode.C)) // C Ű�� ������ �� ���� ������ ī�޶� ȸ��
        {
            if (islooked == false)
            {
                islooked = true;
                LookMark.SetActive(true);
            }
            else
            {
                islooked = false;
                LookMark.SetActive(false);
            }
        }
        if (islooked)
            RotateCameraTowardsMonster();

        LookMark.transform.LookAt(transform);

        if(isDead ==false && PlayerStatManager.instance._hp<=0)
        {
            ChangeState(new DeadState(this));
            
        }

        if (Input.GetKeyDown(KeyCode.E)&&PlayerStatManager.instance._hp<PlayerStatManager.instance._maxHp) 
        {
            UsingPotion();
        }
    }

    public void ChangeState(IState newState)
    {
        // AtkState���� MoveState�� ��ȯ�� ����
        if ((_curState is Atk1State|| _curState is Atk2State|| _curState is Atk3State) && newState is MoveState)
        {
            Debug.Log("Cannot transition from AtkState to MoveState");
            return;
        }
        else if(_curState is DodgeState && newState is MoveState)
        {
            Debug.Log("ȸ���� �̵��Ұ�");
            return;
        }
        else if (_curState is Skill1State && newState is MoveState)
        {
            return;
        }
        else if ((_curState is HitState||_curState is RiseState) && newState is MoveState)
        {
            return;
        }
        _curState?.ExitState(); 
        _curState = newState;
        Text_TemporalState.text = _curState.ToString();
        _curState.EnterState();
        
    }

    public void BindInputCallback(bool isBind, Action<InputAction.CallbackContext> callback)
    {
        if (isBind)

            _inputCallback += callback;
        else
            _inputCallback -= callback;
    }

    public void OnActionInput(InputAction.CallbackContext context)
    {
        if (context.started == false)
            return;

        //Debug.Log("==========�Է� �߻���");

        _inputCallback?.Invoke(context);
    }

    public Vector2 GetMoveInput()
    {
        return _moveInput;
    }
    public void OnAnimationEnd()
    {
        // �ִϸ��̼� ���� �� Idle ���·� ��ȯ
        ChangeState(new IdleState(this));
    }
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        _moveInput = context.ReadValue<Vector2>();
        // Move ���·� ��ȯ
        if (_moveInput != Vector2.zero && !(_curState is MoveState))
        {
            ChangeState(new MoveState(this));
        }
    }

    public void Move()
    {
        // �̵� Ű �Է��� ���� Ȯ��
        bool isMoving = false;
        Vector3 move = Vector3.zero;

        if (Input.GetKey(KeyCode.W))
        {
            move += Vector3.forward;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.S))
        {
            move += Vector3.back;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.A))
        {
            move += Vector3.left;
            isMoving = true;
        }
        if (Input.GetKey(KeyCode.D))
        {
            move += Vector3.right;
            isMoving = true;
        }

        move = move.normalized;

        if (isMoving)
        {
            _targetRotation = Mathf.Atan2(move.x, move.z) * Mathf.Rad2Deg + CinemachineCameraTarget.transform.eulerAngles.y;
            float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

            transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            _speed = MoveSpeed;
        }
        else
        {
            _speed = 0.0f;
        }

        _controller.Move(transform.forward * _speed * Time.deltaTime);
    }

    private void CameraRotation()
    {
        if (LockCameraPosition) return;

        // ���콺 �Է��� ����Ͽ� ī�޶� ȸ���� ó���մϴ�.
        float mouseX = Input.GetAxis("Mouse X") * CameraRotationSpeed;
        float mouseY = Input.GetAxis("Mouse Y") * CameraRotationSpeed;

        _cinemachineTargetYaw += mouseX;
        _cinemachineTargetPitch -= mouseY;
        _cinemachineTargetPitch = Mathf.Clamp(_cinemachineTargetPitch, BottomClamp, TopClamp);

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
    }

    private void RotateCameraTowardsMonster()
    {
        if (Monster == null) return;

        Vector3 directionToMonster = (Monster.position - transform.position).normalized;
        float targetYaw = Mathf.Atan2(directionToMonster.x, directionToMonster.z) * Mathf.Rad2Deg;
        
        _cinemachineTargetYaw = targetYaw;
        _cinemachineTargetPitch = 0.0f; // �ʿ信 ���� ����

        CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch, _cinemachineTargetYaw, 0.0f);
    }

    public void Dodge()
    {
        isDodging = true;
        Vector3 dodgeDirection = transform.forward * dodgeDistance; // ���� ĳ���Ͱ� �ٶ󺸴� �������� ����
        Vector3 targetPosition = transform.position + dodgeDirection;

        // DOTween�� ����Ͽ� CharacterController�� ���� �̵�
        DOTween.To(() => transform.position, x => _controller.Move((x - transform.position) * Time.deltaTime), targetPosition, dodgeDuration)
            .SetEase(Ease.OutQuad)
            .OnComplete(() =>
            {
                isDodging = false;
            });
    }
    public void Jump()
    {
        isJumping = true;

        // �÷��̾��� ���� ��ġ
        Vector3 startPosition = transform.position;
        Vector3 jumpPosition = new Vector3(startPosition.x, startPosition.y + jumpHeight, startPosition.z);

        // ���� �̵�
        DOTween.To(() => startPosition, x => {
            Vector3 delta = x - transform.position;
            characterController.Move(delta);
        }, jumpPosition, jumpDuration / 2).SetEase(Ease.OutQuad)
        .OnComplete(() => {
            // �ٽ� �Ʒ��� �̵�
            DOTween.To(() => jumpPosition, x => {
                Vector3 delta = x - transform.position;
                characterController.Move(delta);
            }, startPosition, jumpDuration / 2).SetEase(Ease.InQuad)
            .OnComplete(() => {
                isJumping = false;
            });
        });
    }

    public void Skill1Effect()
    {
        Skill1_1obj.SetActive(true);
        Skill1_2obj.SetActive(true);
        StartCoroutine(Skill1Particle());
    }
    public void StopSkill1()
    {
        Skill1_1obj.SetActive(false);
        Skill1_2obj.SetActive(false);
        Skill1_1Collider.SetActive(false);
        Skill1_2Collider.SetActive(false);
        StopCoroutine(Skill1Particle());
    }
    private IEnumerator Skill1Particle()
    {
        while (!stopParticles)
        {
            yield return new WaitForSeconds(0.2f);
            // a ��ƼŬ 0.3�� �������� 3�� ���
            for (int i = 0; i < 2; i++)
            {
                Skill1_1.Play();
                Skill1_1Collider.SetActive(true);
                yield return new WaitForSeconds(0.2f);
                Skill1_1Collider.SetActive(false);
                Skill1_1.Stop();
            }

           
            yield return new WaitForSeconds(0.2f);

            // b ��ƼŬ ���
            Skill1_2.Play();
            Skill1_2Collider.SetActive(true);
            yield return new WaitForSeconds(Skill1_2.main.duration);
            Skill1_2Collider.SetActive(false);
            Skill1_2.Stop();
        }
    }
    public void Parry()
    {
        PlayerCollider.SetActive(false);
        ParryCollider.SetActive(true);
    }

    public void EndGuard()
    {
        ParryCollider.SetActive(false);
        PlayerCollider.SetActive(true);
    }


    // �ִϸ��̼� �Ϸ� �̺�Ʈ ó�� �޼���
    public void OnAnimationComplete(string animationName)
    {
        _curState.OnAnimationComplete(animationName);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Bite"))
        {
            Debug.Log("1");
            PlayerStatManager.instance.AtkPlayer(1.0f);
            ChangeState(new HitState(this));
        }

    }


    private IEnumerator ReturnEffectToPoolAfterDelay(GameObject effect, float delay)
    {
        yield return new WaitForSeconds(delay);

        // ��ƼŬ �ý��� ����
        ParticleSystem ps = effect.GetComponent<ParticleSystem>();
        if (ps != null)
        {
            ps.Stop();
        }

        EffectPullingManager.Instance.ReturnObject(effect);


    }
    private void UsingPotion()
    {
        PlayerStatManager.instance.UsePotion();
        PotionEffect.Play();
    }

    public void DeathDelay()
    {
        StartCoroutine(DelayTime(5f));
        ReStartScene();
    }

    private IEnumerator DelayTime(float time)
    {
        new WaitForSeconds(time);
        yield return null;

    }
    private void ReStartScene()
    {
        SceneManager.LoadScene("Main");
    }
}
