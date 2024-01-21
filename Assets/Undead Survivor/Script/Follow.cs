using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Follow : MonoBehaviour
{
    // UI에는 Transform과 RectTransform이 있다
    // RectTransform은 Transform과 달리 선언 및 초기화를 해줘야 한다
    // RectTransform 변수 선언 및 초기화
    RectTransform rect; 

    void Awake()
    {
        rect = GetComponent<RectTransform>();
    }

    void FixedUpdate()
    {
        //월드 좌표와 스크린 좌표는 다르므로 
        //rect.position = GameManager.instance.player.transform.position; 이렇게 하면 안됨
        rect.position = Camera.main.WorldToScreenPoint(GameManager.instance.player.transform.position);
        //WorldToScreenPoint : 월드 상의 오브젝트 위치를 스크린 좌표로 변환
    }
}
