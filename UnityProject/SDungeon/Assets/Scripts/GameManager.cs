using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine;

public enum GAMESTATE
{
    BATTLE,
    WIN,
    LOSE,
    PAUSE,
}
enum TRUNSTATE
{
    START,
    PROCESSING,
    END
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
    
    //메뉴, 일시정지
    public bool isPause;

    public CharacterManager[] characters;
    public List<CharacterManager> lcharacters;
    public List<CharacterManager> lplayerCharacters;
    public List<CharacterManager> lenemyCharacters;

    // 프리팹, 생성 위치
    public GameObject[] playerPrefabs;
    public GameObject[] enemyPrefabs;
    public Transform[] playerTransform;
    public Transform[] enemyTransform;
    
    //플레이어 수, 적 수, 라운드
    private int playerNum;
    private int enemyNum;
    private int roundNum;

    //각종 변수들
    public GAMESTATE gState;
    private TRUNSTATE tState;
    private bool isProcessing;

    private int targetIndex = 0;
    private CharacterManager targetObject;

    private int actionIndex = 0; // 0=공격 1=스킬 2=스킵
    private int skillIndex = 0;
    private bool resetAction = false;

    string enemyAction = "WAIT";


    void Awake()
    {
        isProcessing = false;
        isPause = false;
        gState = GAMESTATE.BATTLE;
        tState = TRUNSTATE.END;
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
        //전투
        if(gState == GAMESTATE.BATTLE)
        {
            //턴이 종료 -> 턴을 시작으로 변경
            if(tState == TRUNSTATE.END)
            {
                tState = TRUNSTATE.START;
            }
            //턴이 시작
            if(tState == TRUNSTATE.START)
            {
                tState = TRUNSTATE.PROCESSING;
            }
            //턴을 진행
            if(tState == TRUNSTATE.PROCESSING)
            {
                if(!isProcessing)
                {
                    isProcessing = true;
                    mProgressTurn();
                }
            }
        }
        //패배
        else if(gState == GAMESTATE.LOSE)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        //승리
        else if(gState == GAMESTATE.WIN)
        {
            if(Input.GetKeyDown(KeyCode.R))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().name);
            }
        }
        //일시정지
        else if(gState == GAMESTATE.PAUSE)
        {
            //UIManager에서 작동중
        }

    }


    //턴의 순서를 정함
    void mSetTurn()
    {
        //Debug.Log("Input : SetTurn!");
        roundNum += 1;
        UIManager.instance.updateTurnText(roundNum);
        
        characters = FindObjectsOfType<CharacterManager>();
        lcharacters = new List<CharacterManager>(characters);
        for(int i = 0 ; i < lcharacters.Count ; i++)
        {
            lcharacters[i].setTurn();
            lcharacters[i].GetComponent<SpriteRenderer>().color = new Vector4(1.0f, 1.0f, 1.0f, 1.0f);
            if(lcharacters[i].State == STATE.END)
            {
                lcharacters[i].State = STATE.WAIT;
            }
        }
        //큰 순서대로 정렬
        lcharacters.Sort((a, b) => a.getTurn().CompareTo(b.getTurn())*(-1) );
        //턴 알림
        nextTurn(0);
    }

    // 턴 진행
    void mProgressTurn()
    {
        //Debug.Log("Input : ProgressTurn!");
        for(int i = 0 ; i < lcharacters.Count ; i++)
        {
            if(lcharacters[i].State == STATE.WAIT)
            {
                //Debug.Log("Progress Chara : " + lcharacters[i].getName());
                lcharacters[i].onNowTurn();
                lcharacters[i].State = STATE.ACTION;
                // 캐릭터의 행동
                StartCoroutine(processTurn(lcharacters[i], i));
                //Debug.Log("코루틴 이후 함수 실행");

                break;
            }
        }
    }
    //턴 진행 코루틴
    IEnumerator processTurn(CharacterManager chara, int index)
    {
        resetAction = false;
        //Debug.Log(chara.getName() + " : Process turn!");
        UIManager.instance.processingChara = chara.gameObject;

        //상태이상 처리
        chara.checkEffect();
        if(chara.State == STATE.ACTION)
        {
            // 몬스터일 경우
            if(chara.tag == "Enemy")
            {
                // 몬스터의 행동 선택 AI
                yield return enemySelectAction(chara);

                // 기본 공격
                if(enemyAction == "Attack")
                {
                    // 타겟을 선택
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
                    yield return new WaitForSeconds(0.8f);
                    chara.attack(lplayerCharacters[targetIndex]);
                }
                // 스킬
                else
                {
                    skillIndex = enemyAction[5] - '0';
                    // 타겟을 선택
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
                    yield return new WaitForSeconds(0.8f);
                    chara.skill(lplayerCharacters[targetIndex], skillIndex);
                }
            }
            // 플레이어일 경우
            else if(chara.tag == "Player")
            {
                // 행동 선택
                UIManager.instance.actionSelectUI_ON();
                yield return selectAction();
                UIManager.instance.actionSelectUI_OFF();
                
                // 공격
                if(actionIndex == 0)
                {
                    UIManager.instance.actionTextUI_ON();
                    // 타겟을 선택
                    yield return StartCoroutine(selectTarget());
                    if(resetAction)
                    {
                        UIManager.instance.actionTextUI_OFF();
                        StartCoroutine(processTurn(chara, index));
                        yield break;
                    }
                    //공격
                    chara.attack(targetObject);
                    UIManager.instance.actionTextUI_OFF();
                }
                // 스킬
                else if(actionIndex == 1)
                {
                    bool flag = true;

                    // 스킬 선택
                    UIManager.instance.skillSelectUI_ON();
                    yield return StartCoroutine(selectSkill());
                    if(resetAction)
                    {
                        UIManager.instance.skillSelectUI_OFF();
                        StartCoroutine(processTurn(chara, index));
                        yield break;
                    }
                    UIManager.instance.skillSelectUI_OFF();

                    // 타겟을 선택
                    UIManager.instance.actionTextUI_ON();
                    yield return StartCoroutine(selectTarget());
                    if(resetAction)
                    {
                        UIManager.instance.actionTextUI_OFF();
                        StartCoroutine(processTurn(chara, index));
                        yield break;
                    }
                    UIManager.instance.actionTextUI_OFF();

                    // 스킬을 사용
                    flag = chara.skill(targetObject, skillIndex);
                    // mp 부족
                    if(!flag)
                    {
                        UIManager.instance.updateLogText("MP부족!");
                        StartCoroutine(processTurn(chara, index));
                        yield break;
                    }
                }
                // 스킵, 마나 회복
                else if(actionIndex == 2)
                {
                    chara.skip();
                }
            }
        }
        // 투명도를 조절
        chara.GetComponent<SpriteRenderer>().color = new Vector4(1.0f, 1.0f, 1.0f, 0.3921f);

        //Debug.Log(chara.getName() + " : End turn!");
        yield return new WaitForSeconds(0.3f);
        // 캐릭터들이 살아있는지 확인하기 위한 작업
        findPlayerEnemy();
        // 턴 마침
        chara.State = STATE.END;
        isProcessing = false;
        UIManager.instance.updateLogText("");
        // 턴 알림
        lcharacters[index].offNowTurn();
        nextTurn(index);

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
        
        //Debug.Log("코루틴 종료");
    }
    // 행동 선택 
    IEnumerator selectAction()
    {
        while(true)
        {
            if(!isPause && Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
                GameObject clickObject = null;
                if(hit.collider != null)
                {
                    clickObject = hit.transform.gameObject;
                    if(clickObject.name == "Attack Button")
                    {
                        actionIndex = 0;
                        break;
                    }
                    else if(clickObject.name == "Skill Button")
                    {
                        actionIndex = 1;
                        break;
                    }
                    else if(clickObject.name == "Skip Button")
                    {
                        actionIndex = 2;
                        break;
                    }
                }
            }
            yield return null;
        }
    }
    // AI 행동 선택
    IEnumerator enemySelectAction(CharacterManager enemy)
    {
        enemyAction = "WAIT";
        enemyAction = enemy.enemyActionAI();
        while(true)
        {
            //Debug.Log(enemy.getObjectName() + "가 선택한 행동 : " + enemyAction);
            if(enemyAction != "WAIT")
                break;
            yield return null;
        }
    }
    // 타겟 선택
    IEnumerator selectTarget()
    {
        while(true)
        {
            if(!isPause && Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
                GameObject clickObject = null;
                if(hit.collider != null)
                {
                    clickObject = hit.transform.gameObject;
                    if(clickObject.tag == "Enemy" || clickObject.tag == "Player")
                    {
                        targetObject = clickObject.GetComponent<CharacterManager>();
                        break;
                    }
                    else if(clickObject.name == "Back")
                    {
                        resetAction = true;
                        break;
                    }
                }
            }
            //테스트용 랜덤타겟
            /*
            if(Input.GetKeyDown(KeyCode.Space))
            {
                int targetIndex = 0;
                float minHide = 100.0f;
                for(int i = 0 ; i < lcharacters.Count ; i++)
                {
                    if(lcharacters[i].State == STATE.DEAD) continue;
                    float rNum = Random.Range(0.0f, 2.0f);
                    rNum += lcharacters[i].hide;
                    if(rNum < minHide)
                    {
                        rNum = minHide;
                        targetIndex = i;
                    }
                }
                targetObject = lcharacters[targetIndex];
                break;
            }
            */
            yield return null;
        }
    }
    // 스킬선택 UI에서 스킬을 선택
    IEnumerator selectSkill()
    {
        while(true)
        {
            if(!isPause && Input.GetMouseButtonDown(0))
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
                GameObject clickObject = null;
                if(hit.collider != null)
                {
                    clickObject = hit.transform.gameObject;
                    if(clickObject.name == "Skill1")
                    {
                        skillIndex = 1;
                        break;
                    }
                    else if(clickObject.name == "Back")
                    {
                        resetAction = true;
                        break;
                    }                    
                }
            }
            yield return null;
        }
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
    // 턴 알림
    private void nextTurn(int i)
    {        
        for(; i < lcharacters.Count ; i++)
        {
            if(lcharacters[i].State == STATE.WAIT)
            {
                lcharacters[i].onNowTurn();
                break;
            }
        }
        i++;
        for(; i < lcharacters.Count ; i++)
        {
            if(lcharacters[i].State == STATE.WAIT)
            {
                lcharacters[i].onNextTurn();
                break;
            }
        }
    }

}
