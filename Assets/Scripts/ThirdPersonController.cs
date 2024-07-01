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
        [Header("�÷��̾�")]
        [Tooltip("ĳ������ �̵� �ӵ� (m/s)")]
        public float MoveSpeed = 2.0f;

        [Tooltip("ĳ���Ͱ� �̵� ������ ���� ȸ���ϴ� �ӵ�")]
        [Range(0.0f, 0.3f)]
        public float RotationSmoothTime = 0.12f;

        [Tooltip("���� �� ���� �ӵ�")]
        public float SpeedChangeRate = 10.0f;

        public AudioClip LandingAudioClip;
        public AudioClip[] FootstepAudioClips;
        [Range(0, 1)] public float FootstepAudioVolume = 0.5f;

        [Header("Cinemachine")]
        [Tooltip("Cinemachine Virtual Camera���� ī�޶� ���� Ÿ��")]
        public GameObject CinemachineCameraTarget;

        [Tooltip("ī�޶� ���� �󸶳� ������ �� �ִ��� (�� ����)")]
        public float TopClamp = 70.0f;

        [Tooltip("ī�޶� �Ʒ��� �󸶳� ������ �� �ִ��� (�� ����)")]
        public float BottomClamp = -30.0f;

        [Tooltip("ī�޶� �缳���� �߰� ����. ī�޶� ��ġ�� ������ �� ������")]
        public float CameraAngleOverride = 0.0f;

        [Tooltip("ī�޶� ��ġ�� ��� �࿡�� ����")]
        public bool LockCameraPosition = false;

        [Tooltip("ī�޶� ȸ�� �ӵ�")]
        public float CameraRotationSpeed = 2.0f;

        // Cinemachine
        private float _cinemachineTargetYaw;
        private float _cinemachineTargetPitch;

        // �÷��̾�
        private float _speed;
        private float _targetRotation = 0.0f;
        private float _rotationVelocity;

        // �ִϸ��̼� ID
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
            // ���� ī�޶� ���� ��������
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
            Debug.LogError("Starter Assets ��Ű���� ���Ӽ��� �����ϴ�. Tools/Starter Assets/Reinstall Dependencies�� ����Ͽ� ������ �ذ��Ͻʽÿ�.");
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
            // �Է��� �ְ� ī�޶� ��ġ�� �������� ���� ���
            if (_input.look.sqrMagnitude >= _threshold && !LockCameraPosition)
            {
                // ���콺 �Է��� Time.deltaTime�� ������ ����
                float deltaTimeMultiplier = IsCurrentDeviceMouse ? 1.0f : Time.deltaTime;

                _cinemachineTargetYaw += _input.look.x * deltaTimeMultiplier * CameraRotationSpeed;
                _cinemachineTargetPitch += _input.look.y * deltaTimeMultiplier * CameraRotationSpeed;
            }

            // ȸ�� ���� 360���� ����
            _cinemachineTargetYaw = ClampAngle(_cinemachineTargetYaw, float.MinValue, float.MaxValue);
            _cinemachineTargetPitch = ClampAngle(_cinemachineTargetPitch, BottomClamp, TopClamp);

            // Cinemachine�� �� Ÿ���� ����
            CinemachineCameraTarget.transform.rotation = Quaternion.Euler(_cinemachineTargetPitch + CameraAngleOverride,
                _cinemachineTargetYaw, 0.0f);
        }

        private void Move()
        {
            // �̵� �ӵ��� ������� ��ǥ �ӵ� ����
            float targetSpeed = MoveSpeed;

            // �Է��� ������ ��ǥ �ӵ��� 0���� ����
            if (_input.move == Vector2.zero) targetSpeed = 0.0f;

            // �÷��̾��� ���� ���� �ӵ� ����
            float currentHorizontalSpeed = new Vector3(_controller.velocity.x, 0.0f, _controller.velocity.z).magnitude;

            float speedOffset = 0.1f;
            float inputMagnitude = _input.analogMovement ? _input.move.magnitude : 1f;

            // ��ǥ �ӵ��� ���� �Ǵ� ����
            if (currentHorizontalSpeed < targetSpeed - speedOffset ||
                currentHorizontalSpeed > targetSpeed + speedOffset)
            {
                // ���� ������� �� �������� �ӵ� ��ȭ�� �ִ� ��� ����
                _speed = Mathf.Lerp(currentHorizontalSpeed, targetSpeed * inputMagnitude,
                    Time.deltaTime * SpeedChangeRate);

                // �ӵ��� �Ҽ��� ��° �ڸ��� �ݿø�
                _speed = Mathf.Round(_speed * 1000f) / 1000f;
            }
            else
            {
                _speed = targetSpeed;
            }

            // �Է� ������ ����ȭ
            Vector3 inputDirection = new Vector3(_input.move.x, 0.0f, _input.move.y).normalized;

            // �̵� �Է��� ������ �÷��̾ �̵��� �� ȸ��
            if (_input.move != Vector2.zero)
            {
                _targetRotation = Mathf.Atan2(inputDirection.x, inputDirection.z) * Mathf.Rad2Deg +
                                  _mainCamera.transform.eulerAngles.y;
                float rotation = Mathf.SmoothDampAngle(transform.eulerAngles.y, _targetRotation, ref _rotationVelocity,
                    RotationSmoothTime);

                // ī�޶� ��ġ�� �������� �Է� ������ ���� ȸ��
                transform.rotation = Quaternion.Euler(0.0f, rotation, 0.0f);
            }

            Vector3 targetDirection = Quaternion.Euler(0.0f, _targetRotation, 0.0f) * Vector3.forward;

            // �÷��̾� �̵�
            _controller.Move(targetDirection.normalized * (_speed * Time.deltaTime));

            // ĳ���Ͱ� �ִϸ����͸� ����ϴ� ��� ������Ʈ
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
