using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
//using UnityEditor.ShaderGraph.Internal;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    // 데미지와 관통 변수 선언
    public float damage;
    public int per;

    // 총알 스크립트에서 리지드바디2D 변수 선언 및 초기화
    Rigidbody2D rigid;

    void Awake()
    {
        rigid = GetComponent<Rigidbody2D>();
    }
 
    // 변수 초기화 함수 작성
    // 초기화 함수에 속도 관련 매개변수 추가
    public void Init(float damage, int per, Vector3 dir) //안에 있는거 받는 값 즉 parameter
    {
        this.damage = damage;  //this : 해당 클래스의 변수로 접근  
            // this.damage = Bullet 함수 내에 damage, 그냥 damage = Init함수에 받아오는 매개변수 damage
        this.per = per;

        // 관통이 -1(무한)보다 큰 것에 대해서는 속도 적용
        //if (per > -1)
        if (per >= 0) { //관통이 무한이 아니면 원거리
            //rigid.velocity = dir;
            // 속력을 곱해주어 총알이 날아가는 속도 증가시키기
            rigid.velocity = dir * 15f;  //속도를 제어
        }
    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        // 관통 로직 이전에 if문으로 조건 추가
        // || (OR) : 혹은, 좌측 우측 둘 중 하나만 true면 결과는 true
        if(!collision.CompareTag("Enemy") || per == -100) // || 는 or 이다
            return;

        // 관통 값이 하나씩 줄어들면서 -1이 되면 비활성화
        per--;

        // 관통 이후의 로직을 감싸는 if 조건을 안전하게 변경
        if (per < 0) {
            // 비활성화 이전에 미리 물리 속도 초기화
            rigid.velocity = Vector2.zero;
            gameObject.SetActive(false);
        }
    }

    //총알이 맵 밖으로 나가는 경우 없어지도록 처리 
    // OnTriggerExit2D 이벤트와 Area를 활용하여 쉽게 비활성화
    void OnTriggerExit2D(Collider2D collision)
    {
        //OnTriggerExit2D 이벤트와 Area를 활용하여 쉽게 비활성화
        if(!collision.CompareTag("Area") || per == -100)
            return;
        
        gameObject.SetActive(false);
    }
}
