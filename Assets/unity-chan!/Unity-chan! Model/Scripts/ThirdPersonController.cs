using UnityEngine;
#if ENABLE_INPUT_SYSTEM 
using UnityEngine.InputSystem;
#endif

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("Player")]
        [Tooltip("ĳ������ �̵� �ӵ� (m/s)")]
        public float MoveSpeed = 2.0f;

        [Tooltip("ĳ������ ������Ʈ �ӵ� (m/s)")]
        public float SprintSpeed = 5.335f;

        [Tooltip("ĳ���Ͱ� �̵� �������� ȸ���ϴ� �ӵ�")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("���ӵ��� ���ӵ�")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("�÷��̾ ������ �� �ִ� ����")]
        public float JumpHeight = 1.2f;

        [Tooltip("ĳ���Ͱ� ����ϴ� �߷� ��. �⺻ ���� ���� -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("�ٽ� ������ �� �ֱ���� �ʿ��� �ð�. 0f�� �����ϸ� ��� ���� ����")]
        public float JumpTimeout = 0.50f;

        [Tooltip("���� ���·� ���� ������ �ʿ��� �ð�. ����� �������� �� ����")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("ĳ���Ͱ� ���鿡 �ִ��� ����. CharacterController�� �⺻ ���� üũ�� ���Ե��� ����")]
        public bool Grounded = true;

        [Tooltip("��ģ ���鿡 ����")]
        public float GroundedOffset = -0.14f;

        [Tooltip("���� üũ�� �ݰ�. CharacterController�� �ݰ�� ��ġ�ؾ� ��")]
        public float GroundedRadius = 0.28f;

        [Tooltip("ĳ���Ͱ� �������� ����ϴ� ���̾�")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("ī�޶� ���� Cinemachine ���� ī�޶��� ���")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("ī�޶� ���� �̵��� �� �ִ� ����")]
        public float TopClamp = 70.0f;

        [Tooltip("ī�޶� �Ʒ��� �̵��� �� �ִ� ����")]
        public float BottomClamp = -30.0f;

        [Tooltip("ī�޶� ��ġ�� �̼� �����ϴ� �� ������ �߰� ����")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("��� �࿡�� ī�޶� ��ġ ���")]
        public bool LockCameraPosition = false;

        // cinemachine ������
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // �÷��̾� ������
        private float _speed;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // Ÿ�Ӿƿ� ��ŸŸ�� ������
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // �ִϸ��̼� ID ������
        private int _animIDMoving;
        private int _animIDGrounded;
        private int _animIDJump;
        private int _animIDFreeFall;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

        // ���� ��ġ�� ���콺���� Ȯ���ϴ� ������Ƽ
        private bool IsCurrentDeviceMouse
        {
            get
            {
#if ENABLE_INPUT_SYSTEM
                return _playerInput.currentControlScheme == "KeyboardMouse";
#else
                return false;
#endif
            }
        }

        // Awake �޼���� ������Ʈ�� ������ �� ȣ��˴ϴ�.
        private void Awake()
        {
            // ���� ī�޶� ã���ϴ�.
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        // Start �޼���� ù ������ ������Ʈ ���� ȣ��˴ϴ�.
        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
            Debug.LogError("Starter Assets ��Ű���� ���Ӽ��� �����Ǿ����ϴ�. Tools/Starter Assets/Reinstall Dependencies�� ����Ͽ� �����ϼ���.");
#endif

            AssignAnimationIDs();

            // ���� �� Ÿ�Ӿƿ��� �ʱ�ȭ�մϴ�.
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        // Update �޼���� �� ������ ȣ��˴ϴ�.
        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        // LateUpdate �޼���� �� �������� �������� ȣ��˴ϴ�.
        private void LateUpdate()
        {
            CameraRotation();
        }

        // �ִϸ��̼� ID�� �Ҵ��մϴ�.
        private void AssignAnimationIDs()
        {
            _animIDMoving = Animator.StringToHash("Moving");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
        }

        // ĳ���Ͱ� ���鿡 �ִ��� Ȯ���մϴ�.
        private void GroundedCheck()
        {
            // �����°� �Բ� ��ü�� ��ġ�� �����մϴ�.
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            // ĳ���͸� ����ϴ� ��� �ִϸ����� ������Ʈ
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        // ī�޶� ȸ���� ó���մϴ�.
        private void CameraRotation()
        {
            // �Է��� �ְ� ī�޶� ��ġ�� �������� ���� ���
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                // ���콺 �Է��� Time.deltaTime�� ������ �ʽ��ϴ�.
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // ȸ���� �����Ͽ� ���� 360���� �ʰ����� �ʵ��� �մϴ�.
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine�� �� ����� ���󰩴ϴ�.
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }

        // �÷��̾� �̵��� ó���մϴ�.
        private void Move()
        {
            // �̵� �ӵ�, ������Ʈ �ӵ� �� ������Ʈ �Է¿� ���� ��ǥ �ӵ��� �����մϴ�.
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // �Է��� ������ ��ǥ �ӵ��� 0���� �����մϴ�.
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // ���� �÷��̾��� ���� �ӵ��� �����մϴ�.
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // �÷��̾ �����̰� �ִ��� ���θ� Ȯ���ϰ� �ִϸ����Ϳ� ������Ʈ�մϴ�.
            bool isMoving = currentHorizontalSpeed > 0.1f;
            if (_hasAnimator)
            {
                // �ִϸ����Ϳ� 'Moving' bool ���� �����մϴ�.
                _animator.SetBool(_animIDMoving, isMoving);
            }

            // �Է� ������ ����ȭ�մϴ�.
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // �̵� �Է��� �ִ� ��� �÷��̾ ȸ���մϴ�.
            if (_input.move != Vector2.zero)
            {
                // ��ǥ ȸ�� ������ ����մϴ�.
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                // ���� ȸ�� ������ �ε巴�� �����մϴ�.
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

                // �Է� ���⿡ ���� �÷��̾ ȸ����ŵ�ϴ�.
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            // ��ǥ ������ ����մϴ�.
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // �÷��̾ �̵���ŵ�ϴ�.
            _controller.Move(targetDirection.normalized * (targetSpeed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        // ������ �߷��� ó���մϴ�.
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // ���� Ÿ�Ӿƿ� Ÿ�̸Ӹ� �缳���մϴ�.
                _fallTimeoutDelta = FallTimeout;

                // ĳ���͸� ����ϴ� ��� �ִϸ����� ������Ʈ
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // ���鿡 ���� �� �ӵ��� ������ �������� �ʵ��� �մϴ�.
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // ����
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // ���ϴ� ���̿� �����ϴ� �� �ʿ��� �ӵ��� ����մϴ�.
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // ĳ���͸� ����ϴ� ��� �ִϸ����� ������Ʈ
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // ���� Ÿ�Ӿƿ�
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // ���� Ÿ�Ӿƿ� Ÿ�̸Ӹ� �缳���մϴ�.
                _jumpTimeoutDelta = JumpTimeout;

                // ���� Ÿ�Ӿƿ�
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // ĳ���͸� ����ϴ� ��� �ִϸ����� ������Ʈ
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // ���鿡 ���� ������ �������� �ʽ��ϴ�.
                _input.jump = false;
            }

            // �͹̳� �ӵ� �Ʒ��� �ð��� ������ ���� �߷��� �����մϴ�.
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        // ������ �����մϴ�.
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        // ���õ� ��� Gizmos�� �׸��ϴ�.
        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // ���õ� ���, ���� �ݶ��̴��� ��ġ�� �ݰ濡 �°� Gizmo�� �׸��ϴ�.
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }

        // �߼Ҹ� �ִϸ��̼� �̺�Ʈ ó��
        private void OnFootstep(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                if (FootstepAudioClips.Length > 0)
                {
                    var index = Random.Range(0, FootstepAudioClips.Length);
                    AudioSource.PlayClipAtPoint(FootstepAudioClips[index], transform.TransformPoint(_controller.center), FootstepAudioVolume);
                }
            }
        }

        // ���� �ִϸ��̼� �̺�Ʈ ó��
        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}
