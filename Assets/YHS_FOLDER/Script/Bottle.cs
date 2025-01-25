using System.Collections;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public int team;

    private Rigidbody rb;
    private Vector3 lastPosition; // ���� ��ġ ����
    private float stationaryTime = 0f; // ���� ������ �ð�
    private bool isCheckingPosition = false; // ��ġ Ȯ�� ������ ���� üũ

    public void AttackBottle(float InPower, float Angle)
    {
        // ������Ʈ�� ȸ����Ų��

        transform.rotation *= Quaternion.Euler(Angle, 0f, 0f);


        // ���� ȸ���� ���⿡ �´� ���� ����
        Vector3 fwd = -transform.up;  // ȸ���� ���� ����

        fwd.Normalize();
        Debug.Log(fwd);

        // ���� �����Ͽ� ���� ������
        rb.AddForce(fwd * InPower, ForceMode.Impulse);

        // ���� �̵� ���¸� ���� ����
        if (!isCheckingPosition)
        {
            StartCoroutine(CheckPositionContinuously());
        }
    }


    public Vector3 GetForwardVector()
    {
        return transform.forward;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    private IEnumerator CheckPositionContinuously()
    {
        isCheckingPosition = true; // ���� üũ ����
        lastPosition = transform.position; // �ʱ� ��ġ ����

        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 0.1�ʸ��� Ȯ��

            // ���� ���������� Ȯ��
            if (Vector3.Distance(lastPosition, transform.position) < 0.01f) // �������� ���� ����
            {
                stationaryTime += 0.1f; // ���� �ð��� ����
                if (stationaryTime >= 3f) // 3�� ���� �������� ������
                {
                    Debug.Log("Bottle is stable for 3 seconds. Moving to next turn.");
                    GameManager.Instance.NextTurn(); // NextTurn ȣ��
                    break;
                }
            }
            else
            {
                stationaryTime = 0f; // �������� �����Ǹ� ���� �ð� �ʱ�ȭ
            }

            lastPosition = transform.position; // ��ġ ������Ʈ
        }

        isCheckingPosition = false; // ���� üũ ����
    }
}
