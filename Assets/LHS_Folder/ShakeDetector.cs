using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class ShakeDetector : MonoBehaviour
{
    // 감지 민감도와 쿨다운 시간
    public float shakeThreshold = 2.5f; // 흔들림 민감도
    public float shakeCooldown = 0.1f;  // 연속 감지 방지를 위한 쿨다운 타임

    public GameObject Energy;
    public GameObject Bottle_Shaking;

    public bool Shake = false;
    private float lastShakeTime;
    private float shakeTimer = 0f; // 사인파의 상태를 유지하기 위한 타이머

    private Vector3 originalRotation; // 오브젝트의 초기 회전값
    void Start()
    {
        originalRotation = Bottle_Shaking.transform.localEulerAngles;
        // 자이로스코프 활성화 (필요한 경우)
        if (SystemInfo.supportsGyroscope) // 장치가 자이로를 지원하는지 확인
        {
            Input.gyro.enabled = true;
            Debug.Log("Gyroscope enabled");
        }
        else
        {
            Debug.LogWarning("Gyroscope not supported on this device");
        }
    }

    void Update()
    {
        // 현재 가속도 값을 가져오기
        Vector3 acceleration = Input.acceleration; // 스마트폰의 가속도 데이터
        float accelerationMagnitude = acceleration.magnitude; // 가속도 크기 계산

        // 임계값을 초과하면 흔들림으로 간주
        if (accelerationMagnitude > shakeThreshold && Time.time > lastShakeTime + shakeCooldown)
        {
            Debug.Log(accelerationMagnitude);
            lastShakeTime = Time.time;



            // 흔들림 발생 시 추가 처리
            OnShakeDetected();
        }
    }

    void OnStartShaking()
    {
        Shake = true;
    }

    void OnShakeDetected()
    {
        // 흔들림 감지 시 실행되는 함수

        if(Shake == true)
        {
            Energy.GetComponent<Slider>().value = Energy.GetComponent<Slider>().value + 0.03f;
            StartCoroutine(ShakeBottleY());
        }
        
        

        // 여기에 흔들림으로 실행할 동작을 추가하세요
    }

    private System.Collections.IEnumerator ShakeBottleY()
    {
        float elapsed = 0f;

        while (elapsed < 0.3f) // 흔들림 지속 시간 (0.5초)
        {
            elapsed += Time.deltaTime;

            // 흔들림 타이머를 업데이트 (상태를 유지하기 위해 누적)
            shakeTimer += Time.deltaTime * 20f;

            // Y축 회전값 계산 (사인파에 누적된 타이머 사용)
            float yRotationOffset = Mathf.Sin(shakeTimer) * 20f;

            // 회전값 업데이트 (Y축만 변경)
            Bottle_Shaking.transform.localEulerAngles = originalRotation + new Vector3(0f, yRotationOffset, 0f);

            yield return null; // 다음 프레임까지 대기
        }

        // 흔들림 종료 후 원래 회전값으로 복구
        Bottle_Shaking.transform.localEulerAngles = originalRotation;
    }
}

