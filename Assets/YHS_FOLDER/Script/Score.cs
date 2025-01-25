using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public Transform targetAreaCenter;  // ��ǥ ������ �߽�
    public float targetAreaRadius = 8f;  // ��ǥ ������ �ִ� �ݰ� (8cm)

    public GameObject[] bottles;  // �� �迭

    private void Start()
    {
        
    }

    // ������ ��ǥ ������ �ִ��� Ȯ���ϰ� ������ ���
    public void CalculateScores()
    {

        bottles = GameObject.FindGameObjectsWithTag("Bottle");  // Bottle �±׸� ���� ��� ���� ������Ʈ�� �����ɴϴ�.
        Debug.Log(bottles.Length);
        foreach (GameObject bottle in bottles)
        {
            if (bottle)
            {
                // ���� ��ġ�� ��ǥ ���� ���� �ִ��� Ȯ��
                float distance = Vector3.Distance(bottle.transform.position, targetAreaCenter.position);

                int scoreToAdd = 0;  // �߰��� ����

                // ������ ���� ���� �ο�
                if (distance <= 0.5f)  // 3cm �̳�
                {
                    scoreToAdd = 20;  // �ְ� ����
                }
                else if (distance <= 1f)  // 5cm �̳�
                {
                    scoreToAdd = 15;  // �߰� ����
                }
                else if (distance <= 1.5f)  // 8cm �̳�
                {
                    scoreToAdd = 10;  // ���� ����
                }

                // ���� �߰�
                if (scoreToAdd > 0)
                {
                    if (bottle.GetComponent<Bottle>().team == 1)  // Red ��
                    {
                        GameManager.Instance.AddScoreToRedPlayer(scoreToAdd);
                    }
                    else if (bottle.GetComponent<Bottle>().team == 2)  // Blue ��
                    {
                        GameManager.Instance.AddScoreToBluePlayer(scoreToAdd);
                    }
                    
                }
            }
            bottle.SetActive(false);    
            //Destroy(bottle);
        }
        
    }
}
