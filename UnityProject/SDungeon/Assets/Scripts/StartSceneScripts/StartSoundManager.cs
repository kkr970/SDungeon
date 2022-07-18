using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartSoundManager : MonoBehaviour
{
    //audio
    public AudioSource bgmAudio;

    //Slider
    public Slider bgmVolume;
    
    //Value
    private float bgmValue = 0.2f;

    void Start()
    {
        bgmValue = StartManager.instance.bgmValue;
        bgmVolume.value = bgmValue;
        bgmAudio.volume = bgmVolume.value;
    }

    void Update()
    {
        bgmSoundSlider();
    }


    //bgm Slider 메서드
    public void bgmSoundSlider()
    {
        bgmAudio.volume = bgmVolume.value;

        bgmValue = bgmVolume.value;
        PlayerPrefs.SetFloat("bgmValue", bgmValue);
    }
}
