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

    public GameObject actionSelectUI;
    public GameObject actionTextUI;
    public GameObject skillSelectUI;

    public ScrollRect logScroll;
    public Text turnText;

    public GameObject infoPanel;
    public Image infoImage;
    public GameObject processingChara;

    //GameOver UI
    public GameObject gameOverUI;

    //GameWin UI
    public GameObject gameWinUI;

    //Pause UI
    public GameObject gamePauseUI;

    void Update()
    {
        if(!Input.GetMouseButton(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if(!GameManager.instance.isPause && hit.collider != null)
            {
                GameObject clickObject = hit.transform.gameObject;
                if(clickObject.tag == "Enemy" || clickObject.tag == "Player")
                {
                    infoPanel.SetActive(true);
                    if(Input.GetAxisRaw("Status") != 0)
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

    //GamePause UI 메서드
    public void gamePause()
    {
        gamePauseUI.SetActive(true);
        Time.timeScale = 0.0f;
    }
    public void gameResume()
    {
        gamePauseUI.SetActive(false);
        Time.timeScale = 1.0f;
    }
}
