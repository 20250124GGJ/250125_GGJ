using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    private bool isDragging = false;     // 마우스를 드래그 중인지 여부
    private Rigidbody rb;                // 병의 Rigidbody 컴포넌트

    public float forceMultiplier = 10f;  // 힘의 배율 (속도 조정)
    public float randomForceRange = 5f;  // 랜덤한 힘의 범위

    void Start()
    {
        rb = GetComponent<Rigidbody>();  // Rigidbody 컴포넌트 초기화
    }

    void Update()
    {
        // 마우스 버튼이 눌렸을 때 (병 클릭 시작)
        if (Input.GetMouseButtonDown(0))
        {
            isDragging = true;  // 드래그 시작
        }

        // 마우스 버튼이 떼어졌을 때 (병을 날림)
        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            // 클릭이 끝났을 때 랜덤한 방향으로 병을 날림
            Vector3 randomDirection = new Vector3(
                Random.Range(-1f, 1f), // x축 랜덤 방향
                Random.Range(-1f, 1f), // y축 랜덤 방향
                Random.Range(-1f, 1f)  // z축 랜덤 방향
            );

            // 힘을 계산하여 AddForce로 적용 (랜덤 방향으로)
            rb.AddForce(randomDirection.normalized * forceMultiplier, ForceMode.Impulse);  // 랜덤한 방향으로 힘을 줌

            isDragging = false;  // 드래그 종료
        }
    }
}
