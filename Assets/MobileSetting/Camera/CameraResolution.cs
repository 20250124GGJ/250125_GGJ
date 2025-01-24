using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// �ػ� ���� ī�޶� ��ũ��Ʈ
public class CameraResolution : MonoBehaviour
{
    void Start()
    {
        Camera camera = GetComponent<Camera>();
        Rect rect = camera.rect;
        float scaleheight = ((float)Screen.width / Screen.height) / ((float)9 / 20);
        float scalewidth = 1f / scaleheight;

        // �� �Ʒ� ���� ���� (�޴����� ������ ���)
        if (scaleheight < 1)
        {
            rect.height = scaleheight;
            rect.y = (1f - scaleheight) / 2f;
        }
        // �� �� ���� ���� (�޴����� �׶��� ���)
        else
        {
            rect.width = scalewidth;
            rect.x = (1f - scalewidth) / 2f;
        }
        camera.rect = rect;
    }

    void OnPreCull() => GL.Clear(true, true, Color.black);
}
