using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform monster; // 몬스터의 위치를 참조
    public Transform originalCameraPosition; // 카메라의 원래 위치를 참조
    public Canvas canvas; // UI 캔버스 참조
    public Image marker; // 몬스터를 표시할 마커 이미지 참조
    private bool isCameraFixed = false; // 카메라 고정 상태를 저장
    private Transform cameraTransform; // 카메라의 Transform

    void Start()
    {
        cameraTransform = Camera.main.transform; // 메인 카메라의 Transform 가져오기
        marker.enabled = false; // 시작할 때 마커를 비활성화
    }

    void Update()
    {
        // 특정 키 입력 감지 (예: C 키)
        if (Input.GetKeyDown(KeyCode.C))
        {
            // 카메라 고정 상태 전환
            isCameraFixed = !isCameraFixed;

            if (isCameraFixed)
            {
                // 몬스터 방향으로 카메라 고정
                cameraTransform.position = monster.position - (monster.forward * 10) + (Vector3.up * 5); // 몬스터 앞 10 단위 떨어진 위치, 위로 5 단위 올림
                cameraTransform.LookAt(monster); // 카메라를 몬스터로 향하게 함
                marker.enabled = true; // 마커를 활성화
            }
            else
            {
                // 원래 카메라 위치로 복귀
                cameraTransform.position = originalCameraPosition.position;
                cameraTransform.rotation = originalCameraPosition.rotation;
                marker.enabled = false; // 마커를 비활성화
            }
        }

        if (isCameraFixed)
        {
            // 마커를 몬스터 위치에 맞게 조정
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(monster.position);
            marker.transform.position = screenPosition;
        }
    }
}
