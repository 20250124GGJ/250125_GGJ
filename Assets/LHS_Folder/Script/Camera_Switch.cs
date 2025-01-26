using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Switch : MonoBehaviour
{
    public GameObject MainCamera; // 메인 카메라
    public GameObject MiddleCamera; // 중간 카메라

    private Vector3 originalScale; // 원래 크기를 저장
    public float scaleFactor = 0.9f; // 클릭 시 줄어들 크기 비율
    public float animationSpeed = 0.1f; // 애니메이션 속도

    private void Start()
    {
        // 원래 크기 저장
        originalScale = transform.localScale;
    }

    public void OnClick()
    {
        // 클릭 애니메이션 효과 실행
        StopAllCoroutines();
        StartCoroutine(AnimateClick());

        // 카메라 전환
        if (MainCamera.activeSelf)
        {
            MainCamera.SetActive(false);
            MiddleCamera.SetActive(true);
        }
        else
        {
            MainCamera.SetActive(true);
            MiddleCamera.SetActive(false);
        }
    }

    private IEnumerator AnimateClick()
    {
        // 크기를 줄임
        transform.localScale = originalScale * scaleFactor;
        yield return new WaitForSeconds(animationSpeed);

        // 원래 크기로 복구
        transform.localScale = originalScale;
    }
}
