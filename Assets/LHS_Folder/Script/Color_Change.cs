using System.Collections;
using UnityEngine;

public class Color_Change : MonoBehaviour
{
    public GameObject Shake_Camera; // Shake_Camera ������Ʈ
    private Renderer objRenderer;  // ������Ʈ�� Renderer

    private bool isChangingColor = false; // ���� ���� ������ Ȯ��

    void Start()
    {
        // ������Ʈ�� Renderer ��������
        objRenderer = GetComponent<Renderer>();

        // �ʱ� ������ �ʷϻ����� ����
        if (objRenderer != null)
        {
            objRenderer.material.color = Color.green;
        }
    }

    void Update()
    {
        // Shake_Camera�� Ȱ��ȭ�Ǿ��� ���� ���� ���� �ƴ� ��
        if (Shake_Camera.activeSelf && !isChangingColor)
        {
            StartCoroutine(ChangeColorOverTime());
        }
    }

    private IEnumerator ChangeColorOverTime()
    {
        isChangingColor = true;

        // ���� ��ȯ �ܰ�
        float duration = 3.0f; // �� ���� �ð�
        float halfDuration = duration / 2f;

        // �ʷ� -> ���
        yield return StartCoroutine(LerpColor(Color.green, Color.yellow, halfDuration));

        // ��� -> ����
        yield return StartCoroutine(LerpColor(Color.yellow, Color.red, halfDuration));

        isChangingColor = false;
    }

    private IEnumerator LerpColor(Color startColor, Color endColor, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Lerp�� �̿��� ���� ����
            objRenderer.material.color = Color.Lerp(startColor, endColor, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null; // ���� �����ӱ��� ���
        }

        // ���� ������ ��Ȯ�� ����
        objRenderer.material.color = endColor;
    }
}
