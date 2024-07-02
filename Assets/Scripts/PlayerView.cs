//using StarterAssets;
//using System;
//using UnityEngine.InputSystem;
//using UnityEngine;
//using UnityEngine.UI;

//public class PlayerView : MonoBehaviour
//{
//    public Animator Animator_Player;
//    private IState _curState;
//    private Action<InputAction.CallbackContext> _inputCallback;
//    private Vector2 _moveInput;

//    [SerializeField] Text Text_TemporalState;

//    private void Start()
//    {
//        ChangeState(new IdleState(this));
//    }

//    private void Update()
//    {
//        _curState?.ExecuteOnUpdate();
//    }

//    public void ChangeState(IState newState)
//    {
//        _curState?.ExitState();
//        _curState = newState;
//        Text_TemporalState.text = _curState.ToString();
//        _curState.EnterState();
//    }

//    public void BindInputCallback(bool isBind, Action<InputAction.CallbackContext> callback)
//    {
//        if (isBind)
//            _inputCallback += callback;
//        else
//            _inputCallback -= callback;
//    }

//    public void OnActionInput(InputAction.CallbackContext context)
//    {
//        if(context.started == false)
//            return;

//        Debug.Log("==========입력 발생함");

//        _inputCallback?.Invoke(context);
//    }

//    public Vector2 GetMoveInput()
//    {
//        return _moveInput;
//    }
//    public void OnAnimationEnd()
//    {
//        // 애니메이션 종료 시 Idle 상태로 전환
//        ChangeState(new IdleState(this));
//    }
//    public void OnMoveInput(InputAction.CallbackContext context)
//    {
//        _moveInput = context.ReadValue<Vector2>();
//        // Move 상태로 전환
//        if (_moveInput != Vector2.zero && !(_curState is MoveState))
//        {
//            ChangeState(new MoveState(this));
//        }
//    }

//    // 캐릭터 이동 로직
//    public void Move(Vector2 direction)
//    {
        
//    }
//}