using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class StartManager : MonoBehaviour
{
    private static StartManager sm_instance;
    public static StartManager instance
    {
        get{
            if(sm_instance == null)
            {
                sm_instance = FindObjectOfType<StartManager>();
            }
            return sm_instance;
        }
    }

    //사운드 변수
    public float bgmValue;
    public AudioSource bgmAudio;

    //설정 on/off 불린
    private bool isSetting = false;

    void Start()
    {
        //sound
        bgmValue = PlayerPrefs.GetFloat("bgmValue", 0.2f);
        bgmAudio.volume = bgmValue;

        //설정 on/off
        isSetting = false;
    }

    void Update()
    {
        if(!isSetting)
        {
            //버튼 클릭
            if(Input.GetMouseButtonDown(0))
            {
                GameObject clickObject = StartUIManager.instance.mouseGetObject();
                if(clickObject != null)
                {
                    // 시작
                    if(clickObject.name == "StartButton")
                    {
                        SceneManager.LoadScene("BattleScene");
                    }
                    // 설정
                    if(clickObject.name == "SettingButton")
                    {
                        StartUIManager.instance.settingUI_ON();
                        isSetting = true;
                    }
                    // 종료
                    if(clickObject.name == "QuitButton")
                    {
                        #if UNITY_EDITOR
                        UnityEditor.EditorApplication.isPlaying = false;
                        #else
                        Application.Quit();
                        #endif
                    }
                }
            }
        }
        //설정창이 켜짐
        else if(isSetting)
        {
            if(Input.GetMouseButtonDown(0))
            {
                GameObject clickObject = StartUIManager.instance.mouseGetObject();
                if(clickObject != null)
                {
                    // 설정창 닫기
                    if(clickObject.name == "Back")
                    {
                        StartUIManager.instance.settingUI_OFF();
                        isSetting = false;
                    }
                }
            }

            // 설정창 닫기
            if(Input.GetKeyDown(KeyCode.Escape))
            {
                StartUIManager.instance.settingUI_OFF();
                isSetting = false;
            }
        }

    }

}
