using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

public class Scanner : MonoBehaviour
{
    // 범위, 레이어, 스캔 결과 배열, 가장 가까운 목표를 담을 변수 선언
    public float scanRange; //범위
    public LayerMask targetLayer; //레이어
    public RaycastHit2D[] targets; //스캔 결과 배열
    public Transform nearestTarget; //가장 가까운 목표

    void FixedUpdate()
    {
        // CircleCastAll : 원형의 캐스트를 쏘고 모든 결과를 반환하는 함수
        // 1. 캐스팅 시작 위치
        // 2. 원의 반지름
        // 3. 캐스팅 방향
        // 4. 캐스팅 길이
        // 5. 대상 레이어
        //(캐스팅 시작 위치, 원의 반지름, 캐스팅 방향, 캐스팅 길이, 대상레이어)
        targets = Physics2D.CircleCastAll(transform.position, scanRange, Vector2.zero, 0, targetLayer);
        // 완성된 함수를 통해 지속적으로 가장 가까운 목표 변수를 업데이트
        nearestTarget = GetNearest();
    }

    //가장 가까운 것을 찾는 함수 추가
    Transform GetNearest()
    {
        Transform result = null;
        float diff = 100; //거리

        //foreach 문으로 캐스팅 결과 오브젝트를 하나씩 접근
        foreach (RaycastHit2D target in targets){
            Vector3 myPos = transform.position;
            Vector3 targetPos = target.transform.position;
            //Distance(A, B) : 벡터 A 와 벡터 B의 거리를 계산해주는 함수
            float curDiff = Vector3.Distance(myPos,targetPos);

            //반복문을 돌며 가져온 거리가 저장된 거리보다 작으면 교체
            //가장 가까운 타깃이 결과에 저장되도록
            if (curDiff < diff) { 
                diff = curDiff;
                result = target.transform;
            }
        }

        return result;
    }

}
