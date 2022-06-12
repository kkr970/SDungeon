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

    public Text turnText;

    public void UpdateTurnText(int newTurn)
    {
        turnText.text = "Turn : " + newTurn;
    }
}
