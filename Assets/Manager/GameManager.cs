using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using static UnityEngine.EventSystems.EventTrigger;

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

    private List<GameObject> droppedFoods = new List<GameObject>();
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
    public GameObject[] foodPrefabs;

    public Score scoreCalculator;
    public GameObject Arrow;
    public GameObject leftWall;
    public GameObject rightWall;

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

    //이벤트 고지 패널
    public Image wallBreak;
    public Image iceBoard;
    public Image foodDrop;
    public Image sauceShower;
    public Image aimSmall;

    public bool ShakeTime_Check = false;

    public bool Angle = false;

    private bool    EventActive = false;
    private Event   TurnEvent;
    private bool    EventCall = false;

    public GameObject Shake_Detector;

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
            int randvalue = Random.Range(30, 48);

            Debug.Log(randvalue);

            if(randvalue > 84)
            {
                TurnEvent = new Event();
                TurnEvent.SetEvent(5);
                StartCoroutine(EventPanelShow());
                EventActive = true;
            }
            else if(randvalue > 64)
            {
                TurnEvent = new Event();
                TurnEvent.SetEvent(4);
                StartCoroutine(EventPanelShow());
                EventActive = true;
            }
            else if (randvalue > 49)
            {
                TurnEvent = new Event();
                TurnEvent.SetEvent(3);
                StartCoroutine(EventPanelShow());
                EventActive = true;
            }
            else if (randvalue > 29)
            {
                TurnEvent = new Event();
                TurnEvent.SetEvent(2);
                StartCoroutine(EventPanelShow());
                EventActive = true;
            }
            else if (randvalue > 9)
            {
                TurnEvent = new Event();
                TurnEvent.SetEvent(1);
                StartCoroutine(EventPanelShow());
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
                if(EventCall == false)
                {
                    EventCall = true;
                    //이벤트 최초 호출
                    switch(TurnEvent.id)
                    {
                        case 1:
                            RemoveWall();
                            //벽 제거(사이드 벽 비활성화)
                            break;
                        case 2:
                            Ice_Board();
                            //얼음바닥(힘1.5배)
                            break;
                        case 3:
                            FoodDropEvent();
                            //음식 폭포(범위나 대충 그냥 음식 오브젝트 설치) -> GameObject 배열로 가지고있다가 턴 지나면 foreach로 제거
                            break;
                        case 4:
                            //소스 범벅 -> UI 활성화
                            break;
                        case 5:
                            //접시 축소 -> 변수하나 만들어서 나중에 점수 구할때 30% 점수 까면됨 ㅇㅇ
                            break;
                    }
                }
               
                bool isOk = TurnEvent.DecreaseTurn();
                Debug.Log("이벤트 활성화 중" + TurnEvent.RemainTurn);
                if (isOk)
                {
                    Debug.Log("이벤트 종료");
                    switch (TurnEvent.id)
                    {
                        case 1:
                            //벽 활성화
                            CreateWall();
                            break;
                        case 2:
                            Ice_Board_Cancel();
                            //얼음바닥(힘1.5배) 변수(원래값 1 -> 1.5) 1.5->1
                            break;
                        case 3:
                            RemoveDroppedFoods();
                            //음식 폭포(범위나 대충 그냥 음식 오브젝트 설치) -> GameObject 배열로 가지고있다가 턴 지나면 foreach로 제거
                            break;
                        case 4:
                            //소스 범벅 -> UI 비활성화
                            break;
                    }
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
            if(EventCall)
            {
                yield return new WaitForSeconds(2f);
                redTeamTurnNotice.gameObject.SetActive(false);
                blueTeamTurnNotice.gameObject.SetActive(true);
            }
            else
            {
                redTeamTurnNotice.gameObject.SetActive(false);
                blueTeamTurnNotice.gameObject.SetActive(true);
            }
            
        }
        else
        {
            blueTeamTurnNotice.gameObject.SetActive(false);
            redTeamTurnNotice.gameObject.SetActive(true);
        }

        StartCoroutine(FadeOut());
  

        yield return null;
    }

    IEnumerator EventPanelShow()
    {
        if(TurnEvent.id==1)
        {
            wallBreak.gameObject.SetActive(true);
        }
        else if (TurnEvent.id == 2)
        {
            iceBoard.gameObject.SetActive(true);
        }
        else if (TurnEvent.id == 3)
        {
            foodDrop.gameObject.SetActive(true);
        }
        else if (TurnEvent.id == 4)
        {
            sauceShower.gameObject.SetActive(true);
        }
        else if (TurnEvent.id == 5)
        {
            aimSmall.gameObject.SetActive(true);
        }

        yield return new WaitForSeconds(2f);

        if (TurnEvent.id == 1)
        {
            wallBreak.gameObject.SetActive(false);
        }
        else if (TurnEvent.id == 2)
        {
            iceBoard.gameObject.SetActive(false);
        }
        else if (TurnEvent.id == 3)
        {
            foodDrop.gameObject.SetActive(false);
        }
        else if (TurnEvent.id == 4)
        {
            sauceShower.gameObject.SetActive(false);
        }
        else if (TurnEvent.id == 5)
        {
            aimSmall.gameObject.SetActive(false);
        }
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

    public void FoodDropEvent()
    {
        // 음식 4개를 생성
        for (int i = 0; i < 4; i++)
        {
            // 랜덤 위치 계산
            float randomX = Random.Range(-1f, 1f);  // x축 -1 ~ 1 범위
            float randomY = 3f;  // y축 고정값 3
            float randomZ = Random.Range(2f, 5f);  // z축 2 ~ 5 범위

            Vector3 spawnPosition = new Vector3(randomX, randomY, randomZ);

            // 4개 음식 프리팹 중에서 랜덤으로 선택
            int randomFoodIndex = Random.Range(0, foodPrefabs.Length);  // 0 ~ 3 인덱스 선택
            GameObject selectedFoodPrefab = foodPrefabs[randomFoodIndex];

            // 랜덤으로 선택된 음식 프리팹을 해당 위치에 생성
            GameObject foodInstance = Instantiate(selectedFoodPrefab, spawnPosition, Quaternion.identity);

            // 생성된 음식을 리스트에 추가
            droppedFoods.Add(foodInstance);
        }
    }

    public void Ice_Board()
    {
        Shake_Detector.GetComponent<ShakeDetector>().ICE = 1.5f;
    }

    public void Ice_Board_Cancel()
    {
        Shake_Detector.GetComponent<ShakeDetector>().ICE = 1.0f;
    }

    public void RemoveDroppedFoods()
    {
        // 리스트에 있는 음식들을 삭제
        foreach (GameObject food in droppedFoods)
        {
            Destroy(food);  // 음식 오브젝트 삭제
        }

        // 리스트 비우기
        droppedFoods.Clear();
    }

    public void RemoveWall()
    {
        leftWall.gameObject.SetActive(false);
        rightWall.gameObject.SetActive(false);
    }

    public void CreateWall()
    {
        leftWall.gameObject.SetActive(true);
        rightWall.gameObject.SetActive(true);
    }
}
