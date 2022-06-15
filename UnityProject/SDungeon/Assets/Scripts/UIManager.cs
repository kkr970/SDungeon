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
    public ScrollRect logScroll;
    public Text turnText;

    //GameOver UI
    public GameObject gameOverUI;

    //GameWin UI
    public GameObject gameWinUI;
    
    //Battle UI 메서드
        //턴 업데이트
    public void updateTurnText(int newTurn)
    {
        turnText.text = "Turn : " + newTurn;
    }
        //로그 업데이트
    public void updateLogText(string log)
    {
        string text = logScroll.GetComponentInChildren<Text>().text;
        logScroll.GetComponentInChildren<Text>().text = "> " + log + text;
        logScroll.GetComponentInChildren<Scrollbar>().value = 1.0f;
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
