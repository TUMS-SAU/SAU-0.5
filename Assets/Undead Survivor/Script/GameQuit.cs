using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameQuit : MonoBehaviour
{
    // Start is called before the first frame update
    public void GameQuitMain()
    {
        Debug.Log("게임종료");
        Application.Quit(); //게임을 종료하는 함수 실행 
        //에디터를 종료하는 기능이 아니므로, 빌드 버전에서만 작동
    }
}
