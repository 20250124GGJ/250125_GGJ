using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Awake()
    {
        Instance = this;
    }
    public static PlayerController Instance
    {
        get;
        private set;
    }
    private GameObject Bottle; // ������ �� ������Ʈ
    private GameObject RotateObject; // ȸ����ų ������Ʈ

    private Vector2 lastTouchPosition; // ������ ��ġ/���콺 ��ġ
    private bool isDragging = false; // �巡�� ������ Ȯ��

    public void SetBottle(GameObject InBottle)
    {
        Bottle = InBottle;
    }

    public void SetRotateObject(GameObject InRotateObject)
    {
        RotateObject = InRotateObject;
    }

    // Update is called once per frame
    private void Start()
    {
        GameManager.Instance.SetPlayerController(this);
    }

    private void Update()
    {
        // �����̽��ٷ� �� ����
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Bottle)
            {
                Bottle.GetComponent<Bottle>().AttackBottle(4.0f);
            }
        }

        // AŰ�� ���� ��� �� �� ����
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.Angle_Search();
            GameManager.Instance.NextTurn();
        }

        // �巡�׷� ȸ�� ���� (���콺 �Ǵ� ��ġ �Է�)
        HandleDragRotation();
    }

    private void HandleDragRotation()
    {
        // ���콺 �Է� ó�� (������ �� PC �׽�Ʈ��)
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition; // �̵� �Ÿ� ���
            RotateBasedOnDrag(delta.x); // �巡�� �̵����� ���� ȸ��
            lastTouchPosition = Input.mousePosition; // ������ ��ġ ������Ʈ
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            GameManager.Instance.ShakeTime();
        }

        // ��ġ �Է� ó�� (�����)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                lastTouchPosition = touch.position;
                isDragging = true;
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.position - lastTouchPosition; // �̵� �Ÿ� ���
                RotateBasedOnDrag(delta.x); // �巡�� �̵����� ���� ȸ��
                lastTouchPosition = touch.position; // ������ ��ġ ������Ʈ
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isDragging = false;
                GameManager.Instance.ShakeTime();
            }
        }
    }

    private void RotateBasedOnDrag(float deltaX)
    {
        if (RotateObject != null)
        {
            // ȸ�� �ӵ��� ȭ�� �ʺ�� ����ȭ
            float rotationSpeed = 1.4f; // ȸ�� �ӵ�
            float screenWidth = Screen.width; // ȭ���� �ʺ�
            float normalizedDeltaX = deltaX / screenWidth; // deltaX�� ȭ�� �ʺ�� ������ ����ȭ

            // ���� Y�� ȸ���� ��������
            float currentYRotation = RotateObject.transform.localEulerAngles.y;

            // 0~360�� ������ -180~180���� ��ȯ
            if (currentYRotation > 180f)
            {
                currentYRotation -= 360f;
            }

            // �巡�� �̵����� ���� ȸ���� ���
            currentYRotation += normalizedDeltaX * rotationSpeed * 360f;

            // Y�� ȸ�� �� ���� (-60�� ~ 60��)
            currentYRotation = Mathf.Clamp(currentYRotation, -60f, 60f);

            // ����� ȸ������ �ٽ� 0~360�� ������ ��ȯ �� ����
            if (currentYRotation < 0f)
            {
                currentYRotation += 360f;
            }

            RotateObject.transform.localEulerAngles = new Vector3(
                RotateObject.transform.localEulerAngles.x,
                currentYRotation,
                RotateObject.transform.localEulerAngles.z
            );
        }
    }


}
