using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Camera_Switch : MonoBehaviour
{
    public GameObject MainCamera; // ���� ī�޶�
    public GameObject MiddleCamera; // �߰� ī�޶�

    private Vector3 originalScale; // ���� ũ�⸦ ����
    public float scaleFactor = 0.9f; // Ŭ�� �� �پ�� ũ�� ����
    public float animationSpeed = 0.1f; // �ִϸ��̼� �ӵ�

    private void Start()
    {
        // ���� ũ�� ����
        originalScale = transform.localScale;
    }

    public void OnClick()
    {
        // Ŭ�� �ִϸ��̼� ȿ�� ����
        StopAllCoroutines();
        StartCoroutine(AnimateClick());

        // ī�޶� ��ȯ
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
        // ũ�⸦ ����
        transform.localScale = originalScale * scaleFactor;
        yield return new WaitForSeconds(animationSpeed);

        // ���� ũ��� ����
        transform.localScale = originalScale;
    }
}
