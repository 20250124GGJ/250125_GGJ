using System.Collections;
using UnityEngine;

public class Color_Change : MonoBehaviour
{
    public GameObject Shake_Camera; // Shake_Camera 오브젝트
    private Renderer objRenderer;  // 오브젝트의 Renderer

    private bool isChangingColor = false; // 색상 변경 중인지 확인

    void Start()
    {
        // 오브젝트의 Renderer 가져오기
        objRenderer = GetComponent<Renderer>();

        // 초기 색상을 초록색으로 설정
        if (objRenderer != null)
        {
            objRenderer.material.color = Color.green;
        }
    }

    void Update()
    {
        // Shake_Camera가 활성화되었고 색상 변경 중이 아닐 때
        if (Shake_Camera.activeSelf && !isChangingColor)
        {
            StartCoroutine(ChangeColorOverTime());
        }
    }

    private IEnumerator ChangeColorOverTime()
    {
        isChangingColor = true;

        // 색상 전환 단계
        float duration = 3.0f; // 총 지속 시간
        float halfDuration = duration / 2f;

        // 초록 -> 노랑
        yield return StartCoroutine(LerpColor(Color.green, Color.yellow, halfDuration));

        // 노랑 -> 빨강
        yield return StartCoroutine(LerpColor(Color.yellow, Color.red, halfDuration));

        isChangingColor = false;
    }

    private IEnumerator LerpColor(Color startColor, Color endColor, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Lerp를 이용해 색상 변경
            objRenderer.material.color = Color.Lerp(startColor, endColor, elapsedTime / duration);

            elapsedTime += Time.deltaTime;
            yield return null; // 다음 프레임까지 대기
        }

        // 최종 색상을 정확히 설정
        objRenderer.material.color = endColor;
    }
}
