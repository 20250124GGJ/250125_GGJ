using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class ShakeDetector : MonoBehaviour
{
    SoundManager soundManager;

    // ���� �ΰ����� ��ٿ� �ð�
    public float shakeThreshold = 2.5f; // ��鸲 �ΰ���
    public float shakeCooldown = 0.1f;  // ���� ���� ������ ���� ��ٿ� Ÿ��

    public GameObject Energy;
    public GameObject Bottle_Shaking;

    public GameObject MainCamera;
    public GameObject ShakeCamera;

    public GameObject Arrow;

    public bool Shake = false;

    private bool isShaking = false;
    private float lastShakeTime;
    private float shakeTimer = 0f; // �������� ���¸� �����ϱ� ���� Ÿ�̸�

    private Vector3 originalRotation; // ������Ʈ�� �ʱ� ȸ����

    private void Awake()
    {
        soundManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<SoundManager>();
    }

    void Start()
    {
        PlayerController.Instance.SetRotateObject(Arrow);

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
        if(GameManager.Instance.ShakeTime_Check == true)
        {
            Shake = GameManager.Instance.ShakeTime_Check;
            if (Shake == true )
            {
                Arrow.SetActive(false);
                MainCamera.SetActive(false);
                ShakeCamera.SetActive(true);

                StartCoroutine(ResetShakeCheckAfterDelay(3f));
                if (accelerationMagnitude > shakeThreshold && Time.time > lastShakeTime + shakeCooldown)
                {
                    Debug.Log(accelerationMagnitude);
                    lastShakeTime = Time.time;



                    // ��鸲 �߻� �� �߰� ó��
                    OnShakeDetected();
                }
            }
            
        }
        // �Ӱ谪�� �ʰ��ϸ� ��鸲���� ����

        
        
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
            soundManager.PlaySFX(soundManager.shakeSound);
            Energy.GetComponent<Slider>().value = Energy.GetComponent<Slider>().value + 0.03f;
            StartCoroutine(ShakeBottleY());
        }
        
        

        // ���⿡ ��鸲���� ������ ������ �߰��ϼ���
    }

    private System.Collections.IEnumerator ResetShakeCheckAfterDelay(float delay)
    {
        // ù ��° ����
        yield return new WaitForSeconds(delay);

        // shake_Check ���� false�� ����
        Shake = false;
        MainCamera.SetActive(true);
        ShakeCamera.SetActive(false);

        GameManager.Instance.ShakeTime_Check = false;
        PlayerController.Instance.Attack(Energy.GetComponent<Slider>().value);
        Energy.GetComponent<Slider>().value = 0;

        // �߰� 3�� ����
        
        // ��: �߰� �ൿ�� ȣ���ϰų� ���¸� ������ �� ����
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

