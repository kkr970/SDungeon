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

    public GameObject[] playerPrefabs;
    public GameObject[] enemyPrefabs;
    
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
            int num = Random.Range(0, playerPrefabs.Length);
            GameObject i0 = Instantiate(playerPrefabs[num], playerTransform[i]);
            i0.name = (i+1) + "." + i0.GetComponent<CharacterManager>().getName();
        }
        for(int i = 0 ; i < enemyNum ; i++)
        {
            int num = Random.Range(0, enemyPrefabs.Length);
            GameObject i0 = Instantiate(enemyPrefabs[num], enemyTransform[i]);
            i0.name = (i+1) + "." + i0.GetComponent<CharacterManager>().getName();
        }
        mSetTurn();
        findPlayerEnemy();
    }

    void Update()
    {
        if(gState == GAMESTATE.BATTLE)
        {
            // 임시 코드 확인용 버튼 A : 세팅 / D : 턴진행
            // 턴 정하기
            if(Input.GetKeyDown(KeyCode.A))
            {
                mSetTurn();
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
        for(int i = 0 ; i < lcharacters.Count ; i++)
        {
            lcharacters[i].setTurn();
            Color newColor = lcharacters[i].GetComponent<SpriteRenderer>().color;
            newColor.a = 1.0f;
            lcharacters[i].GetComponent<SpriteRenderer>().color = newColor;
            lcharacters[i].State = STATE.WAIT;
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

    // 턴 진행
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

                // 행동 종료 후처리
                // 캐릭터들이 살아있는지 확인하기 위한 작업
                findPlayerEnemy();

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
    //턴 진행 코루틴
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

    // 현재 캐릭터가 있는지 확인, 게임 승리, 게임 오버의 상태이전을 가지고있음
    private void findPlayerEnemy()
    {
        lenemyCharacters.Clear();
        lplayerCharacters.Clear();
        for(int i = 0 ; i < lcharacters.Count ; i++)
        {
            if(lcharacters[i].State == STATE.DEAD) continue;
            if(lcharacters[i].getTag() == "Enemy")
            {
                lenemyCharacters.Add(lcharacters[i]);
            }
            else if(lcharacters[i].getTag() == "Player")
            {
                lplayerCharacters.Add(lcharacters[i]);
            }
        }
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
    }
    
}
