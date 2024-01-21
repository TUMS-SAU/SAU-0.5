using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    // 속도, 목표, 생존여부를 위한 변수 선언
    public float speed;
    // 체력 관련 변수도 함께 선언
    public float health;
    public float maxHealth;
    // 애니메이션 Sprite를 바꾸는 데이터
    // Animator의 데이터는 AnimatorController
    // RuntimeAnimatorController 변수 선언
    public RuntimeAnimatorController[] animCon;
    public Rigidbody2D target;

    // 아직 테스트 상태이므로 미리 isLive = true 적용
    //bool isLive = true;
    // OnEnable에서 생존여부와 체력 초기화
    bool isLive;

    // Rigidbody 2D와 Sprite Renderer를 위한 변수 선언
    Rigidbody2D rigid;
    // Collider2D 변수를 생성 및 초기화
    Collider2D coll;
    // 애니메이터 변수 선언 및 초기화하고 이후 로직 작성하기
    Animator anim;
    SpriteRenderer spriter;
    // WaitForFixedUpdate 변수 선언 및 초기화
    WaitForFixedUpdate wait; //다음 fixedUpdate가 될때까지 기다리는 변수

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
        coll = GetComponent<Collider2D>();
        spriter = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();
        wait = new WaitForFixedUpdate();
    }

    // 물리적인 이동이기 때문에 FixedUpdate를 사용
    void FixedUpdate()
    {
        if(!GameManager.instance.isLive)
            return;

        // 몬스터가 살아있는 동안에만 움직이도록 조건 추가
        //if (!isLive)
        // GetCurrentAnimatorStateInfo : 현재 상태 정보를 가져오는 함수
        // IsName : 해당 상태의 이름이 지정된 것과 같은지 확인하는 함수
        if (!isLive || anim.GetCurrentAnimatorStateInfo(0).IsName("Hit"))
            return; //만약 몬스터가 죽은 상태이거나 맞는 상태이면(넉백위해서) 작동하지 않음

        // 위치 차이 = 타겟 위치 - 나의 위치
        Vector2 dirVec = target.position - rigid.position;
        // 방향 = 위치 차이의 정규화 (Normalized)
        // 프레임의 영향으로 결과가 달라지지 않도록 FixedDeltaTime 사용
        Vector2 nextVec = dirVec.normalized * speed * Time.fixedDeltaTime; //다음에 가야할 위치의 양
        //플레이어의 키입력 값을 더한 이동 = 몬스터의 방향 값을 더한 이동
        rigid.MovePosition(rigid.position + nextVec);
        // 물리 속도가 이동에 영향을 주지 않도록 속도 제거
        rigid.velocity = Vector2.zero; //몬스터와 플레이어가 부딪힐 때 발생하는 속도를 (0,0)으로 고정
    }   

    void LateUpdate()
    {
        if(!GameManager.instance.isLive)
            return;

        // 몬스터가 살아있는 동안에만 움직이도록 조건 추가
        if (!isLive)
            return;

        // 목표의 X축 값과 자신의 X축 값을 비교하여 작으면 true가 되도록 설정
        spriter.flipX = target.position.x < rigid.position.x;
    }

    // Enemy는 Scene에 새롭게 등장하면서 활성화된다
    // 활성화되면서 자동으로 실행되는 함수
    // OnEnable : 스크립트가 활성화 될 때, 호출되는 이벤트 함수
    void OnEnable()
    {
        // OnEnable에서 target 변수에 GameManager를 활용하여 Player 할당
        target = GameManager.instance.player.GetComponent<Rigidbody2D>();
        // OnEnable에서 생존여부와 체력 초기화
        isLive = true;

        // 재활용을 위해 OnEnable 함수에서 되돌리기
        // 컴포넌트의 비활성화는 enabled = false
        //재활용 하기 위해 dead상태에서 다시 원상복구하기
        coll.enabled = true;
        // 리지드바디의 물리적 비활성화는 .simulated = false
        rigid.simulated = true;
        // 스프라이트 렌더러의 Sorting Order 감소
        spriter.sortingOrder = 2;
        // SetBool 함수를 통해 죽는 애니메이션 상태로 전환
        anim.SetBool("Dead", false);

        // 데미지를 받아 죽으면 health가 0이 되지만 Pooling에 의해 리젠되면 health도 원래대로 maxHealth가 되어야 한다
        health = maxHealth;
    }

    // Spawner에서 지정해준 데이터들을 함수로 받아야 한다
    // 초기 속성을 적용하는 함수 추가
    // 매개변수로 소환데이터 하나 지정
    public void Init(SpawnData data) //매개변수로 소환데이터 하나 지정
    {
        // 매개변수의 속성을 몬스터 속성 변경에 활용하기
        anim.runtimeAnimatorController = animCon[data.spriteType];
        speed = data.speed;
        maxHealth = data.health;
        health = data.health;
    }

    //무기와 적이 다았을때 이벤트 시스템
    void OnTriggerEnter2D(Collider2D collision)
    {
        // OnTriggerEnter2D 매개변수의 태그를 조건으로 활용
        //if (!collision.CompareTag("Bullet"))
        // 사망 로직이 연달아 실행되는 것을 방지하기 위해 조건 추가
        //collision = 지금 충돌한 상대
        if (!collision.CompareTag("Bullet") || !isLive) 
            //지금 충돌한게 "Bullet"이 맞습니까라고 확인 & 사망 로직이 연달아 실행되는 것을 방지하기 위해 조건 추가
            return;

        // Bullet 컴포넌트로 접근하여 데미지를 가져와 피격 계산하기
        health -= collision.GetComponent<Bullet>().damage; //맞은 무기의 데미지만큼 체력에서 깎기
        // GetCurrentAnimatorStateInfo : 현재 상태 정보를 가져오는 함수
        //코루틴은 StartCoroutine으로 호출
        StartCoroutine(KnockBack()); //StartCoroutine("KnockBack") 도 가능

        // 남은 체력을 조건으로 피격과 사망으로 로직을 나누기
        if (health > 0) {
            // .. Live, Hit Action

            // 몬스터 애니메이터의 피격 상태는 Hit Trigger로 제어되고 있음
            // 피격 부분에 애니메이터의 SetTrigger 함수를 호출하여 상태 변경
            anim.SetTrigger("Hit");
            //효과음을 재생할 부분마다 재생함수 호출
            AudioManager.instance.PlaySfx(AudioManager.Sfx.Hit);
        }
        else{
            // .. Die
            // 여러 로직을 제어하는 isLive 변수를 false로 변경
            isLive = false;
            // 컴포넌트의 비활성화는 enabled = false
            coll.enabled = false; //컴포넌트 비활성화
            // 리지드바디의 물리적 비활성화는 .simulated = false
            rigid.simulated = false; //rigidbody 물리적 비활성화
            spriter.sortingOrder = 1; //스프라이트 랜더러의 sorting order(보이는 순서) 감소
            anim.SetBool("Dead", true); //setBool 함수를 통해 죽는 애니메이션으로 전환

            //Dead();

            // 몬스터 사망 시 킬수 증가와 함께 경험치 함수 호출
            GameManager.instance.kill++;
            GameManager.instance.GetExp();

            // 언데드 사망 사운드는 게임 종료 시에는 나지 않도록 조건 추가
            // if문 안쪽 로직이 한 줄이라면 중괄호 생략 가능
            if (GameManager.instance.isLive){
                //효과음을 재생할 부분마다 재생함수 호출
                AudioManager.instance.PlaySfx(AudioManager.Sfx.Dead);}
        }

        
    }

    //코루틴 Coroutine : 생명 주기와 비동기처럼 실행되는 함수
    //IEnumerator : 코루틴만의 반환형 인터페이스
    // I라고 붙은 것들은 인터페이스라고 부른다
    IEnumerator KnockBack()
    {
        //yield : 코루틴의 반환 키워드
        // IEnumerator를 가진 코루틴에서만 yield 사용 가능
        // yield return을 통해 다양한 쉬는 시간을 지정
        // 1프레임 쉬기
        //yield return null;
        //yield return new WaitForSeconds(2f); //2초 쉬기 
        // new를 계속 쓰면 최적화 부분에서 안좋은 영향을 줄 수 있다
        yield return wait; //다음 하나의 물리 프레임 딜레이
        Vector3 playerPos = GameManager.instance.player.transform.position;
        // 플레이어 기준의 반대 방향 : 현재 위치 - 플레이어 위치
        Vector3 dirVec = transform.position - playerPos;
        // 리지드바디2D의 AddForce 함수로 힘 가하기
        rigid.AddForce(dirVec.normalized * 3, ForceMode2D.Impulse); //순간적인 힘이므로 ForceMode2D.Impulse속성 추가
                                        // 넉백 받는 힘 곱해서 추가

    }

    // 사망할 땐 SetActive 함수를 통한 오브젝트 비활성화
    void Dead()
    {
        gameObject.SetActive(false);
    }
}
