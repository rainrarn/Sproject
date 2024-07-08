using System;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
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

    public GameObject CinemachineCameraTarget;
    public float TopClamp = 70.0f;
    public float BottomClamp = -30.0f;
    public float CameraAngleOverride = 0.0f;
    public bool LockCameraPosition = false;
    public float CameraRotationSpeed = 2.0f;

    // Cinemachine
    private float _cinemachineTargetYaw;
    private float _cinemachineTargetPitch;

    // �÷��̾�
    private float _speed;
    private float _targetRotation = 0.0f;
    private float _rotationVelocity;
    

    public float dodgeDistance = 20.0f; // ȸ�� �� �����ϴ� �Ÿ�
    public float dodgeDuration = 0.5f; // ȸ�� �ð�
    public float jumpPower = 2.0f; // ���� ����
    public float jumpDuration = 1.0f; // ���� �ð�
    private bool isDodging = false;
    private bool isJumping = false;
    private CharacterController _controller;

    public ParticleSystem Atk1;
    public ParticleSystem Atk2;
    public ParticleSystem Atk3;
    public ParticleSystem Atk4;

    public ParticleSystem GuardEffect;
    public GameObject GuardPose;

    public ParticleSystem Skill1_1;
    public ParticleSystem Skill1_2;

    public GameObject AtkCollider;

    public GameObject Skill1Collider1;
    public GameObject Skill1Collider2;

    public GameObject PlayerCollider;
    public GameObject ParryCollider;
    public GameObject GuardCollider;
    private void Start()
    {
        ChangeState(new IdleState(this));
        _controller = GetComponent<CharacterController>();
        _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;
    }

    private void Update()
    {
        _curState?.ExecuteOnUpdate();
        //Move();
        CameraRotation();
        if(Input.GetKeyDown(KeyCode.X)) 
        {
            Jump();
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
        Vector3 initialPosition = transform.position;
        Vector3 jumpTarget = initialPosition + Vector3.up * jumpPower;

        // DOTween�� ����Ͽ� ���� ����
        DOTween.Sequence()
            .Append(DOTween.To(() => 0f, x => _controller.Move(Vector3.up * (x - 0f)), jumpPower, jumpDuration / 2).SetEase(Ease.OutQuad))
            .Append(DOTween.To(() => jumpPower, x => _controller.Move(Vector3.up * (0f - x)), jumpPower, jumpDuration / 2).SetEase(Ease.InQuad))
            .OnComplete(() =>
            {
                isJumping = false;
            });
    }
    
    private IEnumerator Skill1Particle()
    {
        // a ��ƼŬ 0.3�� �������� 3�� ���
        for (int i = 0; i < 3; i++)
        {
            Skill1_1.Play();
            yield return new WaitForSeconds(0.3f);
        }

        // 1.2�� ���
        yield return new WaitForSeconds(1.2f);

        // b ��ƼŬ ���
        Skill1_2.Play();
    }
    public void Parry()
    {
        PlayerCollider.SetActive(false);
        ParryCollider.SetActive(true);
    }

    public void Guard()
    {
        ParryCollider.SetActive(false);
        GuardCollider.SetActive(true);
    }

    public void EndGuard()
    {
        GuardCollider.SetActive(false);
        PlayerCollider.SetActive(true);
    }

    // �ִϸ��̼� �Ϸ� �̺�Ʈ ó�� �޼���
    public void OnAnimationComplete(string animationName)
    {
        _curState.OnAnimationComplete(animationName);
    }
}
