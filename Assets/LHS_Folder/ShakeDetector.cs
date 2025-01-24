using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class ShakeDetector : MonoBehaviour
{
    // ���� �ΰ����� ��ٿ� �ð�
    public float shakeThreshold = 2.5f; // ��鸲 �ΰ���
    public float shakeCooldown = 0.1f;  // ���� ���� ������ ���� ��ٿ� Ÿ��

    public GameObject Energy;
    public GameObject Remain_Time;

    public bool Shake = false;
    private float lastShakeTime;

    void Start()
    {
        // ���̷ν����� Ȱ��ȭ (�ʿ��� ���)
        if (SystemInfo.supportsGyroscope) // ��ġ�� ���̷θ� �����ϴ��� Ȯ��
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
        // ���� ���ӵ� ���� ��������
        Vector3 acceleration = Input.acceleration; // ����Ʈ���� ���ӵ� ������
        float accelerationMagnitude = acceleration.magnitude; // ���ӵ� ũ�� ���

        // �Ӱ谪�� �ʰ��ϸ� ��鸲���� ����
        if (accelerationMagnitude > shakeThreshold && Time.time > lastShakeTime + shakeCooldown)
        {
            Debug.Log(accelerationMagnitude);
            lastShakeTime = Time.time;



            // ��鸲 �߻� �� �߰� ó��
            OnShakeDetected();
        }
    }

    void OnStartShaking()
    {
        Shake = true;
    }

    void OnShakeDetected()
    {
        // ��鸲 ���� �� ����Ǵ� �Լ�

        if(Shake == true)
        {
            Energy.GetComponent<Slider>().value = Energy.GetComponent<Slider>().value + 0.03f;
        }
        
        

        // ���⿡ ��鸲���� ������ ������ �߰��ϼ���
    }
}

