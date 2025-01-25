using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    private int currentturn = 1;

    //�÷��̾��� ���� 1 - Red, 2 - Blue
    private int redplayerscore  = 0;
    private int blueplayerscore = 0;

    public int GetRedPlayerScore    => redplayerscore;
    public int GetBluePlayerScore   => blueplayerscore;

    public GameObject[] BottlePrefabs;

    public Score scoreCalculator;

    //��� ȭ�� �г�
    public TextMeshProUGUI redPlayerScoreText;  
    public TextMeshProUGUI bluePlayerScoreText; 
    public Image redTeamWinImage; 
    public Image blueTeamWinImage;
    public Image drawImage;
    public GameObject resultPanel;

    //�� ���� �г�
    public Image blueTeamTurnNotice;
    public Image redTeamTurnNotice;

    public bool ShakeTime_Check = false;

    public bool Angle = false;

    void Start()
    {
        StartCoroutine(ShowTurnNotice());

        //�� ����
        CreateBottle();
    }
    public void SetPlayerController(PlayerController InPlayerController)
    {
        playerController = InPlayerController;
    }

    // 1 �÷��̾� ���� ���� �Լ� -> ���Ƿ� �ִ� ���� 100���� ����
    public void AddScoreToRedPlayer(int inscore)
    {
        int score = Mathf.Clamp(inscore, 0, 100);

        redplayerscore = Mathf.Clamp(redplayerscore + score, 0, 1000);
    }
    
    // 2 �÷��̾� ���� ���� �Լ�
    public void AddScoreToBluePlayer(int inscore)
    {
        int score = Mathf.Clamp(inscore, 0, 100);

        blueplayerscore = Mathf.Clamp(blueplayerscore + score, 0, 1000);
    }

    // �� �ѱ��
    public void NextTurn()
    {
        if (currentturn == 1)
        {
            currentturn = 2;
        }
        else
        {
            currentturn = 1;
        }

        StartCoroutine(ShowTurnNotice());

        //�� ����
        CreateBottle();
        
    }

    public void Choose_Bottle()
    {

    }

    public void Angle_Search()
    {
        
        Angle = true;
        
        //�Ϸ��� �۾� ����
        //���� ����ϱ� ��ġ?
        
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

    // ���� �ʱ�ȭ
    public void ResetGame()
    {
        //�÷��̾� ���ھ� �ʱ�ȭ
        redplayerscore  = 0;
        blueplayerscore = 0;

        //�� �ʱ�ȭ
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

    private void CreateBottle()
    {
        if (BottlePrefabs.Length == 0)
        {
            Debug.Log("Object Arr Length is 0");
            return;
        }

        if (20 == TotalBottle)
        {
            EndGame();
            return;
        }

        int PrefabIndex = Random.Range(0, BottlePrefabs.Length);
        GameObject RandomPrefab = BottlePrefabs[PrefabIndex];

        GameObject BottlePrefab = Instantiate(RandomPrefab, Vector3.zero, Quaternion.identity);

        /*
         * TEST
        Vector3 randomPosition = new Vector3
            (
            Random.Range(-5f, -3f),
            0f,
            Random.Range(-0.5f, 0.5f) // z ��
            );

        // ���� ����
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
    }

    private void EndGame()
    {
        Debug.Log("���� ����");
              
        scoreCalculator.CalculateScores();
        // UI ǥ��
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

        if (redplayerscore > blueplayerscore)
        {
            redTeamWinImage.gameObject.SetActive(true);  
            blueTeamWinImage.gameObject.SetActive(false); 
        }
        else if (blueplayerscore > redplayerscore)
        {
            blueTeamWinImage.gameObject.SetActive(true);
            redTeamWinImage.gameObject.SetActive(false); 
        }
        else
        {
            redTeamWinImage.gameObject.SetActive(false);
            blueTeamWinImage.gameObject.SetActive(false);
            drawImage.gameObject.SetActive(true);
        }
    }

    IEnumerator ShowTurnNotice()
    {
        // �� ���� ���� ǥ���ϴ� �̹��� Ȱ��ȭ
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

        // ���� �ð��� ���� �� fadeOut ����
        yield return new WaitForSeconds(1f);  // �� ������ ��Ÿ�� �� ��� �ð�
        StartCoroutine(FadeOut());
    }

    IEnumerator FadeOut()
    {
        Image activeNotice = (currentturn == 1) ? blueTeamTurnNotice : redTeamTurnNotice;
        float time = 0f;

        while (time < 1f)
        {
            time += Time.deltaTime;
            activeNotice.color = new Color(1f, 1f, 1f, 1f - time);  // ���İ��� ���� ���ҽ�Ŵ

            yield return null;
        }

        activeNotice.gameObject.SetActive(false);  // FadeOut �� �̹��� ��Ȱ��ȭ
    }
}
