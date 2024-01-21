using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;

public class Weapon : MonoBehaviour
{
    // poolManaager에서 받은 무기를 모양새 있게 관리하는 역할
    // 무기 ID, 프리펩 ID, 데미지, 개수, 속도 변수 선언
    public int id; //무기 id
    public int prefabId; //프리펩 id
    public float damage;//데미지
    public int count; //개수. 몇 개의 무기를 배치할 것이냐
    public float speed; //회전속도 혹은 연사 속도

    // 타이머를 위한 float 변수 추가
    float timer;

    // 무기 관리 스크립트에 플레이어 변수 선언
    Player player;

    void Awake(){
        // GetComponentInParent 함수로 부모의 컴포넌트 가져오기
        //player = GetComponentInParent<Player>();
        // Awake 함수에서의 플레이어 초기화는 게임매니저 활용으로 변경
        player = GameManager.instance.player; //게임메니저 활용으로 초기화
    }

    // Update is called once per frame
    // Update 로직도 switch 문 활용하여 무기마다 로직 실행
    void Update()
    {
        if(!GameManager.instance.isLive)
            return;
        
        //무기 id에 따라 로직을 분리할 switch문 작성
        switch (id){
            case 0: //근접무기 : 삽
                // deltaTime은 한 프레임 당 소비하는 시간
                //transform.Rotate(Vector3.forward * speed * Time.deltaTime);
                transform.Rotate(Vector3.back * speed * Time.deltaTime); //회전 속도에 맞춰서 돌도록 하기
                break;
            default:
                // Update에서 deltaTime을 계속 더하기
                timer += Time.deltaTime; //deltaTime : 한 프레임이 소비하는 시간

                // speed보다 커지면 초기화하면서 발사 로직 실행
                if (timer > speed) {
                    timer = 0f;
                    Fire();
                }
                break;
        }

        //TestCode
        if (Input.GetButtonDown("Jump"))
            // 레벨업 테스트를 위해 Update에 간단한 호출 로직 작성
            //LevelUp(20, 5);
            LevelUp(10, 1);
    }

    // 레벨업 기능 함수 작성
    public void LevelUp(float damage,int count)
    {
        this.damage = damage * Character.Damage;
        this.count += count;

        // 속성 변경과 동시에 근접무기의 경우 배치도 필요하니 함수 호출
        if (id == 0)
            Batch();

        // BroadcastMessage를 초기화, 레벨업 함수 마지막 부분에서 호출
        //player.BroadcastMessage("ApplyGear");
        // BroadcastMessage의 두 번째 인자값으로 DontRequireReceiver 추가
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); 
        //아이템을 누를 때 데미지 카운트가 적용되므로, 기어 데미지가 초기화 될 수 있기 때문에 적용
    }

    //초기화함수
    // Weapon 초기화 함수에 스크립터블 오브젝트를 매개변수로 받아 활용
    public void Init(ItemData data)
    {
        //Basic Set
        name = "Weapon " + data.itemId; //Weapon n 으로 만들어서 
        transform.parent = player.transform; //부모 오브젝트를 플레이어로 지정
        transform.localPosition = Vector3.zero; //지역 위치인 localPosition을 원점으로 변경

        //Property Set
        //각종 무기 속성 변수들을 스크립트블 오브젝트 데이터로 초기화
        id = data.itemId; 
        damage = data.baseDamage * Character.Damage;
        count = data.baseCount + Character.Count;

        //for문으로 프리팹아이디를 풀링 매니저의 변수에서 찾아서 초기화
        //스크립트블 오브젝트의 독립성을 위해서 인덱스가 아닌 프리펩으로 설정
        for (int index = 0 ; index < GameManager.instance.pool.prefabs.Length; index++){
            // 프리펩 아이디는 풀링 매니저의 변수에서 찾아서 초기화
            // 스크립터블 오브젝트의 독립성을 위해서 인덱스가 아닌 프리펩으로 설정
            if (data.projectile == GameManager.instance.pool.prefabs[index]){
                prefabId = index;
                break;
            }
        }

        // 무기 ID에 따라 로직을 분리할 switch 문 작성
        // 무기 ID 하나씩 case ~ break 으로 감싸기
        // 그 외 나머지 경우가 있다면 default ~ break 으로 감싸기
        switch (id){
            case 0: //근접무기 : 삽
                speed = 150 * Character.WeaponSpeed;
                Batch();
                break;
            
            default: //원거리 무기 : 총
                // speed 값은 연사속도를 의미 : 적을 수록 많이 발사
                speed = 0.5f * Character.WeaponRate;
                break;
        }

        // Weapon 스크립트의 초기화 함수에서 로직 작성
        // Hand Set
        // enum 열거형 데이터는 정수 형태로도 사용 가능
        // enum 값 앞에 int 타입을 작성하여 강제 형변환
        Hand hand = player.hands[(int)data.itemType]; //enum의 데이터는 정수 형태로도 사용 가능
                                                      //enum 값 앞에 int 타입을 작성하여 강제 형 변환
        // 스크립터블 오브젝트의 데이터로 스프라이트 적용
        hand.spriter.sprite = data.hand;
        // SetActive 함수로 활성화
        hand.gameObject.SetActive(true);


        // BroadcastMessage : 특정 함수 호출을 모든 자식에게 방송하는 함수
        //player.BroadcastMessage("ApplyGear");
        // BroadcastMessage의 두 번째 인자값으로 DontRequireReceiver 추가
        player.BroadcastMessage("ApplyGear", SendMessageOptions.DontRequireReceiver); 
        ///player가 가지고 있는 모든 기어에 한해서 applyGear가 되도록 하는 것
        ///오류를 막기 위해 DontRequireReceiver를 두번째 인자값으로 추가
    }

    // Batch : 자료를 모아 두었다가 일괄해서 처리하는 자료처리의 형태
    // 생성된 무기를 배치하는 함수 생성 및 호출
    void Batch()
    {
        // for 문으로 count만큼 풀링에서 가져오기
        for (int index = 0; index < count; index++){

            // 가져온 오브젝트의 Transform을 지역변수로 저장
            Transform bullet;

            // 기존 오브젝트를 먼저 활용하고 모자란 것은 풀링에서 가져오기
            // 자신의 자식 오브젝트 개수 확인은 childCount 속성
            //bullet 초기화
            if (index < transform.childCount) {
                //기존 오브젝트를 먼저 활용하고 모자란 것은 풀링에서 가져오기
                //index가 아직 childCount 범위 내라면 GetChild 함수로 가져오기
                bullet = transform.GetChild(index); 
            }
            else {
                // 가져온 오브젝트의 Transform을 지역변수로 저장
                bullet = GameManager.instance.pool.Get(prefabId).transform;
                //poolManager에서 원하는 프리팹을 가져오고 무기의 개수(count) 만큼 돌려서 배치
                //parent 속성을 통해 부모를 내 자신(스크립트가 들어간 곳)으로 변경
                bullet.parent = transform; 
            }



            // 배치하면서 먼저 위치, 회전 초기화 하기
            bullet.localPosition = Vector3.zero;
            bullet.localRotation = Quaternion.identity;

            Vector3 rotVec = Vector3.forward * 360 * index / count;
            // Rotate 함수로 계산된 각도 적용
            bullet.Rotate(rotVec);
            // Translate 함수로 자신의 위쪽으로 이동
            // 이동 방향은 Space World 기준으로
            bullet.Translate(bullet.up * 1.5f, Space.World); //이동 방향은 Space World 기준으로 

            // bullet 컴포넌트 접근하여 속성 초기화 함수 호출
            // 누가 봐도 파악하기 쉽도록 주석을 달아두면 좋습니다.
            // -1 is Infinity Per.
            // 근접 무기는 무조건 관통하기 때문에 관통 숫자가 의미 없으므로 무한인 -1로 둔다
            //bullet.GetComponent<Bullet>().Init(damage, -1);
            // 근접 공격에 사용했던 초기화 함수 호출 고쳐주기
            //bullet.GetComponent<Bullet>().Init(damage, -1, Vector3.zero);
            // 근거리 무기를 의미하는 관통력을 -100으로 재설정
            // -100 is Infinity Per.
            bullet.GetComponent<Bullet>().Init(damage, -100, Vector3.zero); //근접 무기는 계속 관통하기 때문에 per(관통)을 무한으로 관통하게 -100로 설정
        }
    }

    void Fire()
    {
        // 저장된 목표가 없으면 넘어가는 조건 로직 작성
        if (!player.scanner.nearestTarget) //만약 가장 가까운 타깃이 없다면 실행하지 않음
            return;

        Vector3 targetPos = player.scanner.nearestTarget.position;
        // 크기가 포함된 방향 : 목표 위치 - 나의 위치
        Vector3 dir = targetPos - transform.position;
        // normalized : 현재 벡터의 방향은 유지하고 크기를 1로 변환된 속성
        dir = dir.normalized;

        // 기존 생성 로직을 그대로 활용하면서 위치는 플레이어 위치로 지정
        Transform bullet = GameManager.instance.pool.Get(prefabId).transform;
        bullet.position = transform.position; //플레이어 위치에서 쏘는 것으로 고정

        // FromToRotation : 지정된 축을 중심으로 목표를 향해 회전하는 함수
        bullet.rotation = Quaternion.FromToRotation(Vector3.up, dir);
        // 원거리 공격에 맞게 초기화 함수 호출하기
        bullet.GetComponent<Bullet>().Init(damage, count, dir);

        //효과음을 재생할 부분마다 재생함수 호출
        AudioManager.instance.PlaySfx(AudioManager.Sfx.Range);
    }
}
