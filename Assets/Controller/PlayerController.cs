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
    private GameObject Bottle; // 공격할 병 오브젝트
    private GameObject RotateObject; // 회전시킬 오브젝트

    private Vector2 lastTouchPosition; // 마지막 터치/마우스 위치
    private bool isDragging = false; // 드래그 중인지 확인

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
        // 스페이스바로 병 공격
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (Bottle)
            {
                Bottle.GetComponent<Bottle>().AttackBottle(4.0f);
            }
        }

        // A키로 각도 계산 후 턴 종료
        if (Input.GetKeyDown(KeyCode.A))
        {
            GameManager.Instance.Angle_Search();
            GameManager.Instance.NextTurn();
        }

        // 드래그로 회전 조작 (마우스 또는 터치 입력)
        HandleDragRotation();
    }

    private void HandleDragRotation()
    {
        // 마우스 입력 처리 (에디터 및 PC 테스트용)
        if (Input.GetMouseButtonDown(0))
        {
            lastTouchPosition = Input.mousePosition;
            isDragging = true;
        }
        else if (Input.GetMouseButton(0) && isDragging)
        {
            Vector2 delta = (Vector2)Input.mousePosition - lastTouchPosition; // 이동 거리 계산
            RotateBasedOnDrag(delta.x); // 드래그 이동량에 따라 회전
            lastTouchPosition = Input.mousePosition; // 마지막 위치 업데이트
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;
            GameManager.Instance.ShakeTime();
        }

        // 터치 입력 처리 (모바일)
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
                Vector2 delta = touch.position - lastTouchPosition; // 이동 거리 계산
                RotateBasedOnDrag(delta.x); // 드래그 이동량에 따라 회전
                lastTouchPosition = touch.position; // 마지막 위치 업데이트
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
            // 회전 속도와 화면 너비로 정규화
            float rotationSpeed = 1.4f; // 회전 속도
            float screenWidth = Screen.width; // 화면의 너비
            float normalizedDeltaX = deltaX / screenWidth; // deltaX를 화면 너비로 나누어 정규화

            // 현재 Y축 회전값 가져오기
            float currentYRotation = RotateObject.transform.localEulerAngles.y;

            // 0~360도 범위를 -180~180도로 변환
            if (currentYRotation > 180f)
            {
                currentYRotation -= 360f;
            }

            // 드래그 이동량에 따라 회전값 계산
            currentYRotation += normalizedDeltaX * rotationSpeed * 360f;

            // Y축 회전 값 제한 (-60도 ~ 60도)
            currentYRotation = Mathf.Clamp(currentYRotation, -60f, 60f);

            // 변경된 회전값을 다시 0~360도 범위로 변환 후 적용
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
