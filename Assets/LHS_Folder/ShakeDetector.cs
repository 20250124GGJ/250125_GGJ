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
    public GameObject Bottle_Shaking;

    public bool Shake = false;
    private float lastShakeTime;
    private float shakeTimer = 0f; // �������� ���¸� �����ϱ� ���� Ÿ�̸�

    private Vector3 originalRotation; // ������Ʈ�� �ʱ� ȸ����
    void Start()
    {
        originalRotation = Bottle_Shaking.transform.localEulerAngles;
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
            StartCoroutine(ShakeBottleY());
        }
        
        

        // ���⿡ ��鸲���� ������ ������ �߰��ϼ���
    }

    private System.Collections.IEnumerator ShakeBottleY()
    {
        float elapsed = 0f;

        while (elapsed < 0.3f) // ��鸲 ���� �ð� (0.5��)
        {
            elapsed += Time.deltaTime;

            // ��鸲 Ÿ�̸Ӹ� ������Ʈ (���¸� �����ϱ� ���� ����)
            shakeTimer += Time.deltaTime * 20f;

            // Y�� ȸ���� ��� (�����Ŀ� ������ Ÿ�̸� ���)
            float yRotationOffset = Mathf.Sin(shakeTimer) * 20f;

            // ȸ���� ������Ʈ (Y�ุ ����)
            Bottle_Shaking.transform.localEulerAngles = originalRotation + new Vector3(0f, yRotationOffset, 0f);

            yield return null; // ���� �����ӱ��� ���
        }

        // ��鸲 ���� �� ���� ȸ�������� ����
        Bottle_Shaking.transform.localEulerAngles = originalRotation;
    }
}

