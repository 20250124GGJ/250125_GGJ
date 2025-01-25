using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    public Transform targetAreaCenter;  // 목표 영역의 중심
    public float targetAreaRadius = 8f;  // 목표 영역의 최대 반경 (8cm)

    public GameObject[] bottles;  // 병 배열

    private void Start()
    {
        
    }

    // 병들이 목표 영역에 있는지 확인하고 점수를 계산
    public void CalculateScores()
    {

        bottles = GameObject.FindGameObjectsWithTag("Bottle");  // Bottle 태그를 가진 모든 게임 오브젝트를 가져옵니다.
        Debug.Log(bottles.Length);
        foreach (GameObject bottle in bottles)
        {
            if (bottle)
            {
                // 병의 위치가 목표 영역 내에 있는지 확인
                float distance = Vector3.Distance(bottle.transform.position, targetAreaCenter.position);

                int scoreToAdd = 0;  // 추가될 점수

                // 범위에 따른 점수 부여
                if (distance <= 0.5f)  // 3cm 이내
                {
                    scoreToAdd = 20;  // 최고 점수
                }
                else if (distance <= 1f)  // 5cm 이내
                {
                    scoreToAdd = 15;  // 중간 점수
                }
                else if (distance <= 1.5f)  // 8cm 이내
                {
                    scoreToAdd = 10;  // 낮은 점수
                }

                // 점수 추가
                if (scoreToAdd > 0)
                {
                    if (bottle.GetComponent<Bottle>().team == 1)  // Red 팀
                    {
                        GameManager.Instance.AddScoreToRedPlayer(scoreToAdd);
                    }
                    else if (bottle.GetComponent<Bottle>().team == 2)  // Blue 팀
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
