using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    //함수가 아닌 속성을 작성
    public static float Speed
    {
        // 삼항연산자를 활용하여 캐릭터에 따라 특성치 적용
        get { return GameManager.instance.playerId == 0 ? 1.1f : 1f;}
    }

    public static float WeaponSpeed
    {
        // 캐릭터 특성치에 맞는 각종 속성들을 작성
        get { return GameManager.instance.playerId == 1 ? 1.1f : 1f;}
    }

    public static float WeaponRate
    {
        // 캐릭터 특성치에 맞는 각종 속성들을 작성
        get { return GameManager.instance.playerId == 1 ? 0.9f : 1f;}
    }

    public static float Damage
    {
        // 캐릭터 특성치에 맞는 각종 속성들을 작성
        get { return GameManager.instance.playerId == 2 ? 1.2f : 1f;}
    }

    public static int Count
    {
        // 캐릭터 특성치에 맞는 각종 속성들을 작성
        get { return GameManager.instance.playerId == 3 ? 1 : 0;}
    }
}
