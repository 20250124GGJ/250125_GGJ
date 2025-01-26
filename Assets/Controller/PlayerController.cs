using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private void Awake()
    {
        Instance = this;
    }

    public static PlayerController Instance { get; private set; }

    private GameObject Bottle; // ������ �� ������Ʈ
    private GameObject RotateObject; // ȸ����ų ������Ʈ

    private Vector2 lastTouchPosition; // ������ ��ġ/���콺 ��ġ
    private bool isDragging = false; // �巡�� ������ Ȯ��
    private float dragThreshold = 10f; // �巡�� �̵� �Ÿ� ����

    private Rect rotationArea; // ȸ�� ������ ������ �����ϴ� Rect

    public float Throw_Angle = 0;

    public void SetBottle(GameObject InBottle)
    {
        Bottle = InBottle;
    }

    public void SetRotateObject(GameObject InRotateObject)
    {
        RotateObject = InRotateObject;
    }

    private void Start()
    {
        GameManager.Instance.SetPlayerController(this);
        GameManager.Instance.NextTurn();

        // ȭ�� �Ʒ� 70%�� ȸ�� ���� �������� ����
        rotationArea = new Rect(0, 0, Screen.width, Screen.height * 0.7f);
    }

    public void Attack(float pulse)
    {
        if (Bottle)
        {
            Bottle.GetComponent<Bottle>().AttackBottle(pulse * 20.0f, Throw_Angle);
        }
    }

    private void Update()
    {
        HandleDragRotation();
    }

    private void HandleDragRotation()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsWithinRotationArea(Input.mousePosition))
            {
                lastTouchPosition = Input.mousePosition;
                isDragging = true;
            }
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition;

            if (delta.magnitude > dragThreshold)
            {
                RotateBasedOnDrag(delta.x);
                lastTouchPosition = Input.mousePosition;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            if (isDragging)
            {
                isDragging = false;
                Throw_Angle = RotateObject.transform.eulerAngles.y;
                GameManager.Instance.ShakeTime();
            }
        }

        // ��ġ �Է� ó�� (�����)
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (IsWithinRotationArea(touch.position))
                {
                    lastTouchPosition = touch.position;
                    isDragging = true;
                }
            }
            else if (touch.phase == TouchPhase.Moved && isDragging)
            {
                Vector2 delta = touch.position - lastTouchPosition;

                if (delta.magnitude > dragThreshold)
                {
                    RotateBasedOnDrag(delta.x);
                    lastTouchPosition = touch.position;
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                if (isDragging)
                {
                    isDragging = false;
                    Throw_Angle = RotateObject.transform.eulerAngles.y;
                    GameManager.Instance.ShakeTime();
                }
            }
        }
    }

    private void RotateBasedOnDrag(float deltaX)
    {
        if (RotateObject != null)
        {
            float rotationSpeed = 1.4f;
            float screenWidth = Screen.width;
            float normalizedDeltaX = deltaX / screenWidth;

            float currentYRotation = RotateObject.transform.localEulerAngles.y;

            if (currentYRotation > 180f)
            {
                currentYRotation -= 360f;
            }

            currentYRotation += normalizedDeltaX * rotationSpeed * 360f;

            currentYRotation = Mathf.Clamp(currentYRotation, -60f, 60f);

            if (currentYRotation < 0f)
            {
                currentYRotation += 360f;
            }

            RotateObject.transform.localEulerAngles = new Vector3(
                RotateObject.transform.localEulerAngles.x,
                currentYRotation,
                RotateObject.transform.localEulerAngles.z
            );

            if (Bottle != null)
            {
                Bottle.transform.localEulerAngles = new Vector3(
                    Bottle.transform.localEulerAngles.x,
                    currentYRotation + 270.0f,
                    Bottle.transform.localEulerAngles.z
                );
            }
        }
    }

    private bool IsWithinRotationArea(Vector2 position)
    {
        return rotationArea.Contains(position);
    }
}
