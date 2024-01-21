using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoolManager : MonoBehaviour
{
    // Prefabs들을 보관할 변수
    // Prefabs들을 저장할 배열 변수 선언
    public GameObject[] prefabs; // [] : 여러개를 넣을 수 있는 배열 형태인 것을 지정
    // Pool 담당을 하는 리스트들
    // 오브젝트 Pool들을 저장할 배열 변수 선언
    List<GameObject>[] pools; //List<타입>[배열] 

    // Prefabs와 리스트들은 일대일 관계

    // 초기화
    void Awake()
    {
        // pool 초기화
        // 리스트 배열 변수 초기화할 때 크기는 Prefabs 배열 길이 활용
        pools = new List<GameObject>[prefabs.Length];
        // for (반복문) : 시작문; 조건문; 증감문
        //배열 안에 들어있는 각각의 리스트도 순회하면서 초기화
        for (int index = 0; index < pools.Length; index++){ //index가 0부터 시작하므로 프리팹이 2개이면 0,1로 2번 반복
            // 반복문을 통해 모든 오브젝트 풀 리스트를 초기화
            pools[index] = new List<GameObject>(); //생성자의 함수 의미로 (); 
        }

        //Debug.Log(pools.Length); //콘솔창에 pools.Length 출력

        
    }

    // 게임 오브젝트를 반환하는 함수 선언
    // 가져올 오브젝트 종류를 결정하는 매개변수 추가
    public GameObject Get(int index)
    {
        // 게임 오브젝트를 반환하기 위해 지역변수 선언
        // GameObject 지역변수와 리턴을 미리 작성
        GameObject select = null;

        // 선택한 Pool의 놀고 있는 (비활성화 된) GameObject 접근
        // 발견하면 select 변수에 할당

        // foreach : 배열, 리스트들의 데이터를 순차적으로 접근하는 반복문
        // pools가 GameObject의 List이기 때문에 타입을 GameObject로 맞춰준다
        foreach (GameObject item in pools[index]){ // 배열, 리스트를 순회해서 순차적으로 데이터에 접근하는 반복문
            // 내용물 오브젝트가 비활성화(대기 상태)인지 확인
            if (!item.activeSelf){
                // 발견하면 select 변수에 할당
                select = item;
                // 비활성화(대기 상태) 오브젝트를 찾으면 SetActive 함수로 활성화
                select.SetActive(true);
                // break 키워드로 반복문 종료
                break;
            } 
        }

        // 못찾았으면 ? 
        // 미리 선언한 변수가 계속 비어있으면 생성 로직으로 진입
        //select == null 은 !select로 해도 됨
        if (select == null){  
            //새롭게 생성하고 select 변수에 할당
            // Instantiate(게임 오브젝트, 부모 오브젝트의 Transform) : 원본 오브젝트를 복제하여 장면에 생성하는 함수
            // 원본 게임 오브젝트를 복제하여 장면에 두고 이러한 게임 오브젝트를 반환
            /// transform : 내 자신 안에다가 넣겠다 
            /// (없으면 hierachy창에 추가되어 보기 지저분)
            select = Instantiate(prefabs[index], transform);
            // 생성된 오브젝트는 해당 오브젝트 풀 리스트에 Add 함수로 추가
            pools[index].Add(select);
        }
        return select;
    }
}
