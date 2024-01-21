using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hand : MonoBehaviour
{
    //오른쪽, 왼쪽 구분을 위한 변수 선언
    public bool isLeft;
    public SpriteRenderer spriter;

    // 플레이어의 스프라이트렌더러 변수 선언 및 초기화
    SpriteRenderer player;

    // 오른손의 각 위치를 Vector3 형태로 저장
    Vector3 rightPos = new Vector3(0.35f, -0.15f, 0);
    Vector3 rightPosReverse = new Vector3(-0.15f, -0.15f, 0);
    // 왼손의 각 회전을 Quaternion 형태로 저장
    Quaternion leftRot = Quaternion.Euler(0, 0, -35);
    Quaternion leftRotReverse = Quaternion.Euler(0, 0, -135);
    void Awake()
    {
        player = GetComponentsInParent<SpriteRenderer>()[1]; // 2번째가 부모의 스프라이트 랜더러이다. 
    }
    
    void LateUpdate()
    {
        // 플레이어의 반전 상태를 지역변수로 저장
        bool isReverse = player.flipX;

        //근접 무기
        if (isLeft){ 
        // 왼손 회전에는 localRotation 사용
        transform.localRotation = isReverse ? leftRotReverse : leftRot;
            spriter.flipY = isReverse; //왼쪽 스프라이트는 Y축 반전
            // 왼손, 오른손의 sortingOrder를 바꿔주기
            spriter.sortingOrder = isReverse ? 4 : 6; //반전되었을때는 4번째 순서로 아니면 6번째
        }
        //원거리 무기
        else
        {
            // 오른손 이동에는 localPosition 사용
            transform.localPosition = isReverse ? rightPosReverse : rightPos;
            // 오른손 스프라이트는 X축 반전
            spriter.flipX = isReverse; //오른쪽 스프라이트는 X축 반전
            // 왼손, 오른손의 sortingOrder를 바꿔주기
            spriter.sortingOrder = isReverse ? 6 : 4;
        }
    }
}
