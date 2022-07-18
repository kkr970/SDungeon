using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;


public class StartUIManager : MonoBehaviour
{
    private static StartUIManager sui_instance;
    public static StartUIManager instance
    {
        get{
            if(sui_instance == null)
            {
                sui_instance = FindObjectOfType<StartUIManager>();
            }
            return sui_instance;
        }
    }

    //Setting UI
    public GameObject settingUI;

    /*
    void Update()
    {
        
    }
    */

    //클릭 오브젝트 이름 가져오기
    public GameObject mouseGetObject()
    {
        Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f);
        GameObject clickObject = null;
        if(hit.collider != null)
        {
            clickObject = hit.transform.gameObject;
            return clickObject;
        }
        return null;
    }

    //Setting Ui On/Off
    public void settingUI_ON()
    {
        settingUI.SetActive(true);
    }
    public void settingUI_OFF()
    {
        settingUI.SetActive(false);
    }
}
