using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    private bool isDragging = false;     // ���콺�� �巡�� ������ ����
    private Rigidbody rb;                // ���� Rigidbody ������Ʈ

    public float forceMultiplier = 10f;  // ���� ���� (�ӵ� ����)
    public float randomForceRange = 5f;  // ������ ���� ����

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody ������Ʈ �ʱ�ȭ
    }

    void Update()
    {
        // ���콺 ��ư�� ������ �� (�� Ŭ�� ����)
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;  // �巡�� ����
        }

        // ���콺 ��ư�� �������� �� (���� ����)
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            // Ŭ���� ������ �� ������ �������� ���� ����
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f), // x�� ���� ����
                Random.Range(-1f, 1f), // y�� ���� ����
                Random.Range(-1f, 1f)  // z�� ���� ����
            );

            // ���� ����Ͽ� AddForce�� ���� (���� ��������)
            rb.AddForce(randomDirection.normalized * forceMultiplier, ForceMode.Impulse);  // ������ �������� ���� ��

            isDragging = false;  // �巡�� ����
        }
    }
}
