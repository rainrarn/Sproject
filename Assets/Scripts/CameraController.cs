using UnityEngine;
using UnityEngine.UI;

public class CameraController : MonoBehaviour
{
    public Transform monster; // ������ ��ġ�� ����
    public Transform originalCameraPosition; // ī�޶��� ���� ��ġ�� ����
    public Canvas canvas; // UI ĵ���� ����
    public Image marker; // ���͸� ǥ���� ��Ŀ �̹��� ����
    private bool isCameraFixed = false; // ī�޶� ���� ���¸� ����
    private Transform cameraTransform; // ī�޶��� Transform

    void Start()
    {
        cameraTransform = Camera.main.transform; // ���� ī�޶��� Transform ��������
        marker.enabled = false; // ������ �� ��Ŀ�� ��Ȱ��ȭ
    }

    void Update()
    {
        // Ư�� Ű �Է� ���� (��: C Ű)
        if (Input.GetKeyDown(KeyCode.C))
        {
            // ī�޶� ���� ���� ��ȯ
            isCameraFixed = !isCameraFixed;

            if (isCameraFixed)
            {
                // ���� �������� ī�޶� ����
                cameraTransform.position = monster.position - (monster.forward * 10) + (Vector3.up * 5); // ���� �� 10 ���� ������ ��ġ, ���� 5 ���� �ø�
                cameraTransform.LookAt(monster); // ī�޶� ���ͷ� ���ϰ� ��
                marker.enabled = true; // ��Ŀ�� Ȱ��ȭ
            }
            else
            {
                // ���� ī�޶� ��ġ�� ����
                cameraTransform.position = originalCameraPosition.position;
                cameraTransform.rotation = originalCameraPosition.rotation;
                marker.enabled = false; // ��Ŀ�� ��Ȱ��ȭ
            }
        }

        if (isCameraFixed)
        {
            // ��Ŀ�� ���� ��ġ�� �°� ����
            Vector3 screenPosition = Camera.main.WorldToScreenPoint(monster.position);
            marker.transform.position = screenPosition;
        }
    }
}
