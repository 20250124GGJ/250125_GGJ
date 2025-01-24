using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public int team;  // 팀을 구분하는 변수 (0: Red, 1: Blue)
    private Rigidbody rb;
    private Vector3 startPosition;  // 병의 초기 위치
    private bool isClicked = false;  // 병이 클릭되었는지 여부
    public float force = 10f;  // 던질 힘의 크기
    private Vector3 direction;  // 던져질 방향

    public float slowDownFactor = 0.99f;  // 속도를 줄일 계수 (매 프레임마다 속도를 얼마나 줄일지 설정)
    public float minSpeed = 0.1f;  // 최소 속도 (이 값 이하로 속도가 줄어들면 멈추게 함)

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody 컴포넌트 가져오기
        startPosition = transform.position;  // 병의 초기 위치 저장
    }

    void Update()
    {
        // 마우스 버튼을 눌렀을 때
        if (Input.GetMouseButtonDown(0))  // 마우스 클릭 이벤트
        {
            isClicked = true;
        }

        // 마우스를 클릭한 상태에서 방향 계산
        if (isClicked)
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);  // 마우스 위치 (월드 좌표로 변환)
            direction = (mousePosition - (Vector2)transform.position).normalized;  // 병에서 마우스 클릭 위치로 향하는 방향
        }

        // 마우스를 떼면 병을 던짐
        if (Input.GetMouseButtonUp(0) && isClicked)
        {
            ThrowBottle(-direction, force);  // 반대 방향으로 병을 던짐
            isClicked = false;  // 클릭 상태 종료
        }

        // 시간이 지남에 따라 속도 감소
        if (rb.velocity.magnitude > minSpeed)
        {
            rb.velocity *= slowDownFactor;  // 속도 감소
        }
        else
        {
            rb.velocity = Vector3.zero;  // 최소 속도에 도달하면 멈추기
            EndTurn();  // 턴 종료 처리
        }
    }

    // 병을 던지는 메서드 (방향과 힘을 받아서 병에 적용)
    public void ThrowBottle(Vector2 direction, float force)
    {
        rb.velocity = Vector2.zero;  // 이전 속도 초기화
        rb.AddForce(direction * force, ForceMode.Impulse);  // 병에 힘을 가해 던지기
    }

    // 턴 종료 후 다음 팀의 병을 생성
    private void EndTurn()
    {
        if (team == 0) // Red 팀의 턴이 끝나면 Blue 팀 병을 생성
        {
            GameManager.Instance.SpawnBottleForTeam(1);
        }
        else if (team == 1) // Blue 팀의 턴이 끝나면 Red 팀 병을 생성
        {
            GameManager.Instance.SpawnBottleForTeam(0);
        }
    }
}
