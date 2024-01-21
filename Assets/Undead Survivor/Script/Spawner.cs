using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Android;

// 하나의 스크립트 내에 여러 클래스를 선언할 수 있다
public class Spawner : MonoBehaviour
{
    // Spawner에서 PoolManager의 Get을 쓸 것이다
    // PoolManager을 써도 되지만 이렇게되면 Spawner를 Pool에서만 쓰게 되기 때문에 GameManager에서 쓴다

    // 자식 오브젝트의 Transform을 담을 배열 변수 선언
    public Transform[] spawnPoint;
    // 만든 클래스 SpawnData를 그대로 타입으로 활용하여 배열 변수 선언
    public SpawnData[] spawnData;
    // Spawner 스크립트에 소환 레벨 구간을 결정하는 변수 선언
    public float levelTime; //소환 레벨 구간을 결정하는 변수

    // 소환 스크립트에서 레벨 담당 변수 선언
    int level;
    // 소환 타이머를 위한 변수 선언
    float timer;

    void Awake()
    {
        // GetComponent : 하나를 가져온다 / GetComponents : 다수를 가져온다
        // GetComponentsInChildren 함수로 초기화
        spawnPoint = GetComponentsInChildren<Transform>(); //Transform은 어느 오브젝트나 존재하기 때문에 0이 본인의 Transform이므로
                                                           //원하는 위치를 찾으려면 1부터 시작해야 한다.
        // 최대 시간에 몬스터 데이터 크기로 나누어 자동으로 구간 시간 계산
        levelTime = GameManager.instance.maxGameTime / spawnData.Length; 
    }

    // Update is called once per frame
    void Update()
    {
        if(!GameManager.instance.isLive)
            return;

        // timer 변수에는 deltaTime을 계속 더하기
        timer += Time.deltaTime;
        // 적절한 숫자로 나누어 시간에 맞춰 레벨이 올라가도록 작성
        // FloorToInt : 소수점 아래는 버리고 Int형으로 바꾸는 함수
        // CeilToInt : 소수점 아래를 올리고 Int형으로 바꾸는 함수 (소수점 올림)
        //level = Mathf.FloorToInt(GameManager.instance.gameTime / 10f);
        // 인덱스 에러는 레벨 변수 계산 시 Min 함수를 사용하여 막을 수 있다
        //level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / 10f), spawnData.Length - 1);
        level = Mathf.Min(Mathf.FloorToInt(GameManager.instance.gameTime / levelTime), spawnData.Length -1); //게임 시간에 levelTime을 나누어 레벨 설정

        // timer가 일정 시간 값에 도달하면 소환하도록 작성
        //if (timer > 0.2f)
        // 레벨을 활용해 소환 타이밍을 변경하기
        //if (timer > (level == 0 ? 0.5f : 0.2f))
        // 소환 시간 조건을 소환데이터로 변경
        if (timer > spawnData[level].spawnTime){
            timer = 0;
            Spawn();
        }

        /*
        // 테스트를 위해 Update 안에서 점프 버튼 입력 조건 추가
        if (Input.GetButtonDown("Jump"))
        {
            // GameManager의 instance까지 접근하여 풀링의 함수 호출
            //0번 해골 1번 좀비
            GameManager.instance.pool.Get(1);
        }
        */
    }

    // 소환 함수를 새로 작성
    void Spawn()
    {
        // Pool 함수에는 Random 인자 값을 넣도록 변경
        // Range(0, 2)여야 0 ~ 1사이에서 설정 가능하다
        // Get 함수는 GameObject를 반환하고 있던 함수
        // Instantiate 반환 값을 변수에 넣어두기
        //GameObject enemy = GameManager.instance.pool.Get(Random.Range(0, 2));
        // 풀링에서 가져오는 함수에도 레벨 적용
        //GameObject enemy = GameManager.instance.pool.Get(level);
        // PoolManager에서 호출하는 함수 인자 값을 0으로 변경
        GameObject enemy = GameManager.instance.pool.Get(0);

        // 만들어둔 소환 위치 중 하나로 배치되도록 작성
        // 자식 오브젝트에서만 선택되도록 랜덤 시작은 1부터
        // 자기 자신은 0이다
        //적의 위치를 spwanpoint를 통해 지정
        enemy.transform.position = spawnPoint[Random.Range(1, spawnPoint.Length)].position;

        // 오브젝트 풀에서 가져온 오브젝트에서 Enemy 컴포넌트로 접근
        // 새롭게 작성한 함수 Init을 호출하고 소환데이터 인자값 전달
        enemy.GetComponent<Enemy>().Init(spawnData[level]); 

    }
}

// 나만의 클래스를 만들어 데이터를 효율적으로 사용할 수 있다
//직렬화 (Serialization) : 개체를 저장 혹은 전송하기 위해 변환, Inspeactor 창에서도 볼 수 있도록 설정
// System.Serializable 속성 부여
[System.Serializable]
// 소환 데이터를 담당하는 클래스 선언
public class SpawnData
{
    // 추가할 속성들 : 스프라이트 타입, 소환시간, 체력, 속도
    public float spawnTime; //소환 시간
    public int spriteType; //적의 종류
    public int health; //체력
    public float speed; //속도
}
