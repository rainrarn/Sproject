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
        [Tooltip("캐릭터의 이동 속도 (m/s)")]
        public float MoveSpeed = 2.0f;

        [Tooltip("캐릭터의 스프린트 속도 (m/s)")]
        public float SprintSpeed = 5.335f;

        [Tooltip("캐릭터가 이동 방향으로 회전하는 속도")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("가속도와 감속도")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Space(10)]
        [Tooltip("플레이어가 점프할 수 있는 높이")]
        public float JumpHeight = 1.2f;

        [Tooltip("캐릭터가 사용하는 중력 값. 기본 엔진 값은 -9.81f")]
        public float Gravity = -15.0f;

        [Space(10)]
        [Tooltip("다시 점프할 수 있기까지 필요한 시간. 0f로 설정하면 즉시 점프 가능")]
        public float JumpTimeout = 0.50f;

        [Tooltip("낙하 상태로 들어가기 전까지 필요한 시간. 계단을 내려가는 데 유용")]
        public float FallTimeout = 0.15f;

        [Header("Player Grounded")]
        [Tooltip("캐릭터가 지면에 있는지 여부. CharacterController의 기본 지면 체크에 포함되지 않음")]
        public bool Grounded = true;

        [Tooltip("거친 지면에 유용")]
        public float GroundedOffset = -0.14f;

        [Tooltip("지면 체크의 반경. CharacterController의 반경과 일치해야 함")]
        public float GroundedRadius = 0.28f;

        [Tooltip("캐릭터가 지면으로 사용하는 레이어")]
        public LayerMask GroundLayers;

        [Header("Cinemachine")]
        [Tooltip("카메라가 따라갈 Cinemachine 가상 카메라의 대상")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("카메라가 위로 이동할 수 있는 각도")]
        public float TopClamp = 70.0f;

        [Tooltip("카메라가 아래로 이동할 수 있는 각도")]
        public float BottomClamp = -30.0f;

        [Tooltip("카메라 위치를 미세 조정하는 데 유용한 추가 각도")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("모든 축에서 카메라 위치 잠금")]
        public bool LockCameraPosition = false;

        // cinemachine 변수들
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // 플레이어 변수들
        private float _speed;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;
        private float _verticalVelocity;
        private float _terminalVelocity = 53.0f;

        // 타임아웃 델타타임 변수들
        private float _jumpTimeoutDelta;
        private float _fallTimeoutDelta;

        // 애니메이션 ID 변수들
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

        // 현재 장치가 마우스인지 확인하는 프로퍼티
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

        // Awake 메서드는 오브젝트가 생성될 때 호출됩니다.
        private void Awake()
        {
            // 메인 카메라를 찾습니다.
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        // Start 메서드는 첫 프레임 업데이트 전에 호출됩니다.
        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
            Debug.LogError("Starter Assets 패키지에 종속성이 누락되었습니다. Tools/Starter Assets/Reinstall Dependencies를 사용하여 수정하세요.");
#endif

            AssignAnimationIDs();

            // 시작 시 타임아웃을 초기화합니다.
            _jumpTimeoutDelta = JumpTimeout;
            _fallTimeoutDelta = FallTimeout;
        }

        // Update 메서드는 매 프레임 호출됩니다.
        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);

            JumpAndGravity();
            GroundedCheck();
            Move();
        }

        // LateUpdate 메서드는 매 프레임의 마지막에 호출됩니다.
        private void LateUpdate()
        {
            CameraRotation();
        }

        // 애니메이션 ID를 할당합니다.
        private void AssignAnimationIDs()
        {
            _animIDMoving = Animator.StringToHash("Moving");
            _animIDGrounded = Animator.StringToHash("Grounded");
            _animIDJump = Animator.StringToHash("Jump");
            _animIDFreeFall = Animator.StringToHash("FreeFall");
        }

        // 캐릭터가 지면에 있는지 확인합니다.
        private void GroundedCheck()
        {
            // 오프셋과 함께 구체의 위치를 설정합니다.
            Vector3 spherePosition = new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z);
            Grounded = Physics.CheckSphere(spherePosition, GroundedRadius, GroundLayers, QueryTriggerInteraction.Ignore);

            // 캐릭터를 사용하는 경우 애니메이터 업데이트
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDGrounded, Grounded);
            }
        }

        // 카메라 회전을 처리합니다.
        private void CameraRotation()
        {
            // 입력이 있고 카메라 위치가 고정되지 않은 경우
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                // 마우스 입력은 Time.deltaTime을 곱하지 않습니다.
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier;
            }

            // 회전을 제한하여 값이 360도를 초과하지 않도록 합니다.
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine이 이 대상을 따라갑니다.
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride, _cinemachineTargetYaw, 0.0f);
        }

        // 플레이어 이동을 처리합니다.
        private void Move()
        {
            // 이동 속도, 스프린트 속도 및 스프린트 입력에 따라 목표 속도를 설정합니다.
            float targetSpeed = _input.sprint ? SprintSpeed : MoveSpeed;

            // 입력이 없으면 목표 속도를 0으로 설정합니다.
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // 현재 플레이어의 수평 속도를 참조합니다.
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // 플레이어가 움직이고 있는지 여부를 확인하고 애니메이터에 업데이트합니다.
            bool isMoving = currentHorizontalSpeed > 0.1f;
            if (_hasAnimator)
            {
                // 애니메이터에 'Moving' bool 값을 설정합니다.
                _animator.SetBool(_animIDMoving, isMoving);
            }

            // 입력 방향을 정규화합니다.
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // 이동 입력이 있는 경우 플레이어를 회전합니다.
            if (_input.move != Vector2.zero)
            {
                // 목표 회전 각도를 계산합니다.
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg + _mainCamera.transform.eulerAngles.y;
                // 현재 회전 각도를 부드럽게 변경합니다.
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity, RotationSmoothTime);

                // 입력 방향에 따라 플레이어를 회전시킵니다.
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            // 목표 방향을 계산합니다.
            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // 플레이어를 이동시킵니다.
            _controller.Move(targetDirection.normalized * (targetSpeed * Time.deltaTime) + new Vector3(0.0f, _verticalVelocity, 0.0f) * Time.deltaTime);
        }

        // 점프와 중력을 처리합니다.
        private void JumpAndGravity()
        {
            if (Grounded)
            {
                // 낙하 타임아웃 타이머를 재설정합니다.
                _fallTimeoutDelta = FallTimeout;

                // 캐릭터를 사용하는 경우 애니메이터 업데이트
                if (_hasAnimator)
                {
                    _animator.SetBool(_animIDJump, false);
                    _animator.SetBool(_animIDFreeFall, false);
                }

                // 지면에 있을 때 속도가 무한히 떨어지지 않도록 합니다.
                if (_verticalVelocity < 0.0f)
                {
                    _verticalVelocity = -2f;
                }

                // 점프
                if (_input.jump && _jumpTimeoutDelta <= 0.0f)
                {
                    // 원하는 높이에 도달하는 데 필요한 속도를 계산합니다.
                    _verticalVelocity = Mathf.Sqrt(JumpHeight * -2f * Gravity);

                    // 캐릭터를 사용하는 경우 애니메이터 업데이트
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDJump, true);
                    }
                }

                // 점프 타임아웃
                if (_jumpTimeoutDelta >= 0.0f)
                {
                    _jumpTimeoutDelta -= Time.deltaTime;
                }
            }
            else
            {
                // 점프 타임아웃 타이머를 재설정합니다.
                _jumpTimeoutDelta = JumpTimeout;

                // 낙하 타임아웃
                if (_fallTimeoutDelta >= 0.0f)
                {
                    _fallTimeoutDelta -= Time.deltaTime;
                }
                else
                {
                    // 캐릭터를 사용하는 경우 애니메이터 업데이트
                    if (_hasAnimator)
                    {
                        _animator.SetBool(_animIDFreeFall, true);
                    }
                }

                // 지면에 있지 않으면 점프하지 않습니다.
                _input.jump = false;
            }

            // 터미널 속도 아래로 시간이 지남에 따라 중력을 적용합니다.
            if (_verticalVelocity < _terminalVelocity)
            {
                _verticalVelocity += Gravity * Time.deltaTime;
            }
        }

        // 각도를 제한합니다.
        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        // 선택된 경우 Gizmos를 그립니다.
        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);

            if (Grounded) Gizmos.color = transparentGreen;
            else Gizmos.color = transparentRed;

            // 선택된 경우, 지면 콜라이더의 위치와 반경에 맞게 Gizmo를 그립니다.
            Gizmos.DrawSphere(new Vector3(transform.position.x, transform.position.y - GroundedOffset, transform.position.z), GroundedRadius);
        }

        // 발소리 애니메이션 이벤트 처리
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

        // 착지 애니메이션 이벤트 처리
        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}
