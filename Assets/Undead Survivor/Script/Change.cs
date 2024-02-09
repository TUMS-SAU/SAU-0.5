using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Change : MonoBehaviour
{
    //public void FirstToCharacter() // 첫번째 Scene에서 캐릭터 선택 Scene으로
    //{
    //    SceneManager.LoadScene("CharacterScene");
    //}

    public void CharacterToMap() // 캐릭터 선택 Scene에서 맵 선택 Scene으로
    {
        SceneManager.LoadSceneAsync("MapScene");
    }

    public void MapToGame() // 맵 선택 Scene에서 게임화면으로
    {
        SceneManager.LoadSceneAsync("SampleScene");
    }

}


