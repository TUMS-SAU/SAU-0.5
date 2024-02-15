using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pause : MonoBehaviour
{
    public RectTransform rect;

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    //보이는 함수 생성
    public void Show()
    {
       
        rect.localScale = Vector3.one;
        //스케일을 1,1,1 로 만들어서 원상복귀
        GameManager.instance.Stop();

        //효과음을 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        //배경음 잠깐 끄기
        AudioManager.instance.EffectBgm(true);
    }

    //숨기는 함수 생성
    public void Hide()
    {
        rect.localScale = Vector3.zero; 
        //스케일을 0,0,0으로 만들어서 창 크기를 아예 0으로 없애버림

        GameManager.instance.Resume();

        //효과음을 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Select);
        //배경음 다시 켜기
        AudioManager.instance.EffectBgm(false);
    }
    
   
    
}
