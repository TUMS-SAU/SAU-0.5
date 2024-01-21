using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gear : MonoBehaviour
{
    //장비의 타입과 수치를 저장할 변수 선언
    public ItemData.ItemType type;
    public float rate;

    // Weapon과 동일하게 초기화 함수 작성
    //초기화 함수
    public void Init(ItemData data)
    {
        //Basic Set
        name = "Gear " + data.itemId; //Gear n 으로 만들어서
        transform.parent = GameManager.instance.player.transform; //플레이어 밑에 생성하고
        transform.localPosition = Vector3.zero; //위치도 초기화

        //Property Set
        type = data.itemType;
        rate = data.damages[0];
        //처음에 장비가 새롭게 추가될 때 로직 적용 함수를 호출
        ApplyGear(); 
    }

    // Weapon과 동일하게 레벨업 함수 작성
    public void LevelUp(float rate)
    {
        this.rate = rate;
        // 장비가 새롭게 추가되거나 레벨업 할 때 로직적용 함수를 호출
        ApplyGear();
    }

    //타입에 따라 적절하게 로직을 적용시켜주는 함수 추가
    void ApplyGear()
    {
        switch(type) {
            case ItemData.ItemType.Glove:
                //장갑은 무기 속도를 올림
                RateUp();
                break;
            case ItemData.ItemType.Shoe:
                //신발은 플레이어의 이동속도를 올림
                SpeedUp();
                break;
        }
    }

    // 장갑의 기능인 연사력을 올리는 함수 작성
    void RateUp() //공속을 올리는 함수
    {
        //플레이어로 올라가서 모든 weapon을 가져오기
        Weapon[] weapons = transform.parent.GetComponentsInChildren<Weapon>();

        // foreach문으로 하나씩 순회하면서 타입에 따라 속도 올리기
        foreach (Weapon weapon in weapons) {
            switch(weapon.id) {
                //근접무기
                case 0:
                    float speed = 150 * Character.WeaponSpeed;
                    weapon.speed = speed + (speed * rate);
                    break;
                //원거리 무기
                default:
                    speed = 0.5f * Character.WeaponRate;
                    weapon.speed = speed * (1f - rate); //0.5 = 1초에 2번, 작은 값일 수록 빨리 쏘는 것
                    break;
            }
        }
    }

    // 신발의 기능인 이동 속도를 올리는 함수 작성
    void SpeedUp() //이속을 올리는 함수
    {
        float speed = 3 * Character.Speed;
        GameManager.instance.player.speed = speed + speed * rate;

    }
}
