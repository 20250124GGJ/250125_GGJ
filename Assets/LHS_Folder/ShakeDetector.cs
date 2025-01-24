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
    public GameObject Remain_Time;

    public bool Shake = false;
    private float lastShakeTime;

    void Start()
    {
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
        }
        
        

        // 여기에 흔들림으로 실행할 동작을 추가하세요
    }
}

