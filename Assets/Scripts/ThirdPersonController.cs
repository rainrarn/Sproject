using UnityEngine.InputSystem;
using UnityEngine;

namespace StarterAssets
{
    [RequireComponent(typeof(CharacterController))]
#if ENABLE_INPUT_SYSTEM 
    [RequireComponent(typeof(PlayerInput))]
#endif
    public class ThirdPersonController : MonoBehaviour
    {
        [Header("플레이어")]
        [Tooltip("캐릭터의 이동 속도 (m/s)")]
        public float MoveSpeed = 2.0f;

        [Tooltip("캐릭터가 이동 방향을 향해 회전하는 속도")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("가속 및 감속 속도")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Header("Cinemachine")]
        [Tooltip("Cinemachine Virtual Camera에서 카메라가 따라갈 타겟")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("카메라를 위로 얼마나 움직일 수 있는지 (도 단위)")]
        public float TopClamp = 70.0f;

        [Tooltip("카메라를 아래로 얼마나 움직일 수 있는지 (도 단위)")]
        public float BottomClamp = -30.0f;

        [Tooltip("카메라를 재설정할 추가 각도. 카메라 위치를 고정할 때 유용함")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("카메라 위치를 모든 축에서 고정")]
        public bool LockCameraPosition = false;

        [Tooltip("카메라 회전 속도")]
        public float CameraRotationSpeed = 2.0f;

        // Cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // 플레이어
        private float _speed;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;

        // 애니메이션 ID
        private int _animIDSpeed;
        private int _animIDMoving;

#if ENABLE_INPUT_SYSTEM 
        private PlayerInput _playerInput;
#endif
        private Animator _animator;
        private CharacterController _controller;
        private StarterAssetsInputs _input;
        private GameObject _mainCamera;

        private const float _threshold = 0.01f;

        private bool _hasAnimator;

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

        private void Awake()
        {
            // 메인 카메라 참조 가져오기
            if (_mainCamera == null)
            {
                _mainCamera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }

        private void Start()
        {
            _cinemachineTargetYaw = CinemachineCameraTarget.transform.rotation.eulerAngles.y;

            _hasAnimator = TryGetComponent(out _animator);
            _controller = GetComponent<CharacterController>();
            _input = GetComponent<StarterAssetsInputs>();
#if ENABLE_INPUT_SYSTEM 
            _playerInput = GetComponent<PlayerInput>();
#else
            Debug.LogError("Starter Assets 패키지에 종속성이 없습니다. Tools/Starter Assets/Reinstall Dependencies를 사용하여 문제를 해결하십시오.");
#endif

            AssignAnimationIDs();
        }

        private void Update()
        {
            _hasAnimator = TryGetComponent(out _animator);
            Move();
        }

        private void LateUpdate()
        {
            CameraRotation();
        }

        private void AssignAnimationIDs()
        {
            _animIDSpeed = Animator.StringToHash("Speed");
            _animIDMoving = Animator.StringToHash("Moving");
        }

        private void CameraRotation()
        {
            // 입력이 있고 카메라 위치가 고정되지 않은 경우
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                // 마우스 입력은 Time.deltaTime을 곱하지 않음
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * CameraRotationSpeed;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * CameraRotationSpeed;
            }

            // 회전 값을 360도로 제한
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine이 이 타겟을 따라감
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // 이동 속도를 기반으로 목표 속도 설정
            float targetSpeed = MoveSpeed;

            // 입력이 없으면 목표 속도를 0으로 설정
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // 플레이어의 현재 수평 속도 참조
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // 목표 속도로 가속 또는 감속
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // 선형 결과보다 더 유기적인 속도 변화를 주는 곡선을 생성
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // 속도를 소수점 셋째 자리로 반올림
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            // 입력 방향을 정규화
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // 이동 입력이 있으면 플레이어가 이동할 때 회전
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // 카메라 위치를 기준으로 입력 방향을 향해 회전
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // 플레이어 이동
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime));

            // 캐릭터가 애니메이터를 사용하는 경우 업데이트
            if (_hasAnimator)
            {
                _animator.SetBool(_animIDMoving, targetSpeed > 0);
            }
        }

        private static float ClampAngle(float lfAngle, float lfMin, float lfMax)
        {
            if (lfAngle < -360f) lfAngle += 360f;
            if (lfAngle > 360f) lfAngle -= 360f;
            return Mathf.Clamp(lfAngle, lfMin, lfMax);
        }

        private void OnDrawGizmosSelected()
        {
            Color transparentGreen = new Color(0.0f, 1.0f, 0.0f, 0.35f);
            Color transparentRed = new Color(1.0f, 0.0f, 0.0f, 0.35f);
        }

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

        private void OnLand(AnimationEvent animationEvent)
        {
            if (animationEvent.animatorClipInfo.weight > 0.5f)
            {
                AudioSource.PlayClipAtPoint(LandingAudioClip, transform.TransformPoint(_controller.center), FootstepAudioVolume);
            }
        }
    }
}
