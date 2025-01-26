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

    private GameObject Bottle; // 공격할 병 오브젝트
    private GameObject RotateObject; // 회전시킬 오브젝트

    private Vector2 lastTouchPosition; // 마지막 터치/마우스 위치
    private bool isDragging = false; // 드래그 중인지 확인
    private float dragThreshold = 10f; // 드래그 이동 거리 기준

    private Rect rotationArea; // 회전 가능한 영역을 정의하는 Rect

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

        // 화면 아래 70%를 회전 가능 영역으로 설정
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

        // 터치 입력 처리 (모바일)
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
