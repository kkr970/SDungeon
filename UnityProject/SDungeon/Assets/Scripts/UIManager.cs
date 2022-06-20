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
    public ScrollRect logScroll;
    public Text turnText;
    public GameObject infoPanel;
    public Image infoImage;

    //GameOver UI
    public GameObject gameOverUI;

    //GameWin UI
    public GameObject gameWinUI;

    void Update()
    {
        if(!Input.GetMouseButton(0))
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
            if(hit.collider != null)
            {
                GameObject clickObject = hit.transform.gameObject;
                if(clickObject.tag == "Enemy" || clickObject.tag == "Player")
                {
                    infoPanel.SetActive(true);
                    updateInfoText(clickObject.GetComponent<CharacterManager>().getInfo());
                    updateInfoImage(clickObject.GetComponent<SpriteRenderer>().sprite);
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

        //로그 업데이트
    public void updateLogText(string log)
    {
        string text = logScroll.GetComponentInChildren<Text>().text;
        logScroll.GetComponentInChildren<Text>().text = "> " + log + text;
        logScroll.GetComponentInChildren<Scrollbar>().value = 1.0f;
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
}
