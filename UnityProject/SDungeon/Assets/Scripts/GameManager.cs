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
    
    public CharacterManager[] charactors;
    public List<CharacterManager> lcharactors;

    public GameObject knightPrefab;
    public GameObject archmagePrefab;
    public GameObject zomburPrefab;
    public GameObject skeletopPrefab;
    
    public Transform[] playerTransform;
    public Transform[] enemyTransform;
    
    public int playerNum;
    public int enemyNum;
    public int turnNum;

    private bool isProcessing;

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
    }

    void Update()
    {
        //임시 코드 확인용 버튼
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
        for(int i = 0 ; i < lcharactors.Count ; i++)
        {
            lcharactors[i].setTurn();
            Color newColor = lcharactors[i].GetComponent<SpriteRenderer>().color;
            newColor.a = 1.0f;
            lcharactors[i].GetComponent<SpriteRenderer>().color = newColor;
            lcharactors[i].State = STATE.WAIT;
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
        int i = 0;
        for( ; i < lcharactors.Count ; i++)
        {
            if(lcharactors[i].State == STATE.WAIT)
            {
                progressChara = lcharactors[i];
                Debug.Log("Progress Chara : " + progressChara.getName());
                // 캐릭터의 행동
                // 투명도를 조절
                Color newColor = lcharactors[i].GetComponent<SpriteRenderer>().color;
                newColor.a = 0.3921f;
                lcharactors[i].GetComponent<SpriteRenderer>().color = newColor;
                progressChara.State = STATE.END;
                break;
            }
        }
        if(i >= lcharactors.Count - 1)
        {
            mSetTurn();
            return;
        }
    }
}
