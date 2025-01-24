using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public int team;  // ���� �����ϴ� ���� (0: Red, 1: Blue)
    private Rigidbody rb;
    private Vector3 startPosition;  // ���� �ʱ� ��ġ
    private bool isClicked = false;  // ���� Ŭ���Ǿ����� ����
    public float force = 10f;  // ���� ���� ũ��
    private Vector3 direction;  // ������ ����

    public float slowDownFactor = 0.99f;  // �ӵ��� ���� ��� (�� �����Ӹ��� �ӵ��� �󸶳� ������ ����)
    public float minSpeed = 0.1f;  // �ּ� �ӵ� (�� �� ���Ϸ� �ӵ��� �پ��� ���߰� ��)

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody ������Ʈ ��������
        startPosition = transform.position;  // ���� �ʱ� ��ġ ����
    }

    void Update()
    {
        // ���콺 ��ư�� ������ ��
        if (Input.GetMouseButtonDown(0))  // ���콺 Ŭ�� �̺�Ʈ
        {
            isClicked = true;
        }

        // ���콺�� Ŭ���� ���¿��� ���� ���
        if (isClicked)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);  // ���콺 ��ġ (���� ��ǥ�� ��ȯ)
            direction = (mousePosition - (Vector2)transform.position).normalized;  // ������ ���콺 Ŭ�� ��ġ�� ���ϴ� ����
        }

        // ���콺�� ���� ���� ����
        if (Input.GetMouseButtonUp(0) && isClicked)
        {
            ThrowBottle(-direction, force);  // �ݴ� �������� ���� ����
            isClicked = false;  // Ŭ�� ���� ����
        }

        // �ð��� ������ ���� �ӵ� ����
        if (rb.velocity.magnitude > minSpeed)
        {
            rb.velocity *= slowDownFactor;  // �ӵ� ����
        }
        else
        {
            rb.velocity = Vector3.zero;  // �ּ� �ӵ��� �����ϸ� ���߱�
            EndTurn();  // �� ���� ó��
        }
    }

    // ���� ������ �޼��� (����� ���� �޾Ƽ� ���� ����)
    public void ThrowBottle(Vector2 direction, float force)
    {
        rb.velocity = Vector2.zero;  // ���� �ӵ� �ʱ�ȭ
        rb.AddForce(direction * force, ForceMode.Impulse);  // ���� ���� ���� ������
    }

    // �� ���� �� ���� ���� ���� ����
    private void EndTurn()
    {
        if (team == 0) // Red ���� ���� ������ Blue �� ���� ����
        {
            GameManager.Instance.SpawnBottleForTeam(1);
        }
        else if (team == 1) // Blue ���� ���� ������ Red �� ���� ����
        {
            GameManager.Instance.SpawnBottleForTeam(0);
        }
    }
}
