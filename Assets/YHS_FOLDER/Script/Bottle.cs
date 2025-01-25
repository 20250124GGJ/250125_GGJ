using System.Collections;
using UnityEngine;

public class Bottle : MonoBehaviour
{
    public int team;

    private Rigidbody rb;
    private Vector3 lastPosition; // 이전 위치 저장
    private float stationaryTime = 0f; // 병이 정지한 시간
    private bool isCheckingPosition = false; // 위치 확인 중인지 상태 체크

    public void AttackBottle(float InPower, float Angle)
    {
        // 오브젝트를 회전시킨다

        transform.rotation *= Quaternion.Euler(Angle, 0f, 0f);


        // 병의 회전된 방향에 맞는 힘을 적용
        Vector3 fwd = -transform.up;  // 회전된 후의 방향

        fwd.Normalize();
        Debug.Log(fwd);

        // 힘을 적용하여 병을 날리기
        rb.AddForce(fwd * InPower, ForceMode.Impulse);

        // 병의 이동 상태를 추적 시작
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
        isCheckingPosition = true; // 상태 체크 시작
        lastPosition = transform.position; // 초기 위치 저장

        while (true)
        {
            yield return new WaitForSeconds(0.1f); // 0.1초마다 확인

            // 병이 움직였는지 확인
            if (Vector3.Distance(lastPosition, transform.position) < 0.01f) // 움직임이 거의 없음
            {
                stationaryTime += 0.1f; // 정지 시간을 누적
                if (stationaryTime >= 3f) // 3초 동안 움직임이 없으면
                {
                    Debug.Log("Bottle is stable for 3 seconds. Moving to next turn.");
                    GameManager.Instance.NextTurn(); // NextTurn 호출
                    break;
                }
            }
            else
            {
                stationaryTime = 0f; // 움직임이 감지되면 정지 시간 초기화
            }

            lastPosition = transform.position; // 위치 업데이트
        }

        isCheckingPosition = false; // 상태 체크 종료
    }
}
