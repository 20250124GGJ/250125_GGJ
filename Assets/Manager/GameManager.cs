using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get;
        private set;
    }

    //플레이어의 점수 1 - Red, 2 - Blue
    //참조값으로 사용 예정
    private int redplayerscore  = 0;
    private int blueplayerscore = 0;

    public int GetRedPlayerScore    => redplayerscore;
    public int GetBluePlayerScore   => blueplayerscore;

    // 1 플레이어 점수 증가 함수 -> 임의로 최대 점수 100으로 고정
    public void AddScoreToRedPlayer(int inscore)
    {
        int score = Mathf.Clamp(inscore, 0, 100);

        redplayerscore = Mathf.Clamp(redplayerscore + score, 0, 1000);

        Debug.Log("Red Add");
    }
    
    // 2 플레이어 점수 증가 함수
    public void AddScoreToBluePlayer(int inscore)
    {
        int score = Mathf.Clamp(inscore, 0, 100);

        blueplayerscore = Mathf.Clamp(blueplayerscore + score, 0, 1000);

        Debug.Log("Blue Add");
    }

    // 매니저 초기화
    public void ResetGame()
    {
        redplayerscore  = 0;
        blueplayerscore = 0;

        Debug.Log("Reset");
    }

    // Init
    private void Awake()
    {
        Debug.Log("Awake");

        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}
