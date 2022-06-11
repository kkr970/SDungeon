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

    void Awake()
    {
        playerNum = 2; // 1, 2선택 또는 2 고정
        enemyNum = 3;  // 플레이어 수가 1이면2, 2면3 또는 3 고정
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
        if(Input.GetKeyDown(KeyCode.Space))
        {
            mGetTurn();
        }
        if(Input.GetKeyDown(KeyCode.S))
        {

        }
    }


    //턴의 순서를 정함
    void mSetTurn()
    {
        Debug.Log("Input : A, SetTurn!");
        charactors = FindObjectsOfType<CharacterManager>();
        lcharactors = new List<CharacterManager>(charactors);
        for(int i = 0 ; i < lcharactors.Count ; i++)
        {
            lcharactors[i].setTurn();
            lcharactors[i].State = STATE.WAIT;
        }
        //큰 순서대로 정렬
        lcharactors.Sort((a, b) => a.getTurn().CompareTo(b.getTurn())*(-1) );
    }
    void mGetTurn()
    {
        Debug.Log("Input : space, GetTurn!");
        for(int i = 0 ; i < lcharactors.Count ; i++)
        {
            Debug.Log(lcharactors[i].getName() + " " + lcharactors[i].getTurn());
            Debug.Log(lcharactors[i].State);
        }
    }
    void mProgressTurn()
    {

    }
}
