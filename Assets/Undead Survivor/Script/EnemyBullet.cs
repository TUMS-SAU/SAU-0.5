using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : MonoBehaviour
{
    public float bulletSpeed = 5f;
    public float damage;
    public int per;
    Rigidbody2D rigid;
    Vector3 initialDirection; // 총알이 발사된 초기 방향
                              //Transform player;         // 플레이어의 Transform 컴포넌트 저장



    Vector3 moveDirection; // 총알의 이동 방향

    void Start()
    {
        // 랜덤한 방향을 설정 (최초에 한 번만 설정)
        SetRandomDirection();
    }

    void Update()
    {
        // 총알을 일정한 방향으로 이동
        transform.Translate(moveDirection * bulletSpeed * Time.deltaTime);
    }

    void SetRandomDirection()
    {
        // 랜덤한 각도를 생성
        float randomAngle = Random.Range(0f, 360f);
        // 랜덤한 각도를 이용하여 방향 벡터 계산
        moveDirection = Quaternion.Euler(0, 0, randomAngle) * Vector3.right;
    }
    /*void Update()
    {
       
        // 아래 묶음은 총알이 유도되어 계속 따라오는 코드
        player = GameManager.instance.player.transform;
        Vector3 targetPosition = player.position;
        Vector3 dir = targetPosition - transform.position; //크기가 포함된 방향 : 목표 위치(플레이어) - 적의 위치
        dir = dir.normalized; //normalized : 현재 벡터의 방향은 유지하고 크기를 1로 변환하는 속성
       // rigid.velocity = dir * bulletSpeed;
        transform.Translate(dir * bulletSpeed * Time.deltaTime);
    }*/

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }

    public void Init(float damage, int per, Vector3 dir) //안에 있는거 받는 값 즉 parameter
    {
        this.damage = damage;  //this : 해당 클래스의 변수로 접근  
                               // this.damage = Bullet 함수 내에 damage, 그냥 damage = Init함수에 받아오는 매개변수 damage
        this.per = per;

        if (per >= 0)
        { //관통이 무한이 아니면 원거리
            initialDirection = dir.normalized;
            //rigid.velocity = dir * 15f;  //속도를 제어
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Player") || per == -100) // || 는 or 이다
            return;

        // 플레이어에게 대미지 입히기
        Player player = collision.GetComponent<Player>();
        if (player != null)
        {
            player.TakeDamage(damage);
        }

        per--;

        if (per < 0)
        {
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
        else
        {
            // 다시 랜덤한 방향을 설정
            SetRandomDirection();
        }

    }

    //총알이 맵 밖으로 나가는 경우 없어지도록 처리 
    void OnTriggerExit2D(Collider2D collision)
    {
        //OnTriggerExit2D 이벤트와 Area를 활용하여 쉽게 비활성화
        if (!collision.CompareTag("Area") || per == -100)
            return;

        gameObject.SetActive(false);
    }

}
