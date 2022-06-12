using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
    
    private CharacterManager[] charactors;
    public List<CharacterManager> lcharactors;
    public List<CharacterManager> lplayerCharactors;
    public List<CharacterManager> lenemyCharactors;

    public GameObject knightPrefab;
    public GameObject archmagePrefab;
    public GameObject zomburPrefab;
    public GameObject skeletopPrefab;
    
    public Transform[] playerTransform;
    public Transform[] enemyTransform;
    
    public int playerNum;
    public int enemyNum;
    public int turnNum;

    public bool isProcessing;

    void Awake()
    {
        isProcessing = false;
        playerNum = 2; // 1, 2선택 또는 2 고정
        enemyNum = 3;  // 플레이어 수가 1이면2, 2면3 또는 3 고정
        turnNum = 0;
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
        // 임시 코드 확인용 버튼
        if(Input.GetKeyDown(KeyCode.A))
        {
            mSetTurn();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {
            mGetTurn();
        }
        if(Input.GetKeyDown(KeyCode.D))
        {
            if(!isProcessing)
            {
                isProcessing = true;
                mProgressTurn();
            }
        }
    }


    //턴의 순서를 정함
    void mSetTurn()
    {
        Debug.Log("Input : SetTurn!");
        turnNum += 1;
        UIManager.instance.UpdateTurnText(turnNum);
        
        charactors = FindObjectsOfType<CharacterManager>();
        lcharactors = new List<CharacterManager>(charactors);
        lenemyCharactors.Clear();
        lplayerCharactors.Clear();
        for(int i = 0 ; i < lcharactors.Count ; i++)
        {
            lcharactors[i].setTurn();
            Color newColor = lcharactors[i].GetComponent<SpriteRenderer>().color;
            newColor.a = 1.0f;
            lcharactors[i].GetComponent<SpriteRenderer>().color = newColor;
            lcharactors[i].State = STATE.WAIT;

            if(lcharactors[i].getTag() == "Enemy")
            {
                lenemyCharactors.Add(lcharactors[i]);
            }
            else if(lcharactors[i].getTag() == "Player")
            {
                lplayerCharactors.Add(lcharactors[i]);
            }
        }
        //큰 순서대로 정렬
        lcharactors.Sort((a, b) => a.getTurn().CompareTo(b.getTurn())*(-1) );
    }
    void mGetTurn()
    {
        Debug.Log("Input : GetTurn!");
        for(int i = 0 ; i < lcharactors.Count ; i++)
        {
            Debug.Log(lcharactors[i].getName() + " " + lcharactors[i].getTurn());
            Debug.Log(lcharactors[i].State);
        }
    }
    void mProgressTurn()
    {
        Debug.Log("Input : ProgressTurn!");
        CharacterManager progressChara = null;

        for(int i = 0 ; i < lcharactors.Count ; i++)
        {
            if(lcharactors[i].State == STATE.WAIT)
            {
                progressChara = lcharactors[i];
                Debug.Log("Progress Chara : " + progressChara.getName());
                // 캐릭터의 행동
                StartCoroutine(processTurn(lcharactors[i]));
                break;
            }
        }
    }

    private IEnumerator processTurn(CharacterManager chara)
    {
        Debug.Log(chara.getName() + " : Process turn!");

        yield return new WaitForSeconds(0.2f);
        
        // 몬스터일 경우
        if(chara.tag == "Enemy")
        {
            //타겟을 선택
            int targetIndex = 0;
            float minHide = 100.0f;
            for(int i = 0 ; i < lplayerCharactors.Count ; i++)
            {
                float rNum = Random.Range(0.0f, 2.0f);
                rNum += lplayerCharactors[i].hide;
                if(rNum < minHide)
                {
                    rNum = minHide;
                    targetIndex = i;
                }
            }
            chara.Attack(lplayerCharactors[targetIndex]);
        }
        // 플레이어일 경우
        else if(chara.tag == "Player")
        {
            // 몹중에서 선택 또는 UI에서 직접 선택하도록 제작
            // 일단 구현용 랜덤
            //타겟을 선택
            int targetIndex = 0;
            float minHide = 100.0f;
            for(int i = 0 ; i < lenemyCharactors.Count ; i++)
            {
                float rNum = Random.Range(0.0f, 2.0f);
                rNum += lenemyCharactors[i].hide;
                if(rNum < minHide)
                {
                    rNum = minHide;
                    targetIndex = i;
                }
            }
            chara.Attack(lenemyCharactors[targetIndex]);
        }


        // 턴 마침
        chara.State = STATE.END;
        // 투명도를 조절
        Color newColor = chara.GetComponent<SpriteRenderer>().color;
        newColor.a = 0.3921f;
        chara.GetComponent<SpriteRenderer>().color = newColor;

        Debug.Log(chara.getName() + " : End turn!");
        isProcessing = false;

        // 모든 캐릭터가 행동을 종료시 자동 setTurn()
        int j = 0;
        for(; j < lcharactors.Count ; j++)
        {
            if(lcharactors[j].State == STATE.WAIT)
            {
                Debug.Log("Wait 상태가 있습니다");
                break;
            }
        }
        if(j == lcharactors.Count)
        {
            mSetTurn();
        }
    }
}
