using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Event
{
    public int id { get; private set; }
    public int RemainTurn { get; private set; }

    public Event()
    {

    }
    public Event(int id, int remainTurn)
    {
        this.id = id;
        this.RemainTurn = remainTurn;
    }

    public void SetEvent(int id)
    {
        switch (id)
        {
            case 1:
                this.id = 1;
                this.RemainTurn = 3;
                break;
            case 2:
                this.id = 2;
                this.RemainTurn = 3;
                break;
            case 3:
                this.id = 3;
                this.RemainTurn = 3;
                break;
            case 4:
                this.id = 4;
                this.RemainTurn = 2;
                break;
            case 5:
                this.id = 5;
                this.RemainTurn = 4;
                break;
        }
    }

    //true -> 삭제필요
    public bool DecreaseTurn()
    {
        RemainTurn--;

        if (this.RemainTurn == 0) return true;
        return false;
    }
}

public class GameManager : MonoBehaviour
{
    public static GameManager Instance
    {
        get;
        private set;
    }

    private int TotalBottle = 0;
    //Player Controller
    private PlayerController playerController;
        
    //Turn
    // 1 - Red, 2 - Blue
    private int currentturn = 2;

    //플레이어의 점수 1 - Red, 2 - Blue
    private int redplayerscore  = 0;
    private int blueplayerscore = 0;

    public int GetRedPlayerScore    => redplayerscore;
    public int GetBluePlayerScore   => blueplayerscore;

    public GameObject[] BottlePrefabs;

    public Score scoreCalculator;
    public GameObject Arrow;

    //결과 화면 패널
    public TextMeshProUGUI redPlayerScoreText;  
    public TextMeshProUGUI bluePlayerScoreText; 
    public Image redTeamWinImage; 
    public Image blueTeamWinImage;
    public Image drawImage;
    public GameObject resultPanel;

    //턴 고지 패널
    public Image blueTeamTurnNotice;
    public Image redTeamTurnNotice;

    public bool ShakeTime_Check = false;

    public bool Angle = false;

    private bool    EventActive = false;
    private Event   TurnEvent;
    private int     TurenIndex = 0;

    void Start()
    {
        StartCoroutine(ShowTurnNotice());

        //병 생성
        //CreateBottle();
    }
    public void SetPlayerController(PlayerController InPlayerController)
    {
        playerController = InPlayerController;
    }

    // 1 플레이어 점수 증가 함수 -> 임의로 최대 점수 100으로 고정
    public void AddScoreToRedPlayer(int inscore)
    {
        int score = Mathf.Clamp(inscore, 0, 100);

        redplayerscore = Mathf.Clamp(redplayerscore + score, 0, 1000);
    }
    
    // 2 플레이어 점수 증가 함수
    public void AddScoreToBluePlayer(int inscore)
    {
        int score = Mathf.Clamp(inscore, 0, 100);

        blueplayerscore = Mathf.Clamp(blueplayerscore + score, 0, 1000);
    }

    // 턴 넘기기
    public void NextTurn()
    {
        //이벤트 여부 확인
        if(4==TotalBottle)
        {
            Debug.Log("Event 발생");
            int randvalue = Random.Range(0, 100);

            Debug.Log(randvalue);

            if(randvalue > 84)
            {
                TurnEvent = new Event();
                TurnEvent.SetEvent(5);
                EventActive = true;
            }
            else if(randvalue > 64)
            {
                TurnEvent = new Event();
                TurnEvent.SetEvent(4);
                EventActive = true;
            }
            else if (randvalue > 49)
            {
                TurnEvent = new Event();
                TurnEvent.SetEvent(3);
                EventActive = true;
            }
            else if (randvalue > 29)
            {
                TurnEvent = new Event();
                TurnEvent.SetEvent(2);
                EventActive = true;
            }
            else if (randvalue > 9)
            {
                TurnEvent = new Event();
                TurnEvent.SetEvent(1);
                EventActive = true;
            }
        }

        if (currentturn == 1)
        {
            currentturn = 2;
            Debug.Log("R");
        }
        else
        {
            currentturn = 1;
            Debug.Log("B");
            if (EventActive)
            {
                bool isOk = TurnEvent.DecreaseTurn();
                Debug.Log("이벤트 활성화 중" + TurnEvent.RemainTurn);
                if (isOk)
                {
                    Debug.Log("이벤트 종료");
                    EventActive = false;
                }
            }
        }

        //병 생성
        bool isCreate = CreateBottle();

        if (isCreate)
        {
            StartCoroutine(ShowTurnNotice());

            if (!Arrow.activeSelf) // Arrow가 비활성화 상태인지 확인
            {
                Arrow.SetActive(true); // 활성화
            }
        }
    }

    public void Choose_Bottle()
    {

    }

    public void Angle_Search()
    {
        
        Angle = true;
        
        //일련의 작업 수행
        //각도 계산하기 터치?
        
    }

    public void ShakeTime()
    {
        ShakeTime_Check = true;
        
    }

    public void returnTime()
    {
        
    }

    public void GameStart()
    {
        CreateBottle();
    }

    // 게임 초기화
    public void ResetGame()
    {
        //플레이어 스코어 초기화
        redplayerscore  = 0;
        blueplayerscore = 0;

        //턴 초기화
        currentturn = 1;

        TotalBottle = 0;   
    }

    // Init
    private void Awake()
    {
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

    private bool CreateBottle()
    {
        if (BottlePrefabs.Length == 0)
        {
            Debug.Log("Object Arr Length is 0");
            return false;
        }

        if (10 == TotalBottle)
        {
            EndGame();
            return false;
        }

        int PrefabIndex = Random.Range(0, BottlePrefabs.Length);
        GameObject RandomPrefab = BottlePrefabs[PrefabIndex];

        GameObject BottlePrefab = Instantiate(RandomPrefab, Vector3.zero, Quaternion.Euler(0f, -90f, 90f));


        /*
         * TEST
        Vector3 randomPosition = new Vector3
            (
            Random.Range(-5f, -3f),
            0f,
            Random.Range(-0.5f, 0.5f) // z 값
            );

        // 병을 생성
        GameObject BottlePrefab = Instantiate(RandomPrefab, randomPosition, Quaternion.identity);
        */

        Bottle bottleComponent = BottlePrefab.GetComponent<Bottle>();
        if (bottleComponent)
        {
            bottleComponent.team = currentturn;
            bottleComponent.AttachmentOutline();
        }  

        playerController.SetBottle(BottlePrefab);
        TotalBottle++;

        return true;
    }

    private void EndGame()
    {
        Debug.Log("게임 끝남");
              
        scoreCalculator.CalculateScores();
        // UI 표시
        Debug.Log("bluePlayerScore : "+ blueplayerscore);
        Debug.Log("redPlayerScore : "+ redplayerscore);

        StartCoroutine(ShowResult());
    }

    IEnumerator ShowResult()
    {
        yield return new WaitForSeconds(1f);

        resultPanel.SetActive(true);

        redPlayerScoreText.text = redplayerscore.ToString();
        bluePlayerScoreText.text = blueplayerscore.ToString();
        Debug.Log(redplayerscore.ToString());

        Debug.Log(blueplayerscore.ToString());

        if (redplayerscore > blueplayerscore)
        {
            redTeamWinImage.gameObject.SetActive(true);  
            blueTeamWinImage.gameObject.SetActive(false);
            drawImage.gameObject.SetActive(false);
        }
        else if (blueplayerscore > redplayerscore)
        {
            blueTeamWinImage.gameObject.SetActive(true);
            redTeamWinImage.gameObject.SetActive(false);
            drawImage.gameObject.SetActive(false);
        }
        else
        {
            redTeamWinImage.gameObject.SetActive(false);
            blueTeamWinImage.gameObject.SetActive(false);
            drawImage.gameObject.SetActive(true);
        }

        ResetGame();
    }

    IEnumerator ShowTurnNotice()
    {
        // 각 팀의 턴을 표시하는 이미지 활성화
        if (currentturn == 1)
        {
            redTeamTurnNotice.gameObject.SetActive(false);
            blueTeamTurnNotice.gameObject.SetActive(true);
        }
        else
        {
            blueTeamTurnNotice.gameObject.SetActive(false);
            redTeamTurnNotice.gameObject.SetActive(true);
        }

        StartCoroutine(FadeOut());
  

        yield return null;
    }

    IEnumerator FadeOut()
    {
        Image activeNotice = (currentturn == 1) ? blueTeamTurnNotice : redTeamTurnNotice;
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime;
            activeNotice.color = new Color(1f, 1f, 1f, 1f - time);  // 알파값을 점차 감소시킴

            yield return null;
        }

        activeNotice.gameObject.SetActive(false);  // FadeOut 후 이미지 비활성화
    }
}
