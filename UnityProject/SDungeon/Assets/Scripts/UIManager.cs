using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager instance
    {
        get{
            if(uim_instance == null)
            {
                uim_instance = FindObjectOfType<UIManager>();
            }
            return uim_instance;
        }
    }
    private static UIManager uim_instance;

    //Battle UI
    public GameObject battleUI;
        // SelectUI
    public GameObject actionSelectUI;
    public GameObject actionTextUI;
    public GameObject skillSelectUI;
        // Battle Log UI
    public ScrollRect logScroll;
        // Turn UI
    public Text turnText;
        // Character info
    public GameObject infoPanel;
    public Image infoImage;
    public GameObject processingChara;

    //Pause UI
    public GameObject gamePauseUI;
        // Panel
    public GameObject pauseButtonPanel;
    public GameObject pauseSettingPanel;
        // boolean
    private bool isSetting = false;
        // sound slider
    public Slider bgmVolumeSlider;
    private float bgmVolume = 0.2f;

    //GameOver UI
    public GameObject gameOverUI;

    //GameWin UI
    public GameObject gameWinUI;

    void Update()
    {
        //게임 진행중
        if(GameManager.instance.gState == GAMESTATE.BATTLE && !Input.GetMouseButton(0))
        {
            //UI Info Text 업데이트
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if(!GameManager.instance.isPause && hit.collider != null)
            {
                GameObject clickObject = hit.transform.gameObject;
                if(clickObject.tag == "Enemy" || clickObject.tag == "Player")
                {
                    infoPanel.SetActive(true);
                    if(Input.GetMouseButton(1))
                        updateInfoText(clickObject.GetComponent<CharacterManager>().getStatInfo());
                    else
                        updateInfoText(clickObject.GetComponent<CharacterManager>().getInfo());
                    updateInfoImage(clickObject.GetComponent<SpriteRenderer>().sprite);
                }
                else if(clickObject.name == "Attack Button")
                {
                    infoPanel.SetActive(true);
                    updateInfoText("공격" + System.Environment.NewLine
                                + "힘 계수 데미지 D6( 12 / 3 / 456 )");
                    updateInfoImage(processingChara.GetComponent<SpriteRenderer>().sprite);
                }
                else if(clickObject.name == "Skill Button")
                {
                    infoPanel.SetActive(true);
                    updateInfoText("스킬 사용");
                    updateInfoImage(processingChara.GetComponent<SpriteRenderer>().sprite);
                }
                else if(clickObject.name == "Skip Button")
                {
                    infoPanel.SetActive(true);
                    updateInfoText("스킵 사용" + System.Environment.NewLine
                                + "마나 회복 + 1 + (최대마나/5)");
                    updateInfoImage(processingChara.GetComponent<SpriteRenderer>().sprite);
                }
                else if(clickObject.name == "Skill1")
                {
                    infoPanel.SetActive(true);
                    updateInfoText(processingChara.GetComponent<CharacterManager>().skill_1_Info());
                    updateInfoImage(processingChara.GetComponent<SpriteRenderer>().sprite);
                }
            }
            else
            {
                infoPanel.SetActive(false);
                updateInfoText("");
                updateInfoImage(null);
            }
        
            //일시정지
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                GameManager.instance.gState = GAMESTATE.PAUSE;
                GameManager.instance.isPause = true;
                gamePause();
                return;
            }

        }
        //일시정지 상태
        else if(GameManager.instance.gState == GAMESTATE.PAUSE)
        {
            //설정창
            if(isSetting)
            {
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
                GameObject clickObject = null;
                if(hit.collider != null)
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        clickObject = hit.transform.gameObject;
                        // 뒤로가기
                        if(clickObject.name == "Back")
                        {
                            isSetting = false;
                            buttonUI_ON();
                            settingUI_OFF();
                        }
                    }
                }

                // 뒤로가기
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    isSetting = false;
                    buttonUI_ON();
                    settingUI_OFF();
                }
            }
            //일반 화면
            else
            {
                //UI 버튼 사용
                Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
                GameObject clickObject = null;
                if(hit.collider != null)
                {
                    if(Input.GetMouseButtonDown(0))
                    {
                        clickObject = hit.transform.gameObject;
                        Debug.Log(clickObject.name);
                        //일시정지 해제
                        if(clickObject.name == "Resume Button")
                        {
                            GameManager.instance.isPause = false;
                            GameManager.instance.gState = GAMESTATE.BATTLE;
                            UIManager.instance.gameResume();
                        }
                        //설정
                        else if(clickObject.name == "Setting Button")
                        {
                            isSetting = true;
                            buttonUI_OFF();
                            settingUI_ON();
                        }
                        //종료
                        else if(clickObject.name == "Quit Button")
                        {
                            #if UNITY_EDITOR
                            UnityEditor.EditorApplication.isPlaying = false;
                            #else
                            Application.Quit();
                            #endif
                        }
                    }
                }          
                
                //일시정지 해제
                if(Input.GetKeyDown(KeyCode.Escape))
                {
                    GameManager.instance.isPause = false;
                    GameManager.instance.gState = GAMESTATE.BATTLE;
                    UIManager.instance.gameResume();
                }
            }
        }
    }
    
    //Battle UI 메서드
        //턴 업데이트
    public void updateTurnText(int newTurn)
    {
        turnText.text = "Turn : " + newTurn;
    }
        //행동 선택 UI on/off
    public void actionSelectUI_ON()
    {
        actionSelectUI.SetActive(true);
    }
    public void actionSelectUI_OFF()
    {
        actionSelectUI.SetActive(false);
    }
        //액션 text UI on/off
    public void actionTextUI_ON()
    {
        actionTextUI.SetActive(true);
    }
    public void actionTextUI_OFF()
    {
        actionTextUI.SetActive(false);
    }
        //스킬 선택 UI on/off
    public void skillSelectUI_ON()
    {
        skillSelectUI.SetActive(true);
    }
    public void skillSelectUI_OFF()
    {
        skillSelectUI.SetActive(false);
    }
        //로그 업데이트
    public void updateLogText(string log)
    {
        string text = logScroll.GetComponentInChildren<Text>().text;
        if(text != "")
        {
            logScroll.GetComponentInChildren<Text>().text = text + System.Environment.NewLine + "> " + log;
        }
        else
        {
            logScroll.GetComponentInChildren<Text>().text = "> " + log;
        }
        if(logScroll.GetComponentInChildren<Scrollbar>() != null)
            logScroll.GetComponentInChildren<Scrollbar>().value = 0.0f;
        
        
    }
        //정보 업데이트
    public void updateInfoText(string info)
    {
        infoPanel.GetComponentInChildren<Text>().text = info;
    }
    public void updateInfoImage(Sprite target)
    {
        if(target != null)
        {
            infoImage.sprite = target;        
        }
    }
    

    //GamePause UI 메서드
        // 일시정지
    public void gamePause()
    {
        battleUI.SetActive(false);
        gamePauseUI.SetActive(true);
        Time.timeScale = 0.0f;
    }
        // 일시정지 해제
    public void gameResume()
    {
        battleUI.SetActive(true);
        gamePauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }
        // 일시정지 초기 버튼 UI on/off
    public void buttonUI_ON()
    {
        pauseButtonPanel.SetActive(true);
    }
    public void buttonUI_OFF()
    {
        pauseButtonPanel.SetActive(false);
    }
        // 설정 UI on/off
    public void settingUI_ON()
    {
        pauseSettingPanel.SetActive(true);
    }
    public void settingUI_OFF()
    {
        pauseSettingPanel.SetActive(false);
    }


    //GameOver UI 메서드
    public void gameOver()
    {
        battleUI.SetActive(false);
        gameOverUI.SetActive(true);
    }


    //GameWin UI 메서드
    public void gameWin()
    {
        battleUI.SetActive(false);
        gameWinUI.SetActive(true);
    }


}
