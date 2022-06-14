using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

enum GAMESTATE
{
    BATTLE,
    WIN,
    LOSE
}

public class GameManager : MonoBehaviour
{   
    private static GameManager gm_instance;
    //외부 접근용 프로퍼티
    public static GameManager instance
    {
        get{
            if(gm_instance == null)
            {
                gm_instance = FindObjectOfType<GameManager>();
            }
            return gm_instance;
        }
    }
    
    private CharacterManager[] characters;
    public List<CharacterManager> lcharacters;
    public List<CharacterManager> lplayerCharacters;
    public List<CharacterManager> lenemyCharacters;

    public GameObject knightPrefab;
    public GameObject archmagePrefab;
    public GameObject zomburPrefab;
    public GameObject skeletopPrefab;
    
    public Transform[] playerTransform;
    public Transform[] enemyTransform;
    
    public int playerNum;
    public int enemyNum;
    public int roundNum;

    public bool isProcessing;
    private GAMESTATE gState;

    void Awake()
    {
        isProcessing = false;
        gState = GAMESTATE.BATTLE;
        playerNum = 2; // 1, 2선택 또는 2 고정
        enemyNum = 3;  // 플레이어:적 / 1:2 / 2:3 / x:3
        roundNum = 0;
    }

    void Start()
    {
        for(int i = 0 ; i < playerNum ; i++)
        {
            int num = Random.Range(0, 2);
            switch(num)
            {
                case 0:
                    Instantiate(knightPrefab, playerTransform[i]);
                    break;
                case 1:
                    Instantiate(archmagePrefab, playerTransform[i]);
                    break;
                default:
                    Debug.LogWarning("에러 발생 - 플레이어 생성");
                    break;
            }
        }
        for(int i = 0 ; i < enemyNum ; i++)
        {
            int num = Random.Range(0, 2);
            switch(num)
            {
                case 0:
                    Instantiate(zomburPrefab, enemyTransform[i]);
                    break;
                case 1:
                    Instantiate(skeletopPrefab, enemyTransform[i]);
                    break;
                default:
                    Debug.LogWarning("에러 발생 - 적 생성");
                    break;
            }
        }
        mSetTurn();
    }

    void Update()
    {
        if(gState == GAMESTATE.BATTLE)
        {
            //플레이어 전부 사망
            if(lplayerCharacters.Count <= 0)
            {
                gState = GAMESTATE.LOSE;
                UIManager.instance.gameOver();
            }
            //몹 전부 사망
            if(lenemyCharacters.Count <= 0)
            {
                gState = GAMESTATE.WIN;
                UIManager.instance.gameWin();
            }
            // 임시 코드 확인용 버튼
            // 턴 정하기
            if(Input.GetKeyDown(KeyCode.A))
            {
                mSetTurn();
            }
            // 상태보기
            if(Input.GetKeyDown(KeyCode.S))
            {
                mGetTurn();
            }
            // 턴 진행하기
            if(Input.GetKeyDown(KeyCode.D))
            {
                if(!isProcessing)
                {
                    isProcessing = true;
                    mProgressTurn();
                }
            }
        }
        //패배
        if(gState == GAMESTATE.LOSE)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        //승리
        if(gState == GAMESTATE.WIN)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
    }


    //턴의 순서를 정함
    void mSetTurn()
    {
        Debug.Log("Input : SetTurn!");
        roundNum += 1;
        UIManager.instance.updateTurnText(roundNum);
        
        characters = FindObjectsOfType<CharacterManager>();
        lcharacters = new List<CharacterManager>(characters);
        lenemyCharacters.Clear();
        lplayerCharacters.Clear();
        for(int i = 0 ; i < lcharacters.Count ; i++)
        {
            lcharacters[i].setTurn();
            Color newColor = lcharacters[i].GetComponent<SpriteRenderer>().color;
            newColor.a = 1.0f;
            lcharacters[i].GetComponent<SpriteRenderer>().color = newColor;
            lcharacters[i].State = STATE.WAIT;

            if(lcharacters[i].getTag() == "Enemy")
            {
                lenemyCharacters.Add(lcharacters[i]);
            }
            else if(lcharacters[i].getTag() == "Player")
            {
                lplayerCharacters.Add(lcharacters[i]);
            }
        }
        //큰 순서대로 정렬
        lcharacters.Sort((a, b) => a.getTurn().CompareTo(b.getTurn())*(-1) );
        //턴 알림
        if(lcharacters.Count >= 1)
        {
            lcharacters[0].onNowTurn();
        }
        if(lcharacters.Count >= 2)
        {
            lcharacters[1].onNextTurn();
        }
    }
    void mGetTurn()
    {
        //Debug.Log("Input : GetTurn!");
        for(int i = 0 ; i < lcharacters.Count ; i++)
        {
            Debug.Log(lcharacters[i].getName() + " " + lcharacters[i].getTurn());
            Debug.Log(lcharacters[i].State);
        }
    }
    void mProgressTurn()
    {
        //Debug.Log("Input : ProgressTurn!");
        for(int i = 0 ; i < lcharacters.Count ; i++)
        {
            if(lcharacters[i].State == STATE.WAIT)
            {
                Debug.Log("Progress Chara : " + lcharacters[i].getName());
                lcharacters[i].onNowTurn();
                // 캐릭터의 행동
                StartCoroutine(processTurn(lcharacters[i]));
                lcharacters[i].offNowTurn();

                //턴 알림
                if(i < lcharacters.Count - 1)
                {
                    lcharacters[i+1].onNowTurn();
                }
                if(i < lcharacters.Count - 2)
                {
                    lcharacters[i+2].onNextTurn();
                }
                break;
            }
        }
    }

    private IEnumerator processTurn(CharacterManager chara)
    {
        //Debug.Log(chara.getName() + " : Process turn!");
        
        // 몬스터일 경우
        if(chara.tag == "Enemy")
        {
            // 타겟을 선택
            int targetIndex = 0;
            float minHide = 100.0f;
            for(int i = 0 ; i < lplayerCharacters.Count ; i++)
            {
                if(lplayerCharacters[i].State == STATE.DEAD) continue;
                float rNum = Random.Range(0.0f, 2.0f);
                rNum += lplayerCharacters[i].hide;
                if(rNum < minHide)
                {
                    rNum = minHide;
                    targetIndex = i;
                }
            }
            chara.Attack(lplayerCharacters[targetIndex]);
        }
        // 플레이어일 경우
        else if(chara.tag == "Player")
        {
            // 몹중에서 선택 또는 UI에서 직접 선택하도록 제작
            // 일단 구현용 랜덤
            // 타겟을 선택
            int targetIndex = 0;
            float minHide = 100.0f;
            for(int i = 0 ; i < lenemyCharacters.Count ; i++)
            {
                if(lenemyCharacters[i].State == STATE.DEAD) continue;
                float rNum = Random.Range(0.0f, 2.0f);
                rNum += lenemyCharacters[i].hide;
                if(rNum < minHide)
                {
                    rNum = minHide;
                    targetIndex = i;
                }
            }
            //공격
            chara.Attack(lenemyCharacters[targetIndex]);
        }


        // 턴 마침
        chara.State = STATE.END;
        // 투명도를 조절
        Color newColor = chara.GetComponent<SpriteRenderer>().color;
        newColor.a = 0.3921f;
        chara.GetComponent<SpriteRenderer>().color = newColor;

        Debug.Log(chara.getName() + " : End turn!");
        yield return new WaitForSeconds(0.3f);

        // 모든 캐릭터가 행동을 종료시 자동 setTurn()
        int j = 0;
        for(; j < lcharacters.Count ; j++)
        {
            if(lcharacters[j].State == STATE.WAIT)
            {
                //Debug.Log("Wait 상태가 있습니다");
                break;
            }
        }
        if(j == lcharacters.Count)
        {
            mSetTurn();
        }
        isProcessing = false;
    }
}
