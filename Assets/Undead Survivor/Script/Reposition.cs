using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Reposition : MonoBehaviour
{
    // Collider 2D 변수 생성 및 초기화
    // 몬스터가 죽을 때 시체가 살아있는 몬스터와 충돌하지 않도록 Capsule Collider 2D를 비활성화
    Collider2D coll;

    void Awake()
    {
        // Collider 2D는 기본 도형의 모든 콜라이더 2D를 포함
        coll = GetComponent<Collider2D>();
    }

    // OnTriggerExit2D 함수 작성
    void OnTriggerExit2D(Collider2D collisioin)
    {
        // Player가 Area와 충돌하여 벗어났을 때만 타일을 움직여준다
        // OnTriggerExit2D의 매개변수 상대방 콜라이더의 태그를 조건으로!
        if (!collisioin.CompareTag("Area"))
            // return 키워드를 만나면 더 이상 실행하지 않고 함수 탈출
            return;

        // Player 위치와 Tilemap 위치 필요
        // 거리를 구하기 위해 플레이어 위치와 타일맵 위치를 미리 저장
        Vector3 playerPos = GameManager.instance.player.transform.position;
        Vector3 myPos = transform.position;

        // 더 이상 재배치 로직에 플레이어 입력은 제외
        /*
        // x축, y축 거리
        // 플레이어 위치 - 타일맵 위치 계산으로 거리 구하기
        // Mathf.Abs : 음수도 양수로 만들어주는 절대값 함수
        float diffX = Mathf.Abs(playerPos.x - myPos.x);
        float diffY = Mathf.Abs(playerPos.y - myPos.y);
                
        // 플레이어의 이동 방향을 저장하기 위한 변수 추가
        Vector3 playerDir = GameManager.instance.player.inputVec;

        // 대각선일 때는 Normalized에 의해 1보다 작은 값이 되어버림
        // 3항 연산자 (조건) ? (true일 때 값) : (false일 때 값)
        float dirX = playerDir.x < 0 ? -1 : 1;
        float dirY = playerDir.y < 0 ? -1 : 1;
        */

        // switch ~ case : 값의 상태에 따라 로직을 나눠주는 키워드
        switch (transform.tag){
            case "Ground":
                // 두 오브젝트의 위치 차이를 활용한 로직으로 변경
                // x축, y축 거리
                // 플레이어 위치 - 타일맵 위치 계산으로 거리 구하기
                // Mathf.Abs : 음수도 양수로 만들어주는 절대값 함수
                float diffX = playerPos.x - myPos.x; // 플레이어의 거리와 나의 거리 차이 확인함
                float diffY = playerPos.y - myPos.y;
                // 대각선일 때는 Normalized에 의해 1보다 작은 값이 되어버림
                // 3항 연산자 (조건) ? (true일 때 값) : (false일 때 값)
                float dirX = diffX < 0 ? -1 : 1;
                float dirY = diffY < 0 ? -1 : 1;
                diffX = Mathf.Abs(diffX); //절댓값으로 환산
                diffY = Mathf.Abs(diffY);

                // 두 오브젝트의 거리 차이에서, X축이 Y축보다 크면 수평 이동
                if (diffX > diffY){
                    // Translate : 지정된 값 만큼 현재 위치에서 이동
                    transform.Translate(Vector3.right * dirX * 40);
                }
                else if(diffY > diffX){
                    // Translate : 지정된 값 만큼 현재 위치에서 이동
                    transform.Translate(Vector3.up * dirY * 40);
                }
                break;
            case "Enemy":
                // 콜라이더가 활성화 되어있는지 조건 먼저 작성
                if (coll.enabled){
                    // 두 오브젝트의 거리를 그대로 활용하는 것이 포인트
                    Vector3 dist = playerPos - myPos;
                    // 랜덤 벡터를 더하여 퍼져있는 몬스터 재배치 만들기
                    // + 기존 위치의 2배를 더해서 이동하게 하면 반대방향으로 나오므로 해당 로직 활용
                    Vector3 ran = new Vector3(UnityEngine.Random.Range(-3,3),UnityEngine.Random.Range(-3,3) ,0);
                    // 플레이어의 이동 방향에 따라 맞은 편에서 등장하도록 이동
                    // 카메라가 없는 곳에서 등장하게끔 20을 곱해준다
                    // 랜덤한 위치에서 등장하도록 벡터 더하기
                    transform.Translate(ran + dist * 2); 
                }
                break;
        }
    }
}
