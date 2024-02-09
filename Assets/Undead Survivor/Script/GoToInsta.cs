using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GoToInsta : MonoBehaviour
{
    // 인스타그램 계정 URL
    private string instagramURL = "https://www.instagram.com/sookmyung_tums?igsh=eTh3eGs2c3hiYzNz";


    // Start is called before the first frame update
    void Start()
    {
        // 버튼에 클릭 이벤트 추가
        Button Insta_bt = GetComponent<Button>();

        if (Insta_bt != null)
        {
            Insta_bt.onClick.AddListener(OpenInstagramPage);
        }
     
    }

    // 인스타그램 페이지를 열기 위한 메서드
    void OpenInstagramPage()
    {
        Application.OpenURL(instagramURL);
    }
}


